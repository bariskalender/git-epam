using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
{
    public ReceiptDetailRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
    {
        return await this.Context.ReceiptsDetails
            .Include(rd => rd.Product)
                .ThenInclude(p => p.Category)
            .Include(rd => rd.Receipt)
                .ThenInclude(r => r.Customer)
                    .ThenInclude(c => c.Person)
            .ToListAsync();
    }

    public async Task<ReceiptDetail?> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.ReceiptsDetails
            .Include(rd => rd.Product)
                .ThenInclude(p => p.Category)
            .Include(rd => rd.Receipt)
                .ThenInclude(r => r.Customer)
                    .ThenInclude(c => c.Person)
            .FirstOrDefaultAsync(rd => rd.Id == id);
    }
}
