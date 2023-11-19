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

    public virtual DbSet<TblContent> TblContents { get; set; }

    public virtual DbSet<TblItem> TblItems { get; set; }

    public virtual DbSet<TblItemContent> TblItemContents { get; set; }

    public virtual DbSet<TblItemProperty> TblItemProperties { get; set; }

    public virtual DbSet<TblItemSkill> TblItemSkills { get; set; }

    public virtual DbSet<TblItemType> TblItemTypes { get; set; }

    public virtual DbSet<TblParameter> TblParameters { get; set; }

    public virtual DbSet<TblProperty> TblProperties { get; set; }

    public virtual DbSet<TblSkill> TblSkills { get; set; }

    public virtual DbSet<TblUnit> TblUnits { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserLevel> TblUserLevels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AKBAL\\SQLEXPRESS;Database=db_turnbase_dev;Trusted_Connection=False;User Id=sa;Password=vb79n7nq;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblContent>(entity =>
        {
            entity.ToTable("tbl_contents");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TblItem>(entity =>
        {
            entity.ToTable("tbl_items");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Type).WithMany(p => p.TblItems)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_items_tbl_item_types");
        });

        modelBuilder.Entity<TblItemContent>(entity =>
        {
            entity.HasKey(e => e.ItemContentId);

            entity.ToTable("tbl_item_contents");

            entity.Property(e => e.ItemContentId).HasColumnName("ItemContentID");
            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.IndexId).HasColumnName("IndexID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Content).WithMany(p => p.TblItemContents)
                .HasForeignKey(d => d.ContentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_item_contents_tbl_contents");

            entity.HasOne(d => d.Item).WithMany(p => p.TblItemContents)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_item_contents_tbl_items");
        });

        modelBuilder.Entity<TblItemProperty>(entity =>
        {
            entity.HasKey(e => e.ItemPropertyId);

            entity.ToTable("tbl_item_properties");

            entity.HasIndex(e => new { e.ItemId, e.PropertyId }, "IX_tbl_item_properties");

            entity.Property(e => e.ItemPropertyId).HasColumnName("ItemPropertyID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");

            entity.HasOne(d => d.Item).WithMany(p => p.TblItemProperties)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_item_properties_tbl_items");

            entity.HasOne(d => d.Property).WithMany(p => p.TblItemProperties)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_item_properties_tbl_properties");
        });

        modelBuilder.Entity<TblItemSkill>(entity =>
        {
            entity.HasKey(e => e.ItemSkillId);

            entity.ToTable("tbl_item_skills");

            entity.HasIndex(e => new { e.ItemId, e.SkillId }, "IX_tbl_item_skills").IsUnique();

            entity.HasOne(d => d.Item).WithMany(p => p.TblItemSkills)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_item_skills_tbl_items");

            entity.HasOne(d => d.Skill).WithMany(p => p.TblItemSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_item_skills_tbl_skills");
        });

        modelBuilder.Entity<TblItemType>(entity =>
        {
            entity.ToTable("tbl_item_types");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TblParameter>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_parameters");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblProperty>(entity =>
        {
            entity.ToTable("tbl_properties");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSkill>(entity =>
        {
            entity.ToTable("tbl_skills");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TblUnit>(entity =>
        {
            entity.ToTable("tbl_units");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

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
