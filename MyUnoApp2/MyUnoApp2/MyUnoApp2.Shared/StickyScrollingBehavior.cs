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
        public static int GetHeight(DependencyObject obj)
        {
            return (int)obj.GetValue(HeightProperty);
        }

        public static void SetHeight(DependencyObject obj, int value)
        {
            obj.SetValue(HeightProperty, value);
        }

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached("Height", typeof(int), typeof(StickyScrollingBehavior), new PropertyMetadata(0, OnValueChanged));

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
                if (scrollViewer != null
                    && dependencyObject is FrameworkElement fe
                    && GetHeight(dependencyObject) > 0)
                {
                    void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
                    {
                        UpdateStickyHeader();
                    }

                    void StickAfterThis_Layout(object sender, object e)
                    {
                        UpdateStickyHeader();
                        stickAfterThis.LayoutUpdated -= StickAfterThis_Layout;
                    }

                    void StickyHeader_Layout(object sender, object e)
                    {
                        UpdateStickyHeader();
                        stickyHeader.LayoutUpdated -= StickyHeader_Layout;
                    }

                    void UpdateStickyHeader()
                    {
                        var transform = stickAfterThis.TransformToVisual(scrollViewer);
                        var stickAfterThisPosition = transform.TransformPoint(new Point(0, 0));

                        var translateTransform = stickyHeader.RenderTransform as TranslateTransform;
                        translateTransform.Y = Math.Max(0, stickAfterThisPosition.Y + stickAfterThis.ActualHeight + margin.Top);
                        var marginAtBottom = Math.Min(
                            stickyHeader.ActualHeight + margin.Top + margin.Bottom,
                            stickAfterThisPosition.Y + stickAfterThis.ActualHeight + stickyHeader.ActualHeight + margin.Top + margin.Bottom
                        );
                        stickAfterThis.Margin = new Thickness(0, 0, 0, marginAtBottom);
                    }

                    scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                    stickAfterThis.LayoutUpdated += StickAfterThis_Layout;
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
