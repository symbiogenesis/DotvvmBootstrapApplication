namespace RingDownConsole.Interfaces
{
    public interface IViewModel
    {
        string PageTitle { get; set; }
        string ErrorMessage { get; set; }
        bool ErrorShow { get; set; }
    }
}