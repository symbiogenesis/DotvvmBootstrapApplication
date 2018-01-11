using RingDownConsole.Interfaces;

namespace RingDownConsole.Models
{
    public class Location : IIdentifiable
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
