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
    /// This is accomplished by toggling the Opacity/IsHitTestVisible of a sticky header at the top of the page,
    /// placed on top of the scrolled content, based on whether the non-sticky header has disappeared behind it due to scrolling.
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
        /// Gets or sets whether the behavior is enabled or not. If the behavior is disabled, then we will not listen to the ScrollViewer's events.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(StickyScrollingBehavior), new PropertyMetadata(false, OnValueChanged));

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

            if (stickyHeader != null
                && stickAfterThis != null
                && GetIsEnabled(stickyHeader))
            {
                var scrollViewer = GetScrollViewerParent(stickAfterThis);
                if (scrollViewer != null)
                {
                    void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
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
                        if (scrollViewer.VerticalOffset >= stickAfterThis.ActualHeight + stickAfterThis.Margin.Bottom)
                        {
                            stickyHeader.Opacity = 1;
                            stickyHeader.IsHitTestVisible = true;
                        }
                        else
                        {
                            stickyHeader.Opacity = 0;
                            stickyHeader.IsHitTestVisible = false;
                        }
                    }

                    scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
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
