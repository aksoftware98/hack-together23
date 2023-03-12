// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Graph.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MagicNote.Client.WinUI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PlanningPage : Page
	{
		public PlanningPage()
		{
			this.InitializeComponent();
			lstView.ItemsSource = Items;
		}

		public SampleItem[] Items = new[]
		{
			new SampleItem(),
			new SampleItem(),
			new SampleItem(),
			new SampleItem(),
		};
	}

	public class SampleItem
	{
		public string[] Contacts { get; set; } = new string[3];
	}

}
