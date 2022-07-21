using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyUnoApp2
{
	public class NavigationViewItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate OverviewTemplate { get; set; }

		public DataTemplate MessagingTemplate { get; set; }

		public DataTemplate StatementsAndDocumentsTemplate { get; set; }

		public DataTemplate FinancialPlanTemplate { get; set; }

		public DataTemplate NetWorthTemplate { get; set; }

		public DataTemplate GoalsTemplate { get; set; }

		public DataTemplate ResourcesTemplate { get; set; }

		public DataTemplate ChatTemplate { get; set; }

		public DataTemplate SeparatorTemplate { get; set; }

		public DataTemplate MobileWebsiteTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			return SelectTemplateCore(item);
		}

		[SuppressMessage("High cyclomatic complexity", "NV2005", Justification = "Nothing complex here")]
		protected override DataTemplate SelectTemplateCore(object item)
		{
			switch (item?.ToString())
			{
				case "1":
					return OverviewTemplate;
				case "2":
					return MessagingTemplate;
				case "3":
					return StatementsAndDocumentsTemplate;
				case "4":
					return FinancialPlanTemplate;
				case "5":
					return NetWorthTemplate;
				case "6":
					return GoalsTemplate;
				case "7":
					return ResourcesTemplate;
				case "8":
					return ChatTemplate;
				case "9":
					return SeparatorTemplate;
				case "0":
					return MobileWebsiteTemplate;
				default:
					return null;
			}
		}
	}
}
