using R3;
using R3MaterialDesignNavigationTransitionTemplate.ViewModels.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels
{
    public class TransitionBoxViewModel : BoxViewModelBase
    {
        public BindableReactiveProperty<int> CurrentPageIndex { get; }

        private readonly Lazy<Page1BoxViewModel> _page1ViewModel;
        public Page1BoxViewModel Page1BoxViewModel => _page1ViewModel.Value;

        private readonly Lazy<Page2BoxViewModel> _page2ViewModel;
        public Page2BoxViewModel Page2BoxViewModel => _page2ViewModel.Value;

        private readonly Lazy<Page3BoxViewModel> _page3ViewModel;
        public Page3BoxViewModel Page3BoxViewModel => _page3ViewModel.Value;

        public TransitionBoxViewModel(
            Lazy<Page1BoxViewModel> p1,
            Lazy<Page2BoxViewModel> p2,
            Lazy<Page3BoxViewModel> p3) : base()
        {
            this.CurrentPageIndex = new BindableReactiveProperty<int>(0);
            _page1ViewModel = p1;
            _page2ViewModel = p2;
            _page3ViewModel = p3;
        }
    }
}
