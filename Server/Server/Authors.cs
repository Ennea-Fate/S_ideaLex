using System;
using System.Collections.Generic;

namespace Server
{
    public partial class Authors
    {
        public Authors()
        {
            Books = new HashSet<Books>();
            Ethnoidioglosses = new HashSet<Ethnoidioglosses>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Books> Books { get; set; }
        public virtual ICollection<Ethnoidioglosses> Ethnoidioglosses { get; set; }
    }
}
