using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Services.Interface;

namespace ReceiptProcessor.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public GenericService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _repository.Add(entity);
            _repository.Save();
        }

        public virtual T? Get(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid ID", nameof(id));
            return _repository.Find(id);
        }

        public virtual void Remove(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid ID", nameof(id));
            _repository.Remove(id);
            _repository.Save();
        }
    }
}
