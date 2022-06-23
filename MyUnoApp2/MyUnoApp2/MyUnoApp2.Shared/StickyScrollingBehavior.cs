using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyUnoApp2
{
    public class StickyScrollingBehavior
    {
        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

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

        public static readonly DependencyProperty StickAfterThisProperty =
            DependencyProperty.RegisterAttached("StickAfterThis", typeof(FrameworkElement), typeof(StickyScrollingBehavior), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var stickyHeader = dependencyObject as FrameworkElement;
            var stickAfterThis = GetStickAfterThis(dependencyObject);
            var margin = GetMargin(dependencyObject);
            if (stickyHeader != null && stickAfterThis != null && margin != default(Thickness))
            {
                var scrollViewer = GetScrollViewerParent(stickAfterThis);
                if (scrollViewer != null)
                {
                    void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
                    {
                        UpdateStickyHeader();
                    }

                    void StickAfterThis_SizeChanged(object sender, SizeChangedEventArgs args)
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
                        translateTransform.Y = Math.Max(0, 0 - scrollViewer.VerticalOffset + stickAfterThis.ActualHeight + margin.Top);
                    }

                    scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                    stickAfterThis.SizeChanged += StickAfterThis_SizeChanged; ;
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
