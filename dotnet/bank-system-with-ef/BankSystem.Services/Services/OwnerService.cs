using System.Diagnostics.CodeAnalysis;
using BankSystem.EF.Entities;

namespace BankSystem.Services.Services;

public sealed class OwnerService : IDisposable
{
    private readonly BankContext context;
    private bool disposed;

    public OwnerService(BankContext context)
    {
        this.context = context;
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Method name is required by tests.")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Method intentionally uses instance state.")]
    public IReadOnlyList<object> GetAccountOwnersTotalBalance()
    {
        ArgumentNullException.ThrowIfNull(this.context);
        return new List<object>();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.context.Dispose();
        }

        this.disposed = true;
    }
}
