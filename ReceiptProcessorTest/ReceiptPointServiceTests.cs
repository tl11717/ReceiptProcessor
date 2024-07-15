using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services;

namespace ReceiptProcessorTest
{
    public class ReceiptPointServiceTests
    {
        private readonly IReceiptPointRepository _mockRepository;
        private readonly ReceiptPointService _service;

        public ReceiptPointServiceTests()
        {
            _mockRepository = Substitute.For<IReceiptPointRepository>();
            _service = new ReceiptPointService(_mockRepository);
        }

        [Fact]
        public void ProcessReceiptPoint_ValidInput_ReturnsReceiptPoint()
        {
            // Arrange
            var id = Guid.NewGuid();
            var points = 100;

            // Act
            var result = _service.ProcessReceiptPoint(id, points);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(points, result.Points);
            _mockRepository.Received(1).Add(Arg.Is<ReceiptPoint>(rp => rp.Id == id && rp.Points == points));
        }

        [Fact]
        public void ProcessReceiptPoint_EmptyGuid_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _service.ProcessReceiptPoint(Guid.Empty, 100));
        }

        [Fact]
        public void ProcessReceiptPoint_NegativePoints_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _service.ProcessReceiptPoint(Guid.NewGuid(), -1));
        }

        [Fact]
        public void ProcessReceiptPoint_ExistingId_ThrowsInvalidOperationException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Find(id).Returns(new ReceiptPoint());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _service.ProcessReceiptPoint(id, 100));
        }
    }
}
