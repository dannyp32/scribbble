using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Controls;
using scribbble.Models;

namespace Scribbble
{
    public partial class NewNote : PhoneApplicationPage
    {
        private readonly TemplateStorage storage = new TemplateStorage();

        public NewNote()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private Guid? TemplateId
        {
            get
            {
                if (!NavigationContext.QueryString.ContainsKey("Id"))
                {
                    return null;
                }

                return new Guid(NavigationContext.QueryString["Id"]);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (TemplateId == null)
                return;

            var template = storage.GetItems()
                .Single(t => t.Id == TemplateId);

            Subject.Text = template.Subject;
            Body.Text = template.Body;
        }

        private void OnSave(object sender, EventArgs e)
        {
            QuickMailTemplate template;

            if (TemplateId == null)
            {

                template = new QuickMailTemplate
                {
                    Id = Guid.NewGuid(),
                    Subject = Subject.Text,
                    Body = Body.Text
                };

                storage.Save(template);
            }
            else
            {
                template = storage.GetItems()
                    .Single(t => t.Id == TemplateId);

                template.Subject = Subject.Text;
                template.Body = Body.Text;
            }

            storage.SaveChanges();

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Body_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Body.Text.Equals("Simply tap here and begin writing..."))
            {
                Body.Text = "";
            }
        }

        private void Subject_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Subject.Text.Equals("new note"))
            {
                Subject.Text = "";
            }
        }

        private void Body_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Body.Text.Equals(""))
            {
                Body.Text = "Simply tap here and begin writing...";
            }
        }

        private void Subject_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Subject.Text.Equals(""))
            {
                Subject.Text = "new note";
            }
        }

        private void OnReview(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

    }
}