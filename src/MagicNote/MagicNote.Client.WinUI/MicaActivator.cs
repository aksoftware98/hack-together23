// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using WinRT; // required to support Window.As<ICompositionSupportsSystemBackdrop>()
using Microsoft.UI.Composition.SystemBackdrops;
namespace MagicNote.Client.WinUI
{

	public class MicaActivator
	{

		WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See below for implementation.
		MicaController m_backdropController;
		SystemBackdropConfiguration m_configurationSource;
		Window window;

		public MicaActivator(Window window)
		{
			this.window = window;
			TrySetSystemBackdrop();
			window.ExtendsContentIntoTitleBar = true; 
		}

		private void Window_Activated(object sender, WindowActivatedEventArgs args)
		{
			m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
		}

		private void Window_Closed(object sender, WindowEventArgs args)
		{
			// Make sure any Mica/Acrylic controller is disposed
			// so it doesn't try to use this closed window.
			if (m_backdropController != null)
			{
				m_backdropController.Dispose();
				m_backdropController = null;
			}
			window.Activated -= Window_Activated;
			m_configurationSource = null;
		}

		private void Window_ThemeChanged(FrameworkElement sender, object args)
		{
			if (m_configurationSource != null)
			{
				SetConfigurationSourceTheme();
			}
		}

		private void SetConfigurationSourceTheme()
		{
			switch (((FrameworkElement)window.Content).ActualTheme)
			{
				case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
				case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
				case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
			}
		}


		bool TrySetSystemBackdrop()
		{
			if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
			{
				m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
				m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

				// Create the policy object.
				m_configurationSource = new SystemBackdropConfiguration();
				window.Activated += Window_Activated;
				window.Closed += Window_Closed;
				((FrameworkElement)window.Content).ActualThemeChanged += Window_ThemeChanged;

				// Initial configuration state.
				m_configurationSource.IsInputActive = true;
				SetConfigurationSourceTheme();

				m_backdropController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();
				m_backdropController.TintColor = Windows.UI.Color.FromArgb(88, 88, 88, 255);
				//m_backdropController.FallbackColor = Windows.UI.Color.FromArgb(53, 30, 40, 50);
				//m_backdropController.TintOpacity = 0.3f;
				//m_backdropController.LuminosityOpacity = 0.5f;
				// Enable the system backdrop.
				// Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
				m_backdropController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
				m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
				return true; // succeeded
			}

			return false; // Mica is not supported on this system
		}
	}
}