using BankSystem.Services.Generators;
using BankSystem.Services.Models;

namespace BankSystem.Services.Models.Accounts;

public abstract class BankAccount
{
    private readonly List<AccountCashOperation> operations = new();

    protected BankAccount(
        AccountOwner owner,
        string currencyCode,
        IUniqueNumberGenerator generator,
        decimal initialBalance = 0)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(currencyCode);
        ArgumentNullException.ThrowIfNull(generator);

        this.AccountOwner = owner;
        this.CurrencyCode = currencyCode;
        this.Number = generator.Generate();
        this.Balance = 0;
        this.BonusPoints = 0;

        owner.Add(this);

        if (initialBalance > 0)
        {
            this.Deposit(initialBalance, DateTime.MinValue, string.Empty);
        }
    }

    public AccountOwner AccountOwner { get; }

    public string CurrencyCode { get; }

    public string Number { get; }

    public decimal Balance { get; protected set; }

    public int BonusPoints { get; protected set; }

    public void Deposit(decimal amount, DateTime date, string note)
    {
        this.Balance += amount;
        this.BonusPoints += this.CalculateDepositRewardPoints(amount);
        this.operations.Add(new AccountCashOperation(amount, date, note));
    }

    public void Withdraw(decimal amount, DateTime date, string note)
    {
        if (amount > this.Balance)
        {
            throw new InvalidOperationException();
        }

        this.Balance -= amount;
        this.BonusPoints += this.CalculateWithdrawRewardPoints(amount);
        this.operations.Add(new AccountCashOperation(-amount, date, note));
    }

    public IReadOnlyList<AccountCashOperation> GetAllOperations()
    {
        return this.operations.AsReadOnly();
    }

    protected abstract int CalculateDepositRewardPoints(decimal amount);

    protected abstract int CalculateWithdrawRewardPoints(decimal amount);
}
