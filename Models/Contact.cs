using System;
using System.Runtime.Serialization;

namespace ContactBook.Models
{
    public class Contact
    {
        [DataMember]
        public Guid id { get; set; }

        [DataMember]
        public string firstName { get; set; }

        [DataMember]
        public string lastName { get; set; }

        [DataMember]
        public string number { get; set; }

        public string searchExpression
        {
            get
            {
                return firstName + "|" + lastName + "|" + number;
            }
        }
    }
}
