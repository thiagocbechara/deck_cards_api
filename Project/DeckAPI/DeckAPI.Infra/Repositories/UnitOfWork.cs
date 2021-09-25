using DeckAPI.Domain.Entities;
using DeckAPI.Infra.Db;
using DeckAPI.Infra.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace DeckAPI.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Deck> _deckRepository = null;
        private IRepository<Card> _cardsRepository = null;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Deck> Decks
        {
            get
            {
                if(_deckRepository == null)
                {
                    _deckRepository = new Repository<Deck>(_context);
                }
                return _deckRepository;
            }
        }
        public IRepository<Card> Cards => _cardsRepository ??= new Repository<Card>(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
