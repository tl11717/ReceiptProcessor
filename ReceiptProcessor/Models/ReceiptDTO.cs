using System.ComponentModel.DataAnnotations;

namespace ReceiptProcessor.Models
{
    public class ReceiptDTO
    {
        /// <summary>
        /// The Id of the receiptDTO.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the retailer or store the receiptDTO is from.
        /// </summary>
        [Required]
        public string Retailer { get; set; }

        /// <summary>
        /// The date of the purchase printed on the receiptDTO.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public string PurchaseDate { get; set; }

        /// <summary>
        /// The time of the purchase printed on the receiptDTO. 24-hour time expected.
        /// </summary>
        [Required]
        [DataType(DataType.Time)]
        public string PurchaseTime { get; set; }

        /// <summary>
        /// The total amount paid on the receiptDTO.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public string Total { get; set; }

        /// <summary>
        /// The list of Items.
        /// </summary>
        [Required]
        public List<Item> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiptDTO"/> class.
        /// </summary>
        public ReceiptDTO()
        {
            Id = Guid.NewGuid();
        }
    }
}
