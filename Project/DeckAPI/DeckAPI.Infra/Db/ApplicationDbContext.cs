using DeckAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeckAPI.Infra.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNull] DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Deck> Decks { get; set; }
    }
}
