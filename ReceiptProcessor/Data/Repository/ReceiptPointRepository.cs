using ReceiptProcessor.Data.Context;
using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;

namespace ReceiptProcessor.Data.Repository
{
    public class ReceiptPointRepository : Repository<ReceiptPoint>, IReceiptPointRepository
    {
        public ReceiptPointRepository(ReceiptProcessorDBContext context) : base(context) { }
    }
}
