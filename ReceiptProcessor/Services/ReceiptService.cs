using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services.Interface;

namespace ReceiptProcessor.Services
{
    public class ReceiptService : GenericService<Receipt>, IReceiptService
    {
        private Receipt _receipt;
        private readonly IReceiptRepository _receiptRepository;

        public ReceiptService(IReceiptRepository receiptRepository) : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public Dictionary<Guid, Receipt> GetAll()
        {
            return _receiptRepository.FindAll() ?? new Dictionary<Guid, Receipt>();
        }

        public int CalculateReceiptPoints(Receipt receipt)
        {
            if (receipt == null) throw new ArgumentNullException(nameof(receipt));

            _receipt = receipt;
            int totalPoints = 0;

            totalPoints += ProcessPointsForRetailer();
            totalPoints += ProcessPointsForTotal();
            totalPoints += ProcessPointsForItems();
            totalPoints += ProcessPointsForDate();
            totalPoints += ProcessPointsForTime();

            return totalPoints;
        }

        private int ProcessPointsForRetailer()
        {
            return _receipt.Retailer.Count(char.IsLetterOrDigit);
        }

        private int ProcessPointsForTotal()
        {
            if (_receipt.Total <= 0) return 0;

            int points = 0;

            if (_receipt.Total == Math.Floor(_receipt.Total))
            {
                points += 50;
            }

            if (_receipt.Total % .25 == 0)
            {
                points += 25;
            }

            return points;
        }

        private int ProcessPointsForItems()
        {
            if (!_receipt.Items.Any())
            {
                return 0;
            }

            int points = (_receipt.Items.Count / 2) * 5;

            points += _receipt.Items
                .Where(item => item.ShortDescription.Trim().Length % 3 == 0)
                .Sum(item => (int)Math.Ceiling(item.Price * 0.2));

            return points;
        }

        private int ProcessPointsForDate()
        {
            return _receipt.PurchaseDate.Day % 2 != 0 ? 6 : 0;
        }

        private int ProcessPointsForTime()
        {
            var purchaseTime = _receipt.PurchaseTime;
            var startTime = new TimeOnly(14, 0);
            var endTime = new TimeOnly(16, 0);

            return (purchaseTime >= startTime && purchaseTime <= endTime) ? 10 : 0;
        }
    }
}
