using R3MaterialDesignNavigationTransitionTemplate.Models;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels
{
    public class BoxViewModelBase : ViewModelBase, IBoxViewModel
    {

        private readonly MainWindowViewModel _parent;
        public BoxViewModelBase()
        {
            _parent = App.GetService<MainWindowViewModel>()!;
        }

        public static string Key => throw new NotImplementedException();

        public void OnErrorOccurred(ErrorOccurredEventArgs e)
        {
            _parent.OnErrorOccurred(this, e);
        }

        public void OnMessage(MessageEventArgs e)
        {
            _parent.OnMessage(this, e);
        }

        public void OnSnackBarMessage(SnackBarMessageEventArgs e)
        {
            _parent.OnSnackBarMessage(this, e);
        }
    }
}
