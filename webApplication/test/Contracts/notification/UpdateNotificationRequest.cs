namespace webApplication.Contracts.notification
    {
    public class UpdateNotificationRequest
        {
        public int notificationid { get; set; }

        public int userid { get; set; }

        public DateTime? createdat { get; set; }

        public bool? isread { get; set; }

        public string note { get; set; } = null!;
        }
    }