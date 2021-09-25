using DeckAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckAPI.Infra.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Deck> Decks { get; }
        IRepository<Card> Cards { get; }

        Task CommitAsync();
    }
}
