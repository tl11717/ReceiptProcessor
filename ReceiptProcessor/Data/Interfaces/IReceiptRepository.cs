using ReceiptProcessor.Models;

namespace ReceiptProcessor.Data.Interfaces
{
    public interface IReceiptRepository : IRepository<Receipt>
    {
        Dictionary<Guid, Receipt> FindAll();
    }
}
