using BankSystem.Services.Generators;

namespace BankSystem.Services.Models.Accounts;

public class StandardAccount : BankAccount
{
    public StandardAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator generator)
        : base(owner, currencyCode, generator)
    {
    }

    public StandardAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator generator, decimal initialBalance)
        : base(owner, currencyCode, generator, initialBalance)
    {
    }

    public StandardAccount(AccountOwner owner, string currencyCode, Func<string> generator)
        : base(owner, currencyCode, new DelegateNumberGenerator(generator))
    {
    }

    public StandardAccount(AccountOwner owner, string currencyCode, Func<string> generator, decimal initialBalance)
        : base(owner, currencyCode, new DelegateNumberGenerator(generator), initialBalance)
    {
    }

    protected override int CalculateDepositRewardPoints(decimal amount)
    {
        return ((int)this.Balance / 100) + ((int)amount / 1000);
    }

    protected override int CalculateWithdrawRewardPoints(decimal amount)
    {
        return ((int)this.Balance / 100) + ((int)amount / 1000);
    }

    private sealed class DelegateNumberGenerator : IUniqueNumberGenerator
    {
        private readonly Func<string> generator;

        public DelegateNumberGenerator(Func<string> generator)
        {
            this.generator = generator;
        }

        public string Generate()
        {
            return this.generator();
        }
    }
}
