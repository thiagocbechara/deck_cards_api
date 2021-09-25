namespace DeckAPI.Domain.Entities
{
    public class Card: BaseEntity
    {
        // Ah, Kh, Tc, 9d, Black Lotus
        public string Value { get; set; }

        //Navigation
        public virtual Deck Deck { get; set; }
    }
}