using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace MyUnoApp2
{
    [ContentProperty(Name = "Content")]
    public partial class ScrollViewerWithStickyHeader : Control
    {
        private const string PART_ScrollViewer = "PART_ScrollViewer";

        private ScrollViewer _scrollViewer;
        private TextBlock _stickyHeader;
        private TextBlock _nonStickyHeader;
        private FrameworkElement _beforeStickyHeader;
        private double _nonStickyHeaderInitialFontSize;
        private double? _nonStickyHeaderLeftPadding;

        public ScrollViewerWithStickyHeader()
        {
        }

        #region Properties
        public static object GetBeforeNonStickyHeaderFor(DependencyObject obj)
        {
            return (object)obj.GetValue(BeforeNonStickyHeaderForProperty);
        }

        public static void SetBeforeNonStickyHeaderFor(DependencyObject obj, object value)
        {
            obj.SetValue(BeforeNonStickyHeaderForProperty, value);
        }

        public static readonly DependencyProperty BeforeNonStickyHeaderForProperty =
            DependencyProperty.RegisterAttached("BeforeNonStickyHeaderFor", typeof(object), typeof(ScrollViewerWithStickyHeader), new PropertyMetadata(null, BeforeNonStickyHeaderForChanged));

        private static void BeforeNonStickyHeaderForChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is ScrollViewerWithStickyHeader scrollViewer
                && dObj is FrameworkElement beforeStickyHeader)
            {
                scrollViewer._beforeStickyHeader = beforeStickyHeader;
            }
        }

        public static object GetStickyHeaderFor(DependencyObject obj)
        {
            return (object)obj.GetValue(StickyHeaderForProperty);
        }

        public static void SetStickyHeaderFor(DependencyObject obj, object value)
        {
            obj.SetValue(StickyHeaderForProperty, value);
        }

        public static readonly DependencyProperty StickyHeaderForProperty =
            DependencyProperty.RegisterAttached("StickyHeaderFor", typeof(object), typeof(ScrollViewerWithStickyHeader), new PropertyMetadata(null, StickyHeaderForChanged));

        private static void StickyHeaderForChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is ScrollViewerWithStickyHeader scrollViewer
                && dObj is TextBlock stickyHeader)
            {
                stickyHeader.Opacity = 0;
                stickyHeader.IsHitTestVisible = false;
                scrollViewer._stickyHeader = stickyHeader;
            }
        }

        public static object GetNonStickyHeaderFor(DependencyObject obj)
        {
            return (object)obj.GetValue(NonStickyHeaderForProperty);
        }

        public static void SetNonStickyHeaderFor(DependencyObject obj, object value)
        {
            obj.SetValue(NonStickyHeaderForProperty, value);
        }

        public static readonly DependencyProperty NonStickyHeaderForProperty =
            DependencyProperty.RegisterAttached("NonStickyHeaderFor", typeof(object), typeof(ScrollViewerWithStickyHeader), new PropertyMetadata(null, NonStickyHeaderForChanged));

        private static void NonStickyHeaderForChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is ScrollViewerWithStickyHeader scrollViewer
                && dObj is TextBlock nonStickyHeader)
            {
                scrollViewer._nonStickyHeader = nonStickyHeader;
                scrollViewer._nonStickyHeaderInitialFontSize = nonStickyHeader.FontSize;
                scrollViewer._nonStickyHeaderLeftPadding = null;
            }
        }

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ScrollViewerWithStickyHeader), new PropertyMetadata(null));

        public object StickyHeaderContainer
        {
            get { return (object)GetValue(StickyHeaderContainerProperty); }
            set { SetValue(StickyHeaderContainerProperty, value); }
        }

        public static readonly DependencyProperty StickyHeaderContainerProperty =
            DependencyProperty.Register("StickyHeaderContainer", typeof(object), typeof(ScrollViewerWithStickyHeader), new PropertyMetadata(null));
        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _scrollViewer = (ScrollViewer)GetTemplateChild(PART_ScrollViewer);
            _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (_stickyHeader != null
                && _nonStickyHeader != null)
            {
                CalculateOffsets();

                var proportionProgress = 
                    _scrollViewer.VerticalOffset / 
                    (_beforeStickyHeader.ActualHeight + _beforeStickyHeader.Margin.Top + _beforeStickyHeader.Margin.Bottom 
                    + _stickyHeader.ActualHeight + _stickyHeader.Margin.Top);
                if (proportionProgress >= 1)
                {
                    _stickyHeader.Opacity = 1;
                    _stickyHeader.IsHitTestVisible = true;
                    _nonStickyHeader.Opacity = 0;
                    _nonStickyHeader.IsHitTestVisible = false;
                }
                else
                {
                    _stickyHeader.Opacity = 0;
                    _stickyHeader.IsHitTestVisible = false;
                    _nonStickyHeader.Opacity = 1;
                    _nonStickyHeader.IsHitTestVisible = true;

                    _nonStickyHeader.FontSize = (_stickyHeader.FontSize - _nonStickyHeaderInitialFontSize) * proportionProgress + _nonStickyHeaderInitialFontSize;

                    var leftMargin = GetLeftMargin(_scrollViewer.ActualWidth, _stickyHeader.ActualWidth, _nonStickyHeaderLeftPadding.Value, proportionProgress);
                    _nonStickyHeader.Margin = new Thickness(leftMargin, 0, 0, 0);
                }
            }
        }

        private void CalculateOffsets()
        {
            if (_nonStickyHeaderLeftPadding == null)
            {
                var distance = _nonStickyHeader.TransformToVisual(_scrollViewer).TransformPoint(new Windows.Foundation.Point(0, 0));
                _nonStickyHeaderLeftPadding = distance.X;
            }
        }

        private double GetLeftMargin(double scrollableWidth, double centeredElementWidth, double leftPadding, double proportionProgress)
        {
            return ((scrollableWidth - centeredElementWidth) / 2 - leftPadding) * proportionProgress;
        }

        private static ScrollViewerWithStickyHeader GetScrollViewerParent(TextBlock obj)
        {
            DependencyObject parent = obj;
            while (parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is ScrollViewerWithStickyHeader scrollViewer)
                {
                    return scrollViewer;
                }
            }

            return null;
        }
    }
}
