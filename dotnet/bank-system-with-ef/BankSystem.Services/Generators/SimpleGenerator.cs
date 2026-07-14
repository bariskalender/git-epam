using System.Globalization;

namespace BankSystem.Services.Generators;

public sealed class SimpleGenerator : IUniqueNumberGenerator
{
    private static readonly SimpleGenerator instance = new();

    private int counter;

    private SimpleGenerator()
    {
    }

    public static SimpleGenerator Instance => instance;

    public string Generate()
    {
        this.counter++;
        return this.counter
            .ToString("X", CultureInfo.InvariantCulture)
            .PadLeft(32, '0');
    }
}
