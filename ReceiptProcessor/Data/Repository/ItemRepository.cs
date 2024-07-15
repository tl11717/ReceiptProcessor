using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;

namespace ReceiptProcessor.Data.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(IReceiptProcessorDBContext context) : base(context) { }
    }
}
