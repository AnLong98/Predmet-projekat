
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Documents.DomainModels
{
    public class Notification
    {
        public int ID { get; set; }
        public IconType NotificationType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public int UserID { get; set; }
        public int NotificationAnchorID { get; set; }
        public NotificationAnchor NotificationAnchor { get; set; }
    }
}
