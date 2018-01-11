using RingDownConsole.Interfaces;

namespace RingDownConsole.Models
{
    public class Status : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string ImageName { get; set; }

        public bool IsActive { get; set; }
    }
}
