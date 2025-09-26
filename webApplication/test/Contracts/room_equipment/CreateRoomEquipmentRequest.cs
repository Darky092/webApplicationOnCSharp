namespace webApplication.Contracts.room_equipment
{
    public class CreateRoomEquipmentRequest
    {
        public int roomid { get; set; }

        public string equipment { get; set; } = null!;

    }
}