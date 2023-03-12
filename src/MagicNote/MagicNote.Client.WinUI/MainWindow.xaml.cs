// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using MagicNote.Client.WinUI.Pages;
using MagicNote.Client.WinUI.Services;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
namespace MagicNote.Client.WinUI
{
	public sealed partial class MainWindow : Window
	{

		public Frame MainWindowFrame = null;
		public MainWindow()
		{
			this.InitializeComponent();
			MainWindowFrame = AppFrame;
			App.NavigationService = new NavigationService(MainWindowFrame);
			new MicaActivator(this);
			SetTitleBar(AppTitleBar);
			ExtendsContentIntoTitleBar = true;
			MainWindowFrame.Navigate(typeof(LoginPage));
		}
		
	}
}