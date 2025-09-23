namespace webApplication.Contracts.city
    {
    public class GetCityResponse
        {
        public int cityid { get; set; }

        public string cityname { get; set; } = null!;

        public string? postalcode { get; set; }

        public string? country { get; set; }
        }
    }