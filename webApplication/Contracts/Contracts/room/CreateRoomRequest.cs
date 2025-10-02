namespace webApplication.Contracts.room
{
    public class CreateRoomRequest
    {

        public string roomnumber { get; set; } = null!;

        public int institutionid { get; set; }
    }
}