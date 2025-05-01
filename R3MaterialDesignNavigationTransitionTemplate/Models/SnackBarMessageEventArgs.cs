namespace R3MaterialDesignNavigationTransitionTemplate.Models
{
    public class SnackBarMessageEventArgs(string message) : EventArgs
    {
        public string Message => message;
    }
}
