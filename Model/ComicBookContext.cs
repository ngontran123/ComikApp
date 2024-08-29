using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestComikApp.Model;

public partial class ComicBookContext : DbContext
{
    public ComicBookContext()
    {
    }

    public ComicBookContext(DbContextOptions<ComicBookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=ComicBook;Username=postgres;Password=miyuki123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .HasComment("Identifier for username")
                .HasColumnName("id");
            entity.Property(e => e.Avatar)
                .HasColumnType("character varying")
                .HasColumnName("avatar");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasColumnType("character varying")
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasComment("Password for the user")
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phonenumber)
                .HasColumnType("character varying")
                .HasColumnName("phonenumber");
            entity.Property(e => e.Username)
                .HasComment("Username")
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
