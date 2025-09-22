namespace webApplication.Contracts.portfolio
{
    public class GetPortfolioResponse
    {
        public int userid { get; set; }

        public string achievement { get; set; } = null!;

        public DateTime? addedat { get; set; }
    }
}
