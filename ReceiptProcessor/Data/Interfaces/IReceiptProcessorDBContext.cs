using Microsoft.EntityFrameworkCore;

namespace ReceiptProcessor.Data.Interfaces
{
    public interface IReceiptProcessorDBContext
    {
        DbSet<T> Set<T>() where T : class;

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}