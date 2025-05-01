using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels.Transitions
{
    public class Page2BoxViewModel : ViewModelBase
    {
        public TransitionBoxViewModel Parent { get; }
        public ReactiveCommand PreviousCommand { get; }
        public ReactiveCommand NextCommand { get; }
        public Page2BoxViewModel(TransitionBoxViewModel parent) : base()
        {
            this.Parent = parent;
            this.PreviousCommand = new ReactiveCommand();
            this.PreviousCommand.Subscribe(_ =>
            {
                this.Parent.CurrentPageIndex.Value--;
            });
            this.NextCommand = new ReactiveCommand();
            this.NextCommand.Subscribe(_ =>
            {
                this.Parent.CurrentPageIndex.Value++;
            });
        }

    }
}
