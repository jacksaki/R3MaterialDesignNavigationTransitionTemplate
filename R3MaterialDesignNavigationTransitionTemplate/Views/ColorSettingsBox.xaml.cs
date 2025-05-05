﻿using R3MaterialDesignNavigationTransitionTemplate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R3MaterialDesignNavigationTransitionTemplate.Views
{
    /// <summary>
    /// ColorSettingsBox.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorSettingsBox : UserControl
    {
        public ColorSettingsBox()
        {
            InitializeComponent();
            this.DataContext = App.GetService<ColorSettingsBoxViewModel>();
        }
    }
}
