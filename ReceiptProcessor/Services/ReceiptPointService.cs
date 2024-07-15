using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services.Interface;

namespace ReceiptProcessor.Services
{
    public class ReceiptPointService : GenericService<ReceiptPoint>, IReceiptPointService
    {
        private readonly IReceiptPointRepository _receiptPointRepository;

        public ReceiptPointService(IReceiptPointRepository receiptPointRepository) : base(receiptPointRepository)
        {
            _receiptPointRepository = receiptPointRepository;
        }

        public ReceiptPoint ProcessReceiptPoint(Guid id, int points)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid receipt ID", nameof(id));
            }

            if (points < 0)
            {
                throw new ArgumentException("Points cannot be negative", nameof(points));
            }

            var receiptPoint = new ReceiptPoint { Id = id, Points = points };

            if (_receiptPointRepository.Find(id) != null)
            {
                throw new InvalidOperationException($"A receipt point with ID {id} already exists");
            }

            Add(receiptPoint);

            return receiptPoint;
        }
    }
}
