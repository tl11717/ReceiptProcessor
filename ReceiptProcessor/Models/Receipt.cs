using System.ComponentModel.DataAnnotations;

namespace ReceiptProcessor.Models
{
    public class Receipt
    {
        /// <summary>
        /// The Id of the receipt.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the retailer or store the receipt is from.
        /// </summary>
        [Required]
        public string Retailer { get; set; }

        /// <summary>
        /// The date of the purchase printed on the receipt.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateOnly PurchaseDate { get; set; }

        /// <summary>
        /// The time of the purchase printed on the receipt. 24-hour time expected.
        /// </summary>
        [Required]
        [DataType(DataType.Time)]
        public TimeOnly PurchaseTime { get; set; }

        /// <summary>
        /// The total amount paid on the receipt.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public double Total { get; set; }

        /// <summary>
        /// The list of Items
        /// </summary>
        [Required]
        public List<Item> Items { get; set; }
    }
}
