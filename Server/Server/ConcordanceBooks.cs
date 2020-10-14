using System;
using System.Collections.Generic;

namespace Server
{
    public partial class ConcordanceBooks
    {
        public long Id { get; set; }
        public long ConcordanceId { get; set; }
        public long BookId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Books Book { get; set; }
        public virtual Concordances Concordance { get; set; }
    }
}
