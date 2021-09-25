using System;
using System.Collections.Generic;

namespace DeckAPI.Domain.Entities
{
    public class Deck : BaseEntity
    {
        public Guid Guid { get; set; }
        public virtual IEnumerable<Card> Cards { get; set; }
    }
}
