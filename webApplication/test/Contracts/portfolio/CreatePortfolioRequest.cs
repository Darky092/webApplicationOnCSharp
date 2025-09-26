namespace webApplication.Contracts.portfolio
{
    public class CreatePortfolioRequest
    {
        public int userid { get; set; }

        public string achievement { get; set; } = null!;

    }
}