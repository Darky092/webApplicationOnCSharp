namespace webApplication.Contracts.institution
{
    public class CreateInstitutionRequest
    {

        public string institutionname { get; set; } = null!;

        public string street { get; set; } = null!;

        public string? phone { get; set; }

        public string? website { get; set; }

        public int? cityid { get; set; }
    }
}
