using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using MockTesting;

namespace MockTesting.Tests
{
    [TestFixture]
    public class TransactionProcessorTests
    {
        private Mock<IPermissionService> _permission;
        private Mock<IAccountService> _account;
        private Mock<ITransactionService> _transaction;
        private Mock<ILogger> _logger;
        private List<string> _logs;
        private TransactionProcessor _sut;

        private static readonly Regex TxIdRegex =
            new Regex(@"Transaction\s+([0-9a-fA-F\-]{36})\s+", RegexOptions.Compiled);

        [SetUp]
        public void Setup()
        {
            _permission = new Mock<IPermissionService>(MockBehavior.Strict);
            _account = new Mock<IAccountService>(MockBehavior.Strict);
            _transaction = new Mock<ITransactionService>(MockBehavior.Strict);

            _logs = new List<string>();
            _logger = new Mock<ILogger>(MockBehavior.Strict);
            _logger
                .Setup(l => l.Log(It.IsAny<string>()))
                .Callback<string>(msg => _logs.Add(msg));

            _sut = new TransactionProcessor(
                _permission.Object,
                _account.Object,
                _transaction.Object,
                _logger.Object);
        }

        [Test]
        public void ProcessTransfer_SuccessfulTransfer_LogsAndReturnsTrue()
        {
            int from = 1, to = 2;
            decimal amount = 50m;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(true);
            _account.Setup(a => a.GetBalance(from)).Returns(100m);
            _transaction.Setup(t => t.Transfer(from, to, amount));

            bool result = _sut.ProcessTransfer(from, to, amount);

            Assert.That(result, Is.True);

            _permission.Verify(p => p.HasTransferPermission(from), Times.Once);
            _account.Verify(a => a.GetBalance(from), Times.Once);
            _transaction.Verify(t => t.Transfer(from, to, amount), Times.Once);

            Assert.That(_logs.Count, Is.EqualTo(2));
            var txId = ExtractTxId(_logs[0]);
            Assert.That(_logs[0], Is.EqualTo($"Transaction {txId} started: Transfer {amount} from {from} to {to}"));
            Assert.That(_logs[1], Is.EqualTo($"Transaction {txId} completed successfully"));
        }

        [Test]
        public void ProcessTransfer_NoPermission_LogsAndReturnsFalse()
        {
            int from = 1, to = 2;
            decimal amount = 10m;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(false);

            bool result = _sut.ProcessTransfer(from, to, amount);

            Assert.That(result, Is.False);

            _account.Verify(a => a.GetBalance(It.IsAny<int>()), Times.Never);
            _transaction.Verify(t => t.Transfer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);

            var txId = ExtractTxId(_logs[0]);
            Assert.That(_logs[1], Is.EqualTo($"Transaction {txId} failed: Permission denied"));
        }

        [Test]
        public void ProcessTransfer_InsufficientBalance_LogsAndReturnsFalse()
        {
            int from = 1, to = 2;
            decimal amount = 200m;
            decimal balance = 100m;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(true);
            _account.Setup(a => a.GetBalance(from)).Returns(balance);

            bool result = _sut.ProcessTransfer(from, to, amount);

            Assert.That(result, Is.False);

            _account.Verify(a => a.GetBalance(from), Times.Exactly(2));
            _transaction.Verify(t => t.Transfer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);

            var txId = ExtractTxId(_logs[0]);
            Assert.That(_logs[1],
                Is.EqualTo($"Transaction {txId} failed: Insufficient funds. Current balance: {balance}"));
        }

        [Test]
        public void ProcessTransfer_TransactionThrowsException_LogsAndReturnsFalse()
        {
            int from = 1, to = 2;
            decimal amount = 50m;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(true);
            _account.Setup(a => a.GetBalance(from)).Returns(100m);

            _transaction
                .Setup(t => t.Transfer(from, to, amount))
                .Throws(new InvalidOperationException("Connection failed"));

            bool result = _sut.ProcessTransfer(from, to, amount);

            Assert.That(result, Is.False);

            var txId = ExtractTxId(_logs[0]);
            Assert.That(_logs[1],
                Is.EqualTo($"Transaction {txId} failed with error: Connection failed"));
        }

        [Test]
        public void ProcessTransfer_InvalidFromUserId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.ProcessTransfer(0, 2, 10m));
        }

        [Test]
        public void ProcessTransfer_InvalidToUserId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.ProcessTransfer(1, 0, 10m));
        }

        [Test]
        public void ProcessTransfer_SameUserIds_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.ProcessTransfer(1, 1, 10m));
        }

        [Test]
        public void ProcessTransfer_InvalidAmount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.ProcessTransfer(1, 2, 0m));
        }

        [Test]
        public void Constructor_NullDependencies_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TransactionProcessor(
                null, _account.Object, _transaction.Object, _logger.Object));

            Assert.Throws<ArgumentNullException>(() => new TransactionProcessor(
                _permission.Object, null, _transaction.Object, _logger.Object));

            Assert.Throws<ArgumentNullException>(() => new TransactionProcessor(
                _permission.Object, _account.Object, null, _logger.Object));

            Assert.Throws<ArgumentNullException>(() => new TransactionProcessor(
                _permission.Object, _account.Object, _transaction.Object, null));
        }

        [Test]
        public void ProcessTransfer_WithMinValidValues_ShouldSucceed()
        {
            int from = 1, to = 2;
            decimal amount = 0.0000000000000000000000000001m;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(true);
            _account.Setup(a => a.GetBalance(from)).Returns(amount);
            _transaction.Setup(t => t.Transfer(from, to, amount));

            bool result = _sut.ProcessTransfer(from, to, amount);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ProcessTransfer_WithMaxIntValues_ShouldSucceed()
        {
            int from = int.MaxValue - 1;
            int to = int.MaxValue;
            decimal amount = decimal.MaxValue;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(true);
            _account.Setup(a => a.GetBalance(from)).Returns(decimal.MaxValue);
            _transaction.Setup(t => t.Transfer(from, to, amount));

            bool result = _sut.ProcessTransfer(from, to, amount);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ProcessTransfer_TransactionId_ShouldBeUnique()
        {
            int from = 1, to = 2;
            decimal amount = 10m;

            _permission.Setup(p => p.HasTransferPermission(from)).Returns(true);
            _account.Setup(a => a.GetBalance(from)).Returns(100m);
            _transaction.Setup(t => t.Transfer(from, to, amount));

            _sut.ProcessTransfer(from, to, amount);
            _sut.ProcessTransfer(from, to, amount);

            var firstId = ExtractTxId(_logs[0]);
            var secondId = ExtractTxId(_logs[2]);

            Assert.That(firstId, Is.Not.EqualTo(secondId));
        }

        private static Guid ExtractTxId(string message)
        {
            var match = TxIdRegex.Match(message);
            if (!match.Success)
                throw new InvalidOperationException($"Transaction ID not found in log message: {message}");

            return Guid.Parse(match.Groups[1].Value);
        }
    }
}
