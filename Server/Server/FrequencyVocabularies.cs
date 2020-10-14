using System;
using System.Collections.Generic;

namespace Server
{
    public partial class FrequencyVocabularies
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long BookId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Books Book { get; set; }
    }
}
