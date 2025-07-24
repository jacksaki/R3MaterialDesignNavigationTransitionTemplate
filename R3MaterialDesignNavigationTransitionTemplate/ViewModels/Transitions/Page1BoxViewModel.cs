using R3;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels.Transitions
{
    public class Page1BoxViewModel : ViewModelBase
    {
        public TransitionBoxViewModel Parent { get; }
        public ReactiveCommand NextCommand { get; }

        public Page1BoxViewModel(TransitionBoxViewModel parent) : base()
        {
            this.Parent = parent;
            this.NextCommand = new ReactiveCommand();
            this.NextCommand.Subscribe(_ =>
            {
                this.Parent.CurrentPageIndex.Value++;
            });
        }
    }
}
