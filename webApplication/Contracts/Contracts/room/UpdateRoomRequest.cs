namespace webApplication.Contracts.room
{
    public class UpdateRoomRequest
    {
        public int roomid { get; set; }

        public string roomnumber { get; set; } = null!;

        public int institutionid { get; set; }
    }
}