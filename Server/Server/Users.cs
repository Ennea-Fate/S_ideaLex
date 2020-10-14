using System;
using System.Collections.Generic;

namespace Server
{
    public partial class Users
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            EigStatusVotes = new HashSet<EigStatusVotes>();
            Ethnoidioglosses = new HashSet<Ethnoidioglosses>();
            Notebooks = new HashSet<Notebooks>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public long RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Roles Role { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<EigStatusVotes> EigStatusVotes { get; set; }
        public virtual ICollection<Ethnoidioglosses> Ethnoidioglosses { get; set; }
        public virtual ICollection<Notebooks> Notebooks { get; set; }
    }
}
