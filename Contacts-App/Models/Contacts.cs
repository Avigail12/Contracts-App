using System;
using System.Collections.Generic;

namespace Contacts_App.Models
{
    public partial class Contacts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public byte[] CreatedAt { get; set; }
        public TimeSpan? UpdatedAt { get; set; }
    }
}
