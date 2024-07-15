using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Services;

namespace ReceiptProcessorTests
{
    public class GenericServiceTests
    {
        private readonly IRepository<TestEntity> _repository;
        private readonly GenericService<TestEntity> _genericService;

        public GenericServiceTests()
        {
            _repository = Substitute.For<IRepository<TestEntity>>();
            _genericService = new GenericService<TestEntity>(_repository);
        }

        [Fact]
        public void Add_CallsRepositoryAdd()
        {
            // Arrange
            var entity = new TestEntity { Id = Guid.NewGuid() };

            // Act
            _genericService.Add(entity);

            // Assert
            _repository.Received(1).Add(entity);
        }

        [Fact]
        public void Get_CallsRepositoryFindAndReturnsResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new TestEntity { Id = id };
            _repository.Find(id).Returns(entity);

            // Act
            var result = _genericService.Get(id);

            // Assert
            Assert.Equal(entity, result);
            _repository.Received(1).Find(id);
        }

        [Fact]
        public void Get_WhenEntityNotFound_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repository.Find(id).Returns((TestEntity)null);

            // Act
            var result = _genericService.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Remove_CallsRepositoryRemove()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            _genericService.Remove(id);

            // Assert
            _repository.Received(1).Remove(id);
        }

        [Fact]
        public void Add_NullEntity_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _genericService.Add(null));
        }

        [Fact]
        public void Get_DefaultGuid_ThrowsArgumentException()
        {
            // Arrange
            var defaultGuid = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _genericService.Get(defaultGuid));
        }

        [Fact]
        public void Remove_NonExistentId_DoesNotThrowException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            try
            {
                _genericService.Remove(nonExistentId);
                Assert.True(true); 
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Expected no exception, but got: {ex.Message}");
            }
        }

        [Fact]
        public void Add_CallsSave()
        {
            // Arrange
            var entity = new TestEntity { Id = Guid.NewGuid() };

            // Act
            _genericService.Add(entity);

            // Assert
            _repository.Received(1).Save();
        }

        [Fact]
        public void Remove_CallsSave()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            _genericService.Remove(id);

            // Assert
            _repository.Received(1).Save();
        }
    }

    // Test entity class
    public class TestEntity
    {
        public Guid Id { get; set; }
    }
}