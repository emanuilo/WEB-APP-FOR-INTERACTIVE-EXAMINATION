namespace WebRole1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class baza : DbContext
    {
        public baza()
            : base("name=baza")
        {
        }

        public virtual DbSet<Kanal> Kanals { get; set; }
        public virtual DbSet<Klon> Klons { get; set; }
        public virtual DbSet<Korisnik> Korisniks { get; set; }
        public virtual DbSet<Narudzbina> Narudzbinas { get; set; }
        public virtual DbSet<Odgovor> Odgovors { get; set; }
        public virtual DbSet<Parametri> Parametris { get; set; }
        public virtual DbSet<Pitanje> Pitanjes { get; set; }
        public virtual DbSet<PonudjeniOdg> PonudjeniOdgs { get; set; }
        public virtual DbSet<Prati> Pratis { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kanal>()
                .Property(e => e.Naziv)
                .IsUnicode(false);

            modelBuilder.Entity<Kanal>()
                .Property(e => e.Lozinka)
                .IsUnicode(false);

            modelBuilder.Entity<Kanal>()
                .HasMany(e => e.Odgovors)
                .WithRequired(e => e.Kanal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Klon>()
                .Property(e => e.Naslov)
                .IsUnicode(false);

            modelBuilder.Entity<Klon>()
                .Property(e => e.Tekst)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.Ime)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.Prezime)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.Lozinka)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.Uloga)
                .IsUnicode(false);

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.Kanals)
                .WithRequired(e => e.Korisnik)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.Odgovors)
                .WithRequired(e => e.Korisnik)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.Narudzbinas)
                .WithRequired(e => e.Korisnik)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.Pratis)
                .WithRequired(e => e.Korisnik)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.Pitanjes)
                .WithRequired(e => e.Korisnik)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Narudzbina>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Pitanje>()
                .Property(e => e.Naslov)
                .IsUnicode(false);

            modelBuilder.Entity<Pitanje>()
                .Property(e => e.Tekst)
                .IsUnicode(false);

            modelBuilder.Entity<Pitanje>()
                .HasOptional(e => e.Klon)
                .WithRequired(e => e.Pitanje);

            modelBuilder.Entity<Pitanje>()
                .HasMany(e => e.Odgovors)
                .WithRequired(e => e.Pitanje)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pitanje>()
                .HasMany(e => e.PonudjeniOdgs)
                .WithRequired(e => e.Pitanje)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PonudjeniOdg>()
                .Property(e => e.Sadrzaj)
                .IsUnicode(false);

            modelBuilder.Entity<PonudjeniOdg>()
                .HasMany(e => e.Odgovors)
                .WithRequired(e => e.PonudjeniOdg)
                .WillCascadeOnDelete(false);
        }
    }
}
