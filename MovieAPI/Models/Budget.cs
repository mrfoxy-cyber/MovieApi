namespace MovieAPI.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public required int CurrencyId { get; set; }

        public Currencies Currency { get; set; } = null!; //navigation

        public decimal Amount { get; set; } 
    }
}
