namespace webApplication.Contracts.group
{
    public class GetGroupResponse
    {
        public int groupid { get; set; }

        public string groupname { get; set; } = null!;

        public int? course { get; set; }

        public int curatorid { get; set; }

        public string? specialty { get; set; }

        public int institutionid { get; set; }
    }
}