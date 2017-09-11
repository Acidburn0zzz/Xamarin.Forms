﻿using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Diagnostics;
#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 58833, "ListView SelectedItem Binding does not fire", PlatformAffected.Android)]
	public class Bugzilla58833 : TestContentPage
	{
		[Preserve(AllMembers = true)]
		class TestCell : ViewCell
		{
			Label label;

			public TestCell()
			{
				ContextActions.Add(new MenuItem { Text = "Action" });
				label = new Label();

				label.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(() =>
					{
						Debug.WriteLine($">>>>> TestCell TestCell 30: TapGesture Fired");
					})
				});

				View = label;
			}

			protected override void OnBindingContextChanged()
			{
				base.OnBindingContextChanged();

				label.Text = (string)BindingContext;
			}
		}

		protected override void Init()
		{
			var items = new List<string>();
			for (int i = 0; i < 5; i++)
				items.Add($"Item #{i}");

			var list = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(TestCell)),
				ItemsSource = items
			};
			list.ItemSelected += List_ItemSelected;

			Content = new StackLayout
			{
				Children = {
					list
				}
			};
		}

		void List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("**** CurrentItem Changed *****");
			Debug.WriteLine($">>>>> Bugzilla58833 List_ItemSelected 60: {e.SelectedItem} Selected");
		}

#if UITEST
		[Test]
		public void Issue1Test ()
		{
			RunningApp.Screenshot ("I am at Issue 1");
			RunningApp.WaitForElement (q => q.Marked ("IssuePageLabel"));
			RunningApp.Screenshot ("I see the Label");
		}
#endif
	}
}