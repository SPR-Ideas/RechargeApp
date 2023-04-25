namespace RechargeApp.Models
{
    public class RechargeHistory
    {
        public int Id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int validity { get; set; }
        public DateTime timestamp { get; set; }
    }
}
