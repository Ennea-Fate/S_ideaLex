using System;
using System.Collections.Generic;

namespace Server
{
    public partial class BookEigs
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public long EthnoidioglossId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Books Book { get; set; }
        public virtual Ethnoidioglosses Ethnoidiogloss { get; set; }
    }
}
