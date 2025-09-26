namespace webApplication.Contracts.city
{
    public class CreateCityRequest
    {

        public string cityname { get; set; } = null!;

        public string? postalcode { get; set; }

        public string? country { get; set; }
    }
}