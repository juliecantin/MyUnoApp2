using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyUnoApp2
{
    /// <summary>
    /// This behavior makes part of the content of scrolled page look as though it becomes a sticky header.
    /// This is accomplished by placing the content outside of, and on top of, the scrolled content, and adjust its
    /// vertical position with a TranslateTransform as the ScrollViewer changes positions.
    /// </summary>
    public class StickyScrollingBehavior
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets whether the behavior is enabled or not. If the behavior is disabled, then the control will not be
        /// affected by changes to TranslateTransform and we will not listen to the ScrollViewer's events.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(StickyScrollingBehavior), new PropertyMetadata(false, OnValueChanged));

        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        /// <summary>
        /// Gets or sets the desired margin for the sticky header. For the moment, only the top margin is taken into account by
        /// the calculations. Must be set to a non-zero value for the behavior to be enabled.
        /// </summary>
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(StickyScrollingBehavior), new PropertyMetadata(default(Thickness), OnValueChanged));

        public static FrameworkElement GetStickAfterThis(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(StickAfterThisProperty);
        }

        public static void SetStickAfterThis(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(StickAfterThisProperty, value);
        }

        /// <summary>
        /// Gets or sets the view after which the sticky header is meant to be displayed.
        /// Commonly set with a binding of type {Binding ElementName=(...)}
        /// </summary>
        public static readonly DependencyProperty StickAfterThisProperty =
            DependencyProperty.RegisterAttached("StickAfterThis", typeof(FrameworkElement), typeof(StickyScrollingBehavior), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var stickyHeader = dependencyObject as FrameworkElement;
            var stickAfterThis = GetStickAfterThis(dependencyObject);
            var margin = GetMargin(dependencyObject);

            if (stickyHeader != null
                && stickAfterThis != null
                && margin != default(Thickness)
                && GetIsEnabled(stickyHeader))
            {
                var scrollViewer = GetScrollViewerParent(stickAfterThis);
                if (scrollViewer != null)
                {
                    void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
                    {
                        UpdateStickyHeader();
                    }

                    void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
                    {
                        UpdateStickyHeader();
                    }

                    void StickAfterThis_SizeChanged(object sender, SizeChangedEventArgs e)
                    {
                        UpdateStickyHeader();
                    }

                    void StickyHeader_Layout(object sender, object e)
                    {
                        UpdateStickyHeader();
                        stickyHeader.LayoutUpdated -= StickyHeader_Layout;
                    }

                    void UpdateStickyHeader()
                    {
                        var translateTransform = stickyHeader.RenderTransform as TranslateTransform;
                        if (scrollViewer.VerticalOffset == 0)
                        {
                            var offsetFromTransform = stickAfterThis.TransformToVisual(scrollViewer).TransformPoint(new Point(0, 0)).Y;
                            var scrollViewerActualOffset = stickAfterThis.ActualHeight - offsetFromTransform - 12;
                            translateTransform.Y = 0 - scrollViewerActualOffset + stickAfterThis.ActualHeight + margin.Top;
                        }
                        else
                        {
                            translateTransform.Y = Math.Max(
                                0,
                                0 - scrollViewer.VerticalOffset + stickAfterThis.ActualHeight + margin.Top);
                        }
                    }

                    scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                    scrollViewer.ViewChanging += ScrollViewer_ViewChanging;
                    stickAfterThis.SizeChanged += StickAfterThis_SizeChanged;
                    stickyHeader.LayoutUpdated += StickyHeader_Layout;
                }
            }
        }

        private static ScrollViewer GetScrollViewerParent(DependencyObject obj)
        {
            DependencyObject parent = obj;
            while (parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is ScrollViewer scrollViewer)
                {
                    return scrollViewer;
                }
            }

            return null;
        }
    }
}
