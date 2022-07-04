using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyUnoApp2
{
    public class TextStickyScrollingBehavior
    {
        public static object GetStickyHeader(DependencyObject obj)
        {
            return (object)obj.GetValue(StickyHeaderProperty);
        }

        public static void SetStickyHeader(DependencyObject obj, object value)
        {
            obj.SetValue(StickyHeaderProperty, value);
        }

        public static readonly DependencyProperty StickyHeaderProperty =
            DependencyProperty.RegisterAttached("StickyHeader", typeof(object), typeof(TextStickyScrollingBehavior), new PropertyMetadata(null, OnStickyHeaderChanged));



        public static double GetNonStickyHeaderOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(NonStickyHeaderOffsetProperty);
        }

        public static void SetNonStickyHeaderOffset(DependencyObject obj, double value)
        {
            obj.SetValue(NonStickyHeaderOffsetProperty, value);
        }

        public static readonly DependencyProperty NonStickyHeaderOffsetProperty =
            DependencyProperty.RegisterAttached("NonStickyHeaderOffset", typeof(double), typeof(TextStickyScrollingBehavior), new PropertyMetadata(0d, OnStickyHeaderChanged));

        private static void OnStickyHeaderChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (dObj is TextBlock nonStickyHeader
                && GetStickyHeader(dObj) is TextBlock stickyHeader)
            {
                var scrollViewer = GetScrollViewerParent(nonStickyHeader);
                if (scrollViewer != null)
                {
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
