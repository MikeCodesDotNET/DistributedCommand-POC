using System;
using System.Collections.Generic;
using System.Text;

namespace Ongar.Messaging.Messages
{
    public class ViewModelPropertyDidChangeMessage
    {
        public string ViewModelName { get; set; }

        public string FieldName { get; set; }

        public string FieldType { get; set; }

        public object NewValue { get; set; }
    }
}
