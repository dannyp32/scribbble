using System;

namespace scribbble.Models
{
    public class QuickMailTemplate
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }
    }
}
