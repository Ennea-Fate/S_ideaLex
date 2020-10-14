using System;
using System.Collections.Generic;

namespace Server
{
    public partial class EigStatusVotes
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long EthnoidioglossId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Ethnoidioglosses Ethnoidiogloss { get; set; }
        public virtual Users User { get; set; }
    }
}
