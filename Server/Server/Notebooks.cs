using System;
using System.Collections.Generic;

namespace Server
{
    public partial class Notebooks
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Users User { get; set; }
    }
}
