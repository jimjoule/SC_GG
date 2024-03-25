namespace SC_WEBAPISOCKET.Models
{
    public class Reg
    {
        public Guid? Id { get; set; }
        public float temp { get; set; }
        public float light { get; set; }
        public DateTime timestamp { get; set; }
        public string deviceId { get; set; }

    }
}
