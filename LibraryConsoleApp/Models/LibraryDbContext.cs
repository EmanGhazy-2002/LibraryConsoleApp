using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryConsoleApp.Models;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCheckout> BookCheckouts { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-45SSJQ2;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC075F7307D5");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC0797FF9571");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Books__AuthorId__398D8EEE");
        });

        modelBuilder.Entity<BookCheckout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookChec__3214EC0782BE1B68");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCheckouts)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookCheck__BookI__3E52440B");

            entity.HasOne(d => d.Member).WithMany(p => p.BookCheckouts)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__BookCheck__Membe__3F466844");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Members__3214EC07B1413B2E");

            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
