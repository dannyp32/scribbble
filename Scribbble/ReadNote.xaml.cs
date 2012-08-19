using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using scribbble.Models;

namespace Scribbble
{
    public partial class ReadNote : PhoneApplicationPage
    {
        private readonly TemplateStorage storage = new TemplateStorage();

        public ReadNote()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private Guid? TemplateId
        {
            get
            {
                if (!NavigationContext.QueryString.ContainsKey("Id"))
                    return null;

                return new Guid(NavigationContext.QueryString["Id"]);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var template = storage.GetItems()
                .Single(t => t.Id == TemplateId);

            Subject.Text = template.Subject;
            Body.Text = template.Body;
        }

        private void OnSend(object sender, EventArgs e)
        {
            var template = storage.GetItems()
                .Single(t => t.Id == TemplateId);

            var composeTask = new EmailComposeTask
            {
                Subject = template.Subject,
                Body = template.Body
            };

            composeTask.Show();
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var template = storage.GetItems()
                .Single(t => t.Id == TemplateId);

            NavigationService.Navigate(new Uri("/NewNote.xaml?Id=" + template.Id, UriKind.Relative));
        }

        private void OnDelete(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you would like to delete this message?");
            var template = storage.GetItems()
                .Single(t => t.Id == TemplateId);

            storage.Delete(template);
            storage.SaveChanges();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void textDoubleTap(object sender, GestureEventArgs e)
        {
            var template = storage.GetItems().Single(t => t.Id == TemplateId);

            NavigationService.Navigate(new Uri("/NewNote.xaml?Id=" + template.Id, UriKind.Relative));
        }

        private void OnReview(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}