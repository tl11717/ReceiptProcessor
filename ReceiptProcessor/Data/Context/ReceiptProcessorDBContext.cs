using Microsoft.EntityFrameworkCore;
using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Models;

namespace ReceiptProcessor.Data.Context
{
    public class ReceiptProcessorDBContext : DbContext, IReceiptProcessorDBContext
    {
        public ReceiptProcessorDBContext(DbContextOptions<ReceiptProcessorDBContext> options) : base(options)
        {

        }
        public DbSet<ReceiptPoint> ReceiptPoints { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Item> Items { get; set; }

        DbSet<T> IReceiptProcessorDBContext.Set<T>()
        {
            return base.Set<T>();
        }

        Task<int> IReceiptProcessorDBContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        int IReceiptProcessorDBContext.SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
