using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MyUnoApp2
{
	public class NavigationViewSwipeBehavior
	{
		public static bool GetIsAttached(NavigationView obj)
		{
			return (bool)obj.GetValue(IsAttachedProperty);
		}

		public static void SetIsAttached(NavigationView obj, bool value)
		{
			obj.SetValue(IsAttachedProperty, value);
		}

		public static readonly DependencyProperty IsAttachedProperty =
			DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(NavigationViewSwipeBehavior), new PropertyMetadata(null, IsAttachedChanged));

		private static void IsAttachedChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			var navView = (NavigationView)sender;
			var isAttached = (bool)args.NewValue;

			if (isAttached)
			{
				navView.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateRailsX | ManipulationModes.TranslateInertia;
				navView.ManipulationDelta += NavView_ManipulationDelta;
			}
		}

		private static void NavView_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			var navView = (NavigationView)sender;
			var swipedDistance = e.Cumulative.Translation.X;

			//Used to see if we didn't swipe enough so that a basic tap will not open or close the pane
			if (Math.Abs(swipedDistance) <= 2)
			{
				return;
			}

			//Dependending on whether we swipe to the right or to the left, it will open or close respectively
			if (swipedDistance > 0)
			{
				navView.IsPaneOpen = true;
			}
			else
			{
				navView.IsPaneOpen = false;
			}
		}
	}
}
