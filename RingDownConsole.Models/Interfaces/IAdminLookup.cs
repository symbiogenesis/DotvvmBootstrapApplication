namespace RingDownConsole.Interfaces
{
    public interface IAdminLookup : IIdentifiable
    {
        string Name { get; set; }

        bool IsDefault { get; set; }

        bool IsSaved { get; set; }

        string Comments { get; set; }
    }
}