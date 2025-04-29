using inventory_manager.Helpers;
using inventory_manager.Pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Graphics;
using Windows.Graphics.Display;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace inventory_manager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        // define stuff for getting the scale factor

        private const int MDT_EFFECTIVE_DPI = 0;

        [DllImport("Shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

        [DllImport("User32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        private static double GetScaleFactorForMonitor(IntPtr hwnd)
        {
            IntPtr hmonitor = MonitorFromWindow(hwnd, 0);
            uint dpiX, dpiY;
            int result = GetDpiForMonitor(hmonitor, MDT_EFFECTIVE_DPI, out dpiX, out dpiY);
            if (result != 0)
            {
                throw new Exception("Failed to get DPI for monitor.");
            }
            return dpiX / 96.0; // 96 DPI is the default DPI for 100% scaling
        }

        // END define stuff for getting the scale factor


        private static Window? startupWindow;
        private static AppWindow? appWindow;
        static private List<Window> _activeWindows = new List<Window>();

        public static AppWindow? AppWindow => appWindow;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            //Debugger.Launch();
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            try
            {
                // Load the startup window.
                startupWindow = new Window
                {
                    SystemBackdrop = new MicaBackdrop()
                };

                // Handle the window being closed.
                startupWindow.Closed += (sender, args) => {
                    _activeWindows.Remove(startupWindow);
                };

                // Add the window to the list of active windows.
                _activeWindows.Add(startupWindow);

                // merge title bar with app content option
                // makes it look more Windows 11 like
                startupWindow.ExtendsContentIntoTitleBar = true;

                // set starting window size
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(startupWindow);
                var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                appWindow = AppWindow.GetFromWindowId(windowId);

                // Check if the monitor is scaled
                double scaleFactor = 1.0;
                try
                {
                    scaleFactor = GetScaleFactorForMonitor(hwnd);
                    Debug.WriteLine($"Scale Factor: {scaleFactor}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to get scale factor: {ex.Message}");
                }

                // Calculate the window size based on the scale factor
                int baseWidth = 1240;
                int baseHeight = 768;
                int adjustedWidth = (int)(baseWidth * scaleFactor);
                int adjustedHeight = (int)(baseHeight * scaleFactor);

                
                // set the window size
                appWindow.Resize(new SizeInt32(adjustedWidth, adjustedHeight));

                // 2560 x 1440 monitor resolution
                // 100% scaling
                // 1240 x 768 window size

                // 2560 x 1600 monitor resolution
                // 150% scaling
                // 1840 x 700 window size

                // Load the startup window content.
                // The content is a NavigationRootPage that contains a Frame that will host the app's pages.
                // The NavigationRootPage is a custom page that contains a NavigationView(menu) and a Frame.
                // The NavigationView(menu) is used to navigate between the app's pages.
                // The Frame is used to host the app's pages.
                // The NavigationRootPage is defined in the Pages/NavigationRootPage.xaml file.
                // window (host for everything) -> NavigationRootPage (host for menu and frame(page)) -> Navigation menu and Frame -> MainPage (loaded into the Frame)
                Frame rootFrame;
                if (startupWindow.Content is NavigationRootPage rootPage)
                {
                    rootFrame = (Frame)rootPage.FindName("rootFrame");
                }
                else
                {
                    rootPage = new NavigationRootPage();
                    rootFrame = (Frame)rootPage.FindName("rootFrame");
                    if (rootFrame == null)
                    {
                        throw new Exception("Root frame not found");
                    }
                    SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                    rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                    rootFrame.NavigationFailed += OnNavigationFailed;

                    startupWindow.Content = rootPage;
                }

                // Set the page and arguments to load when the app starts.
                var targetPageType = typeof(InventoryPage);
                var targetPageArguments = string.Empty;
                
                // Load the main page into the frame.
                rootPage.Navigate(targetPageType, targetPageArguments);

                // if navigating to the main page
                if (targetPageType == typeof(InventoryPage))
                {
                    // highlight the first item in the navigation menu
                    var navItem = (NavigationViewItem)rootPage.NavigationView.MenuItems[0];
                    navItem.IsSelected = true;
                }

                // Send it.
                // Activate the startup window.
                startupWindow.Activate();
            }
            catch (Exception ex)
            {
                LogException(ex);
                Environment.Exit(1);
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void LogException(Exception ex)
        {
            Debug.WriteLine($"Log Error: {ex.Message}");

            string logFileDir = Path.Combine(Directory.GetCurrentDirectory(), "StartupLogs");
            
            if (!Directory.Exists(logFileDir))
            {
                Directory.CreateDirectory(logFileDir);
            }

            string logFilePath = Path.Combine(logFileDir, "error.log");

            File.AppendAllText(logFilePath, $"{DateTime.Now}: {ex.ToString()}{Environment.NewLine}");
        }
    }
}
