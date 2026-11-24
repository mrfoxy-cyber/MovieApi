namespace MovieAPI.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public required int CurrencyId { get; set; }

        public decimal Amount { get; set; } 
    }
}
