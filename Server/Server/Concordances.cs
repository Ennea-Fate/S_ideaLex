using System;
using System.Collections.Generic;

namespace Server
{
    public partial class Concordances
    {
        public Concordances()
        {
            ConcordanceBooks = new HashSet<ConcordanceBooks>();
        }

        public long Id { get; set; }
        public string Concordance { get; set; }
        public bool? IsItLemma { get; set; }
        public int? Width { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<ConcordanceBooks> ConcordanceBooks { get; set; }
    }
}
