using System.Windows.Controls;
using R3MaterialDesignNavigationTransitionTemplate.ViewModels;

namespace R3MaterialDesignNavigationTransitionTemplate.Views
{
    /// <summary>
    /// SampleBox.xaml の相互作用ロジック
    /// </summary>
    public partial class SampleBox : UserControl
    {
        public SampleBox()
        {
            InitializeComponent();
            this.DataContext = App.GetService<SampleBoxViewModel>();
        }
    }
}
