#if __ANDROID__
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using static Android.Views.View;

namespace MyUnoApp2
{
    public class LayoutChangedListener : Java.Lang.Object, IOnLayoutChangeListener
    {
        private double _targetHeight;
        private double _targetScrollOffset;

        public LayoutChangedListener(double targetHeight, double targetScrollOffset)
            : base()
        {
            _targetHeight = targetHeight;
            _targetScrollOffset = targetScrollOffset;
        }

        public void OnLayoutChange(View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
        }
    }
}
#endif