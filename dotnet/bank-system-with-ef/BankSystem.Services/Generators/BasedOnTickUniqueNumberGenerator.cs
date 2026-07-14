using System.Globalization;

namespace BankSystem.Services.Generators;

public class BasedOnTickUniqueNumberGenerator : IUniqueNumberGenerator
{
    private long seed;

    public BasedOnTickUniqueNumberGenerator(DateTime dateTime)
    {
        this.seed = dateTime.Ticks;
    }

    public string Generate()
    {
        this.seed++;
        return this.seed
            .ToString("X", CultureInfo.InvariantCulture)
            .PadLeft(32, '0');
    }
}
