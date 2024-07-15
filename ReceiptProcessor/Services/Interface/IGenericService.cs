namespace ReceiptProcessor.Services.Interface
{
    public interface IGenericService<T> where T : class
    {
        void Add(T value);

        T? Get(Guid id);

        void Remove(Guid id);
    }
}
