namespace ReceiptProcessor.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T value);

        T? Find(Guid id);

        void Remove(Guid id);

        void Save();
    }
}
