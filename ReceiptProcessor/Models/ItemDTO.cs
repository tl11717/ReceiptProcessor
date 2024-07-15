using System.ComponentModel.DataAnnotations;

namespace ReceiptProcessor.Models
{
    public class ItemDTO
    {
        /// <summary>
        /// The Id of the ItemDTO.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Short Product Description for the item.
        /// </summary>
        [Required]
        public string ShortDescription { get; set; }

        /// <summary>
        /// The total price payed for this item.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public string Price { get; set; }
    }
}
