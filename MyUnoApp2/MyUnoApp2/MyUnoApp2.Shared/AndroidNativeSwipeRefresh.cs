#if __ANDROID__
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Uno.UI;
using Windows.UI.Xaml;
using System.Numerics;
using AndroidX.SwipeRefreshLayout.Widget;
using Android.Views;
using System.Drawing;
using AndroidX.Core.View;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace MyUnoApp2
{
    public partial class AndroidNativeSwipeRefresh : SwipeRefreshLayout
	{
		private Android.Views.View _content;

		public AndroidNativeSwipeRefresh() : base(ContextHelper.Current) { }

		public Android.Views.View Content
		{
			get { return _content; }
			set
			{
				if (_content != null)
				{
					RemoveView(_content);
				}
				_content = value;
				if (_content != null)
				{
					AddView(_content);
				}
			}
		}
	}
}

#endif