namespace webApplication.Contracts.group
{
    public class CreateGroupRequest
    {

        public string groupname { get; set; } = null!;

        public int? course { get; set; }

        public int curatorid { get; set; }

        public string? specialty { get; set; }

        public int institutionid { get; set; }
    }
}