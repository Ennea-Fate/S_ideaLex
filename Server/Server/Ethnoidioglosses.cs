using System;
using System.Collections.Generic;

namespace Server
{
    public partial class Ethnoidioglosses
    {
        public Ethnoidioglosses()
        {
            BookEigs = new HashSet<BookEigs>();
            Comments = new HashSet<Comments>();
            EigStatusVotes = new HashSet<EigStatusVotes>();
        }

        public long Id { get; set; }
        public string Word { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public long AuthorId { get; set; }
        public long UserId { get; set; }
        public long BookId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Authors Author { get; set; }
        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<BookEigs> BookEigs { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<EigStatusVotes> EigStatusVotes { get; set; }
    }
}
