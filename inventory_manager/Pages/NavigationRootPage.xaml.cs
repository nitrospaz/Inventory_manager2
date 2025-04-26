using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace inventory_manager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationRootPage : Page
    {
        public Action NavigationViewLoaded { get; set; }

        public NavigationRootPage()
        {
            this.InitializeComponent();
        }

        // Wraps a call to rootFrame.Navigate to give the Page a way to know which NavigationRootPage is navigating.
        // Please call this function rather than rootFrame.Navigate to navigate the rootFrame.
        public void Navigate(Type pageType, object targetPageArguments = null, NavigationTransitionInfo navigationTransitionInfo = null)
        {
            NavigationRootPageArgs args = new NavigationRootPageArgs
            {
                NavigationRootPage = this,
                Parameter = targetPageArguments
            };
            rootFrame.Navigate(pageType, args, navigationTransitionInfo);
        }

        public Microsoft.UI.Xaml.Controls.NavigationView NavigationView
        {
            get { return NavigationViewControl; }
        }

        private void OnPaneDisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                VisualStateManager.GoToState(this, "Top", true);
            }
            else
            {
                if (args.DisplayMode == NavigationViewDisplayMode.Minimal)
                {
                    VisualStateManager.GoToState(this, "Compact", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Default", true);
                }
            }
        }

        private void OnNavigationViewControlLoaded(object sender, RoutedEventArgs e)
        {
            // Delay necessary to ensure NavigationView visual state can match navigation
            Task.Delay(500).ContinueWith(_ => this.NavigationViewLoaded?.Invoke(), TaskScheduler.FromCurrentSynchronizationContext());

            var navigationView = sender as NavigationView;
            navigationView.RegisterPropertyChangedCallback(NavigationView.IsPaneOpenProperty, OnIsPaneOpenChanged);

            // Set IsPaneOpen to false to collapse the navigation menu by default
            navigationView.IsPaneOpen = false;
        }

        private void OnIsPaneOpenChanged(DependencyObject sender, DependencyProperty dp)
        {
            var navigationView = sender as NavigationView;
            var announcementText = navigationView.IsPaneOpen ? "Navigation Pane Opened" : "Navigation Pane Closed";

            var peer = FrameworkElementAutomationPeer.FromElement(navigationView);
            peer.RaiseNotificationEvent(AutomationNotificationKind.ActionCompleted,
                                        AutomationNotificationProcessing.ImportantMostRecent,
                                        announcementText,
                                        "NavigationViewPaneIsOpenChangeNotificationId");
        }

        private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                if (rootFrame.CurrentSourcePageType != typeof(SettingsPage))
                {
                    Navigate(typeof(SettingsPage));
                }
            }
            else
            {
                // the list of selected items comes from the
                // NavigationView.MenuItems list on the NavigationRootPage.xaml
                // if you add a new item to the list, you need to add a new case here
                var selectedItem = args.SelectedItemContainer;
                if (selectedItem == Home)
                {
                    if (rootFrame.CurrentSourcePageType != typeof(MainPage))
                    {
                        Navigate(typeof(MainPage));
                    }
                }
                else if (selectedItem == MySecondPage)
                {
                    if (rootFrame.CurrentSourcePageType != typeof(Page2))
                    {
                        Navigate(typeof(Page2));
                    }
                }

                //else if (selectedItem == ColorItem)
                //{
                //    Navigate(typeof(ItemPage), "Color");
                //}
                else
                {
                    //if (selectedItem.DataContext is ControlInfoDataGroup)
                    //{
                    //    var itemId = ((ControlInfoDataGroup)selectedItem.DataContext).UniqueId;
                    //    Navigate(typeof(SectionPage), itemId);
                    //}
                    //else if (selectedItem.DataContext is ControlInfoDataItem)
                    //{
                    //    var item = (ControlInfoDataItem)selectedItem.DataContext;
                    //    Navigate(typeof(ItemPage), item.UniqueId);
                    //}
                }
            }
        }


        private void OnControlsSearchBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            //{
            //    var suggestions = new List<ControlInfoDataItem>();

            //    var querySplit = sender.Text.Split(" ");
            //    foreach (var group in ControlInfoDataSource.Instance.Groups)
            //    {
            //        var matchingItems = group.Items.Where(
            //            item =>
            //            {
            //                // Idea: check for every word entered (separated by space) if it is in the name, 
            //                // e.g. for query "split button" the only result should "SplitButton" since its the only query to contain "split" and "button"
            //                // If any of the sub tokens is not in the string, we ignore the item. So the search gets more precise with more words
            //                bool flag = item.IncludedInBuild;
            //                foreach (string queryToken in querySplit)
            //                {
            //                    // Check if token is not in string
            //                    if (item.Title.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
            //                    {
            //                        // Token is not in string, so we ignore this item.
            //                        flag = false;
            //                    }
            //                }
            //                return flag;
            //            });
            //        foreach (var item in matchingItems)
            //        {
            //            suggestions.Add(item);
            //        }
            //    }
            //    if (suggestions.Count > 0)
            //    {
            //        controlsSearchBox.ItemsSource = suggestions.OrderByDescending(i => i.Title.StartsWith(sender.Text, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Title);
            //    }
            //    else
            //    {
            //        controlsSearchBox.ItemsSource = new string[] { "No results found" };
            //    }
            //}
        }

        private void OnControlsSearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //if (args.ChosenSuggestion != null && args.ChosenSuggestion is ControlInfoDataItem)
            //{
            //    var infoDataItem = args.ChosenSuggestion as ControlInfoDataItem;
            //    var hasChangedSelection = EnsureItemIsVisibleInNavigation(infoDataItem.Title);

            //    // In case the menu selection has changed, it means that it has triggered
            //    // the selection changed event, that will navigate to the page already
            //    if (!hasChangedSelection)
            //    {
            //        Navigate(typeof(ItemPage), infoDataItem.UniqueId);
            //    }
            //}
            //else if (!string.IsNullOrEmpty(args.QueryText))
            //{
            //    Navigate(typeof(SearchResultsPage), args.QueryText);
            //}
        }

        private void CtrlF_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            //controlsSearchBox.Focus(FocusState.Programmatic);
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            //TestContentLoadedCheckBox.IsChecked = true;
        }

        private void OnRootFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            //TestContentLoadedCheckBox.IsChecked = false;
        }

    }

    public class NavigationRootPageArgs
    {
        public NavigationRootPage NavigationRootPage;
        public object Parameter;
    }

    public enum DeviceType
    {
        Desktop,
        Mobile,
        Other,
        Xbox
    }
}
