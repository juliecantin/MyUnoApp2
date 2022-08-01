using System;
using System.Collections.Generic;
using System.Text;
using Uno.UI.Toolkit;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyUnoApp2
{
    public static class DynamicHeaderBehavior
    {
        public static object GetScrollViewer(DependencyObject obj)
        {
            return obj.GetValue(ScrollViewerProperty);
        }

        public static void SetScrollViewer(DependencyObject obj, object value)
        {
            obj.SetValue(ScrollViewerProperty, value);
        }

        public static readonly DependencyProperty ScrollViewerProperty =
            DependencyProperty.RegisterAttached("ScrollViewer", typeof(object), typeof(DynamicHeaderBehavior), new PropertyMetadata(null, OnPropertyChanged));

        public static double GetStartingElevation(DependencyObject obj)
        {
            return (double)obj.GetValue(StartingElevationProperty);
        }

        public static void SetStartingElevation(DependencyObject obj, double value)
        {
            obj.SetValue(StartingElevationProperty, value);
        }

        // Using a DependencyProperty as the backing store for StartingElevation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartingElevationProperty =
            DependencyProperty.RegisterAttached("StartingElevation", typeof(double), typeof(DynamicHeaderBehavior), new PropertyMetadata(-1d, OnPropertyChanged));

        public static double GetTargetElevation(DependencyObject obj)
        {
            return (double)obj.GetValue(TargetElevationProperty);
        }

        public static void SetTargetElevation(DependencyObject obj, double value)
        {
            obj.SetValue(TargetElevationProperty, value);
        }

        public static readonly DependencyProperty TargetElevationProperty =
            DependencyProperty.RegisterAttached("TargetElevation", typeof(double), typeof(DynamicHeaderBehavior), new PropertyMetadata(-1d, OnPropertyChanged));

        public static double GetStartingHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(StartingHeightProperty);
        }

        public static void SetStartingHeight(DependencyObject obj, double value)
        {
            obj.SetValue(StartingHeightProperty, value);
        }

        public static readonly DependencyProperty StartingHeightProperty =
            DependencyProperty.RegisterAttached("StartingHeight", typeof(double), typeof(DynamicHeaderBehavior), new PropertyMetadata(-1d, OnPropertyChanged));

        public static double GetTargetHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(TargetHeightProperty);
        }

        public static void SetTargetHeight(DependencyObject obj, double value)
        {
            obj.SetValue(TargetHeightProperty, value);
        }

        public static readonly DependencyProperty TargetHeightProperty =
            DependencyProperty.RegisterAttached("TargetHeight", typeof(double), typeof(DynamicHeaderBehavior), new PropertyMetadata(-1d, OnPropertyChanged));

        public static double GetTargetScrollOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(TargetScrollOffsetProperty);
        }

        public static void SetTargetScrollOffset(DependencyObject obj, double value)
        {
            obj.SetValue(TargetScrollOffsetProperty, value);
        }

        public static readonly DependencyProperty TargetScrollOffsetProperty =
            DependencyProperty.RegisterAttached("TargetScrollOffset", typeof(double), typeof(DynamicHeaderBehavior), new PropertyMetadata(0d, OnPropertyChanged));

        public static Color GetShadowColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(ShadowColorProperty);
        }

        public static void SetShadowColor(DependencyObject obj, Color value)
        {
            obj.SetValue(ShadowColorProperty, value);
        }

        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.RegisterAttached("ShadowColor", typeof(Color), typeof(DynamicHeaderBehavior), new PropertyMetadata(null, OnPropertyChanged));

        public static Color GetBackground(DependencyObject obj)
        {
            return (Color)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, Color value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Color), typeof(DynamicHeaderBehavior), new PropertyMetadata(null, OnPropertyChanged));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Cyclomatic complexity", "NV2005", Justification = "Nothing complex here")]
        private static void OnPropertyChanged(DependencyObject dObject, DependencyPropertyChangedEventArgs args)
        {
            if (dObject is FrameworkElement elevatedView)
            {
                var scrollViewer = GetScrollViewer(elevatedView) as ScrollViewer;

                void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
                {
                    var startingElevation = GetStartingElevation(elevatedView);
                    var startingHeight = GetStartingHeight(elevatedView);
                    var targetElevation = GetTargetElevation(elevatedView);
                    var targetHeight = GetTargetHeight(elevatedView);
                    var targetScrollOffset = GetTargetScrollOffset(elevatedView);
                    var shadowColor = GetShadowColor(elevatedView);
                    var backgroundColor = GetBackground(elevatedView);

                    if (startingElevation != -1d
                        && startingHeight != -1d
                        && targetElevation != -1d
                        && targetHeight != -1d
                        && targetScrollOffset != 0d
                        && shadowColor != null
                        && backgroundColor != null)
                    {
                        // TODO add ease-out/ease-in
                        // Do not take negative values (from overscrolling) into account 
                        var scrollProportion = Math.Max(
                            0,
                            // Stop after we have reached the target offset
                            Math.Min(
                                1,
                                scrollViewer.VerticalOffset / targetScrollOffset)
                        );

                        // TODO causes stuttering?
                        elevatedView.Height = startingHeight + ((targetHeight - startingHeight) * scrollProportion);

#if __IOS__
			            var view = (UIKit.UIView)d;
			            view.Layer.ShadowRadius = 5.0f;
			            view.Layer.ShadowOpacity = 0.4f;
			            view.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 7);
			            view.Layer.MasksToBounds = false;
#elif __ANDROID__
			            var view = (Android.Views.View)elevatedView;
			            AndroidX.Core.View.ViewCompat.SetElevation(view, (float)Uno.UI.ViewHelper.LogicalToPhysicalPixels(8 * scrollProportion));
                        if (scrollProportion == 0)
                        {
                            AndroidX.Core.View.ViewCompat.SetElevation(view, 0);
                            AndroidX.Core.View.ViewCompat.SetTranslationZ(view, 0);
                        }
#endif
                    }
                }

                if (scrollViewer != null)
                {
                    scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                }
            }
        }
    }
}
