﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class CatsContext : DbContext
{
    public CatsContext(DbContextOptions<CatsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cat> Cat { get; set; }

    public virtual DbSet<Tag> Tag { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>().HasKey(c => c.Id);
        modelBuilder.Entity<Tag>().HasKey(t => t.Id);

        modelBuilder.Entity<Cat>().HasIndex(c => c.CatId).IsUnique();
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

        modelBuilder.Entity<Cat>()
    .HasMany(c => c.Tags)
    .WithMany(t => t.Cats)
    .UsingEntity<Dictionary<string, object>>(
        "CatTags",
        j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
        j => j.HasOne<Cat>().WithMany().HasForeignKey("CatId"),
        j => j.HasKey("CatId", "TagId"));

    }
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Cats>(entity =>
    //    {
    //        entity.HasKey(e => e.Id).HasName("PK__Cats__3214EC07C304D71F");

    //        entity.HasIndex(e => e.CatId, "UQ__Cats__6A1C8AFB27BE789C").IsUnique();

    //        entity.Property(e => e.CatId)
    //            .IsRequired()
    //            .HasMaxLength(100);
    //        entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");
    //        entity.Property(e => e.Image).IsRequired();

    //        entity.HasMany(d => d.TagEntity).WithMany(p => p.CatEntity)
    //            .UsingEntity<Dictionary<string, object>>(
    //                "CatTags",
    //                r => r.HasOne<Tags>().WithMany()
    //                    .HasForeignKey("TagEntityId")
    //                    .HasConstraintName("FK__CatTags__TagEnti__2D27B809"),
    //                l => l.HasOne<Cats>().WithMany()
    //                    .HasForeignKey("CatEntityId")
    //                    .HasConstraintName("FK__CatTags__CatEnti__2C3393D0"),
    //                j =>
    //                {
    //                    j.HasKey("CatEntityId", "TagEntityId").HasName("PK__CatTags__5C83F112A1538CB2");
    //                });
    //    });

    //    modelBuilder.Entity<Tags>(entity =>
    //    {
    //        entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC07F674FEA5");

    //        entity.HasIndex(e => e.Name, "UQ__Tags__737584F622355068").IsUnique();

    //        entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");
    //        entity.Property(e => e.Name)
    //            .IsRequired()
    //            .HasMaxLength(100);
    //    });

    //OnModelCreatingPartial(modelBuilder);
    // }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}