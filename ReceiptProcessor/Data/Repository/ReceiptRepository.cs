using ReceiptProcessor.Models;
using Microsoft.EntityFrameworkCore;
using ReceiptProcessor.Data.Interfaces;

namespace ReceiptProcessor.Data.Repository
{
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(IReceiptProcessorDBContext context) : base(context) { }

        public Dictionary<Guid, Receipt> FindAll()
        {
            return _dbSet.Include(r => r.Items).ToDictionary(r => r.Id);
        }
    }
}
