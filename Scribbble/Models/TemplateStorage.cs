using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

namespace scribbble.Models
{
    public class TemplateStorage
    {
        private IList<QuickMailTemplate> templates;
        private const string Filename = "template-list.xml";

        protected IList<QuickMailTemplate> Templates
        {
            get
            {
                return templates ?? (templates = LoadTemplates().ToList());
            }
            set
            {
                templates = value;
            }
        }

        protected IEnumerable<QuickMailTemplate> LoadTemplates()
        {
            using (var applicationStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!applicationStorage.FileExists(Filename))
                    return Enumerable.Empty<QuickMailTemplate>();

                using (var speedListFile = applicationStorage.OpenFile(Filename, FileMode.Open, FileAccess.Read))
                {
                    var document = XDocument.Load(speedListFile);

                    return from t in document.Root.Elements("template")
                           select new QuickMailTemplate
                           {
                               Id = new Guid(t.Attribute("id").Value),
                               Subject = t.Attribute("subject").Value,
                               Body = t.Attribute("body").Value
                           };
                }
            }
        }

        public IEnumerable<QuickMailTemplate> GetItems()
        {
            return Templates;
        }

        public void Save(QuickMailTemplate template)
        {
            Templates.Insert(0, template);
        }

        public void Delete(QuickMailTemplate template)
        {
            Templates.Remove(template);
        }

        public void SaveChanges()
        {
            using (var applicationStorage = IsolatedStorageFile.GetUserStoreForApplication())
            using (var speedListFile = applicationStorage.OpenFile(Filename, FileMode.Create, FileAccess.Write))
            {
                var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("templates",
                        from t in Templates
                        select new XElement("template",
                            new XAttribute("id", t.Id),
                            new XAttribute("subject", t.Subject),
                            new XAttribute("body", t.Body))));

                document.Save(speedListFile);
            }
        }

    }
}
