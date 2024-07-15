using ReceiptProcessor.Models;

namespace ReceiptProcessor.Services.Interface
{
    public interface IReceiptService : IGenericService<Receipt>
    {
        Dictionary<Guid, Receipt> GetAll();

        int CalculateReceiptPoints(Receipt receipt);
    }
}
