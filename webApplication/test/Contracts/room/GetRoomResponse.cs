namespace webApplication.Contracts.room
    {
    public class GetRoomResponse
        {
        public int roomid { get; set; }

        public string roomnumber { get; set; } = null!;

        public int institutionid { get; set; }
        }
    }