namespace webApplication.Contracts.room_equipment
{
    public class UpdateRoomEquipmentRequest
    {
        public int roomid { get; set; }

        public string equipment { get; set; } = null!;

    }
}