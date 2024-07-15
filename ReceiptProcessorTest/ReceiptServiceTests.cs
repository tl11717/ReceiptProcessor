using NSubstitute;
using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services;

namespace ReceiptProcessorTest
{
    public class ReceiptServiceTests
    {
        private readonly ReceiptService _receiptService;
        private readonly IReceiptRepository _mockIReceiptRepository;

        public ReceiptServiceTests()
        {
            _mockIReceiptRepository = Substitute.For<IReceiptRepository>();
            _receiptService = new ReceiptService(_mockIReceiptRepository);

        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(9)]
        public void GetAllReceipts_ReturnsAllReceipts(int receiptCount)
        {
            // Arrange
            var receipts = new Dictionary<Guid, Receipt>();
            for (int i = 0; i < receiptCount; i++)
            {
                var receiptId = Guid.NewGuid();
                receipts.Add(receiptId, new Receipt());
            }
            _mockIReceiptRepository.FindAll().Returns(receipts);

            // Act
            var result = _receiptService.GetAll();

            // Assert
            Assert.Equal(receiptCount, result.Count);
            Assert.Equal(receipts, result);
            foreach (var receipt in receipts)
            {
                Assert.True(result.ContainsKey(receipt.Key));
            }
        }

        [Fact]
        public void GetAllReceipts_WhenNoReceipts_ReturnsEmptyDictionary()
        {
            // Arrange
            var emptyReceipts = new Dictionary<Guid, Receipt>();
            _mockIReceiptRepository.FindAll().Returns(emptyReceipts);

            // Act
            var result = _receiptService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllReceipts_WhenRepositoryReturnsNull_ReturnsEmptyDictionary()
        {
            // Arrange
            _mockIReceiptRepository.FindAll().Returns((Dictionary<Guid, Receipt>)null);

            // Act
            var result = _receiptService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CalculateReceiptPoints_Returns28Points()
        {
            // Arrange
            var receipt = new Receipt()
            {
                Retailer = "Target",
                PurchaseDate = new DateOnly(2022, 1, 1),
                PurchaseTime = new TimeOnly(13, 1),
                Items = new List<Item> {
                    new Item { ShortDescription = "Mountain Dew 12PK", Price = 6.49 },
                    new Item { ShortDescription = "Emils Cheese Pizza", Price = 12.25 },
                    new Item { ShortDescription = "Knorr Creamy Chicken", Price = 1.26 },
                    new Item { ShortDescription = "Doritos Nacho Cheese", Price = 3.35 },
                    new Item { ShortDescription = "Klarbrunn 12-PK 12 FL OZ", Price = 12.00 }
                },
                Total = 35.35
            };

            // Act
            var result = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(28, result);
        }

        [Fact]
        public void CalculateReceiptPoints_NullReceipt_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _receiptService.CalculateReceiptPoints(null));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 5)]
        [InlineData(3, 5)]
        [InlineData(4, 10)]
        [InlineData(5, 10)]
        [InlineData(6, 15)]
        public void CalculateReceiptPoints_ItemCount_ReturnsExpectedPoints(int itemCount, int expectedPoints)
        {
            // Arrange
            var items = new List<Item>();
            for (int i = 0; i < itemCount; i++)
            {
                items.Add(new Item { ShortDescription = $"Item {i}", Price = 0 });
            }
            var receipt = CreateReceiptForItems(items);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(expectedPoints, points);
        }

        [Theory]
        [InlineData("ABC", 1.00, 1)]
        [InlineData("ABCDEF", 2.00, 1)]
        [InlineData("A", 10.00, 0)]
        [InlineData("AB", 10.00, 0)]
        [InlineData("ABCDEFGHI", 10.00, 2)]
        public void CalculateReceiptPoints_ItemShortDescriptionLength_ReturnsExpectedPoints(string shortDescription, int price, int expectedPoints)
        {
            // Arrange
            var items = new List<Item>
            {
                new Item { ShortDescription = shortDescription, Price = price }
            };
            var receipt = CreateReceiptForItems(items);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(expectedPoints, points);
        }

        [Theory]
        [InlineData(2024, 1, 2)]
        [InlineData(2020, 1, 20)]
        [InlineData(2023, 1, 8)]
        [InlineData(2025, 1, 16)]
        public void CalculateReceiptPoints_PurchaseDateWhereDayIsEven_ReturnsOPoints(int year, int month, int day)
        {
            // Arrange
            var receipt = CreateReceiptForPurchaseDate(new DateOnly(year, month, day));

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(0, points);
        }

        [Theory]
        [InlineData(2024, 1, 1)]
        [InlineData(2023, 1, 5)]
        [InlineData(2020, 1, 11)]
        [InlineData(2025, 1, 25)]
        public void CalculateReceiptPoints_PurchaseDateWhereDayIsOdd_Returns6Points(int year, int month, int day)
        {
            // Arrange
            var receipt = CreateReceiptForPurchaseDate(new DateOnly(year, month, day));

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(6, points);
        }

        [Theory]
        [InlineData(1, 59)]
        [InlineData(13, 59)]
        [InlineData(4, 1)]
        [InlineData(16, 1)]
        public void CalculateReceiptPoints_PurchaseTime_Returns0Points(int hour, int minute)
        {
            // Arrange
            var receipt = CreateReceiptForPurchaseTime(new TimeOnly(hour, minute));

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(0, points);
        }

        [Theory]
        [InlineData(14, 0)]
        [InlineData(14, 30)]
        [InlineData(15, 0)]
        [InlineData(15, 30)]
        [InlineData(16, 0)]
        public void CalculateReceiptPoints_PurchaseTime_Returns10Points(int hour, int minute)
        {
            // Arrange
            var receipt = CreateReceiptForPurchaseTime(new TimeOnly(hour, minute));

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(10, points);
        }

        [Theory]
        [InlineData("")]
        [InlineData("!@#$%")]
        [InlineData("!@# $%^ &*(")]
        public void CalculateReceiptPoints_Retailer_Returns0Points(string retailer)
        {
            // Arrange
            var receipt = CreateReceiptForRetailer(retailer);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(0, points);
        }

        [Theory]
        [InlineData(6, "AaBbCc")]
        [InlineData(10, "ABCDE abcde")]
        [InlineData(8, "Aa+Bb+Cc+Dd")]
        [InlineData(11, "Aa + Bb + Cc + Dd + Ee + F")]
        public void CalculateReceiptPoints_Retailer_ReturnsExpectedPoints(int expectedPoints, string retailer)
        {
            // Arrange
            var receipt = CreateReceiptForRetailer(retailer);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(expectedPoints, points);
        }

        [Theory]
        [InlineData(.01)]
        [InlineData(80.30)]
        [InlineData(23.40)]
        public void CalculateReceiptPoints_Total_Returns0Points(double total)
        {
            // Arrange
            var receipt = CreateReceiptForTotal(total);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(0, points);
        }

        [Theory]
        [InlineData(15.25)]
        [InlineData(1.50)]
        [InlineData(234.75)]
        public void CalculateReceiptPoints_Total_Returns25Points(double total)
        {
            // Arrange
            var receipt = CreateReceiptForTotal(total);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(25, points);
        }

        [Theory]
        [InlineData(1.00)]
        [InlineData(2.00)]
        [InlineData(10.00)]
        public void CalculateReceiptPoints_Total_Returns75Points(double total)
        {
            // Arrange
            var receipt = CreateReceiptForTotal(total);

            // Act
            var points = _receiptService.CalculateReceiptPoints(receipt);

            // Assert
            Assert.Equal(75, points);
        }

        private Receipt CreateReceiptForItems(List<Item> items)
        {
            return new Receipt
            {
                Retailer = "",
                PurchaseDate = new DateOnly(2023, 7, 2),
                PurchaseTime = new TimeOnly(12, 0),
                Items = items,
                Total = 0.01
            };
        }

        private Receipt CreateReceiptForPurchaseDate(DateOnly purchaseDate)
        {
            return new Receipt
            {
                Retailer = "",
                PurchaseDate = purchaseDate,
                PurchaseTime = new TimeOnly(12, 0),
                Items = new List<Item>(),
                Total = 0.01
            };
        }

        private Receipt CreateReceiptForPurchaseTime(TimeOnly purchaseTime)
        {
            return new Receipt
            {
                Retailer = "",
                PurchaseDate = new DateOnly(2023, 7, 2),
                PurchaseTime = purchaseTime,
                Items = new List<Item>(),
                Total = 0.01
            };
        }

        private Receipt CreateReceiptForRetailer(string retailer)
        {
            return new Receipt
            {
                Retailer = retailer,
                PurchaseDate = new DateOnly(2023, 7, 2),
                PurchaseTime = new TimeOnly(12, 0),
                Items = new List<Item>(),
                Total = 0.01
            };
        }

        private Receipt CreateReceiptForTotal(double total)
        {
            return new Receipt
            {
                Retailer = "",
                PurchaseDate = new DateOnly(2023, 7, 2),
                PurchaseTime = new TimeOnly(12, 0),
                Items = new List<Item>(),
                Total = total
            };
        }
    }
}