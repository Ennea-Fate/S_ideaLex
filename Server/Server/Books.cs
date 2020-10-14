using System;
using System.Collections.Generic;

namespace Server
{
    public partial class Books
    {
        public Books()
        {
            BookEigs = new HashSet<BookEigs>();
            ConcordanceBooks = new HashSet<ConcordanceBooks>();
            Ethnoidioglosses = new HashSet<Ethnoidioglosses>();
            FrequencyVocabularies = new HashSet<FrequencyVocabularies>();
            GrammarDictionaries = new HashSet<GrammarDictionaries>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public long AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Authors Author { get; set; }
        public virtual ICollection<BookEigs> BookEigs { get; set; }
        public virtual ICollection<ConcordanceBooks> ConcordanceBooks { get; set; }
        public virtual ICollection<Ethnoidioglosses> Ethnoidioglosses { get; set; }
        public virtual ICollection<FrequencyVocabularies> FrequencyVocabularies { get; set; }
        public virtual ICollection<GrammarDictionaries> GrammarDictionaries { get; set; }
    }
}
