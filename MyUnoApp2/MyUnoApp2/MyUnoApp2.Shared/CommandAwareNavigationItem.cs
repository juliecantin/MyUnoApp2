using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyUnoApp2
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("nventive.Input", "NV0117:NV0117 - RegisterAttribute is required on NSObject derived classes", Justification = "Not really required")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("nventive.Reliability", "NV0126:NV0126 - Classes inheriting from UIKit.UIView or Android.Views.View must implement a native contructor", Justification = "Not really required")]
    public partial class CommandAwareNavigationViewItem : NavigationViewItem
    {
        public ICommand NavigationCommand
        {
            get { return (ICommand)GetValue(NavigationCommandProperty); }
            set { SetValue(NavigationCommandProperty, value); }
        }

        public static readonly DependencyProperty NavigationCommandProperty =
            DependencyProperty.Register(nameof(NavigationCommand), typeof(ICommand), typeof(CommandAwareNavigationViewItem), new PropertyMetadata(default(ICommand)));
    }
}
