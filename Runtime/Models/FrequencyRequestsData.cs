using System;

namespace MultiplayerProtocol
{
    public class FrequencyRequestsData
    {
        public string TypeId { get; set; }

        public int CurrentQuantityRequestsInMinute { get; set; }

        public DateTime TimestampForCheckingFrequencyRequests { get; set; }
    }
}
