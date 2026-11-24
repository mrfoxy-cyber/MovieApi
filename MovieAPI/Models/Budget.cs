namespace MovieAPI.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }

        public Currencies Currency { get; set; } = null!; //navigation

        public decimal Amount { get; set; } 
    }
}
