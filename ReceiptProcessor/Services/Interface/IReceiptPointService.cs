using ReceiptProcessor.Models;

namespace ReceiptProcessor.Services.Interface
{
    public interface IReceiptPointService : IGenericService<ReceiptPoint>
    {
        ReceiptPoint ProcessReceiptPoint(Guid id, int points);
    }
}
