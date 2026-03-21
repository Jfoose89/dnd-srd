using System.Xml.Linq;
using dnd_srd.Models;
using Microsoft.EntityFrameworkCore;

namespace dnd_srd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //DbSets - one per entity
        public DbSet<Edition> Editions { get; set; }
        public DbSet<MonsterType> MonsterTypes { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<AbilityScores> AbilityScores { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Trait> Traits { get; set; }
        public DbSet<Models.Environment> Environments { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<RuleEntry> RuleEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Self-referencing relationship on Class for subclasses
            modelBuilder.Entity<Class>()
                .HasOne(c => c.ParentClass)
                .WithMany(c => c.Subclasses)
                .HasForeignKey(c => c.ParentClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1:1 Monster -> AbilityScores
            modelBuilder.Entity<Monster>()
                .HasOne(m => m.AbilityScores)
                .WithOne(a => a.Monster)
                .HasForeignKey<AbilityScores>(a => a.MonsterId);

            // Unique name constraints
            modelBuilder.Entity<Edition>()
                .HasIndex(e => e.Name).IsUnique();
            modelBuilder.Entity<MonsterType>()
                .HasIndex(mt => mt.Name).IsUnique();
            modelBuilder.Entity<Monster>()
                .HasIndex(m => m.Name).IsUnique();
            modelBuilder.Entity<Spell>()
                .HasIndex(s => new { s.Name, s.EditionId }).IsUnique();
            modelBuilder.Entity<Race>()
                .HasIndex(r => new { r.Name, r.EditionId }).IsUnique();
            modelBuilder.Entity<Class>()
                .HasIndex(c => new { c.Name, c.EditionId }).IsUnique();
        }
    }
}
