namespace ReceiptProcessor.Models
{
    public class ReceiptPoint
    {
        /// <summary>
        /// The Id of the Receipt (also used as the reference for the ReceiptPoint).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The amount of points the receipt received.
        /// </summary>
        public int Points { get; set; }
    }
}
