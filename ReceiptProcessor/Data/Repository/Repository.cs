using Microsoft.EntityFrameworkCore;
using ReceiptProcessor.Data.Interfaces;

namespace ReceiptProcessor.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IReceiptProcessorDBContext _context;

        public Repository(IReceiptProcessorDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void Add(T value)
        {
            _dbSet.Add(value);
        }

        public virtual T? Find(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Remove(Guid id)
        {
            var obj = _dbSet.Find(id);
            if (obj != null)
            {
                _dbSet.Remove(obj);
            }
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
