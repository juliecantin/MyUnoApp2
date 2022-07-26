using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyUnoApp2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            navView.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateRailsX | ManipulationModes.TranslateInertia;
            navView.ManipulationDelta += NavView_ManipulationDelta;
            navView.ManipulationDelta += NavView_ManipulationDelta2;
            navView.ManipulationCompleted += NavView_ManipulationCompleted;
        }

        private void NavView_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            TxtCompletedCumulativeX.Text = e.Cumulative.Translation.X.ToString();
        }

        private void NavView_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            TxtCumulativeX.Text = e.Cumulative.Translation.X.ToString();
        }

        private void NavView_ManipulationDelta2(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            TxtDeltaX.Text = e.Delta.Translation.X.ToString();
        }
    }
}
