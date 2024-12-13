namespace book_my_show.Model
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        public string? MovieName { get; set; } = string.Empty;
        public DateTime? MovieTiming { get; set; } 
        public decimal AmountPaid { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? Username { get; set; } = string.Empty;
        public int? SeatsBooked { get; set; }

    }
     
}
