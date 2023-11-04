using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TurnBase.DBLayer.Models;

public partial class DbTurnBaseDevContext : DbContext
{
    public DbTurnBaseDevContext()
    {
    }

    public DbTurnBaseDevContext(DbContextOptions<DbTurnBaseDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserLevel> TblUserLevels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AKBAL\\SQLEXPRESS;Database=db_turnbase_dev;Trusted_Connection=False;User Id=sa;Password=vb79n7nq;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("tbl_users");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserLevel).HasDefaultValueSql("((1))");
            entity.Property(e => e.Username).HasMaxLength(18);
        });

        modelBuilder.Entity<TblUserLevel>(entity =>
        {
            entity.ToTable("tbl_user_levels");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
