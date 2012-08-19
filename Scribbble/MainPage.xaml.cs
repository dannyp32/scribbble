using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using scribbble.Models;

namespace Scribbble
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly TemplateStorage storage = new TemplateStorage();
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += onLoaded;
            // Set the data context of the listbox control to the sample data
            //DataContext = App.ViewModel;
            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }



        // Load data for the ViewModel Items
        private void onLoaded(object sender, RoutedEventArgs e)
        {
            Templates.ItemsSource = storage.GetItems();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Templates.SelectedItem == null)
            {
                return;
            }

            var template = (QuickMailTemplate)Templates.SelectedItem;

            NavigationService.Navigate(new Uri("/ReadNote.xaml?Id=" + template.Id, UriKind.Relative));
            Templates.SelectedItem = null;

        }
        private void newNoteSelected(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewNote.xaml", UriKind.Relative));
        }


        /*
        private void tutorialSelected(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Tutorial.xaml", UriKind.Relative));
        }
        */

    }
}