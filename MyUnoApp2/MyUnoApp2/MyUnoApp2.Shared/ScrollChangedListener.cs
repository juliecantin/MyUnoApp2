using System;
using System.Collections.Generic;
using System.Text;
using static Android.Views.ViewTreeObserver;

namespace MyUnoApp2
{
    public class ScrollChangedListener : Java.Lang.Object, IOnScrollChangedListener
    {
        private double _targetHeight;
        private double _targetScrollOffset;

        public ScrollChangedListener(double targetHeight, double targetScrollOffset)
        {
            _targetHeight = targetHeight;
            _targetScrollOffset = targetScrollOffset;
        }

        public void OnScrollChanged()
        {
        }
    }
}
