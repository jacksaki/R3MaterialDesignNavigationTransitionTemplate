using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels.Transitions
{
    public class Page3BoxViewModel : ViewModelBase
    {
        public TransitionBoxViewModel Parent { get; }
        public ReactiveCommand PreviousCommand { get; }
        public ReactiveCommand ExecuteCommand { get; }
        public BindableReactiveProperty<bool> CanExecute { get; }
        //public ReactiveCommand NextCommand { get; }
        public Page3BoxViewModel(TransitionBoxViewModel parent) : base()
        {
            this.Parent = parent;
            this.PreviousCommand = new ReactiveCommand();
            this.PreviousCommand.Subscribe(_ =>
            {
                this.Parent.CurrentPageIndex.Value--;
            });
            this.CanExecute = new BindableReactiveProperty<bool>();
            this.ExecuteCommand = this.CanExecute.ToReactiveCommand();
            this.ExecuteCommand.SubscribeAwait(async (x, ct) =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    await System.Threading.Tasks.Task.Delay(10);
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }, AwaitOperation.Drop);
        }
    }
}
