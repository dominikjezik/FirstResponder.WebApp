﻿using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Aed Aggregate
    public DbSet<Aed> Aeds { get; set; }
    public DbSet<PublicAed> PublicAeds { get; set; }
    public DbSet<PersonalAed> PersonalAeds { get; set; }
    
    public DbSet<Manufacturer> AedManufacturers { get; set; }
    public DbSet<Language> AedLanguages { get; set; }
    public DbSet<Model> AedModels { get; set; }
    public DbSet<AedPhoto> AedPhotos { get; set; }
    
    // User Aggregate
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUser> GroupUser { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Ignore<User>();
        
        modelBuilder.Entity<Aed>().UseTptMappingStrategy();
        modelBuilder.Entity<PersonalAed>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .IsRequired();
        
        modelBuilder.Entity<Manufacturer>()
            .HasMany<Aed>()
            .WithOne(a => a.Manufacturer)
            .HasForeignKey(a => a.ManufacturerId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Model>()
            .HasMany<Aed>()
            .WithOne(a => a.Model)
            .HasForeignKey(a => a.ModelId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Language>()
            .HasMany<Aed>()
            .WithOne(a => a.Language)
            .HasForeignKey(a => a.LanguageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<GroupUser>()
            .HasKey(groupUser => new { groupUser.GroupId, groupUser.UserId });
        
        modelBuilder.Entity<GroupUser>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired();
    }
    
    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void AddTimestamps()
    {
        var entityEntries = ChangeTracker.Entries().Where(x => 
            x.Entity is IAuditable && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entityEntry in entityEntries)
        {
            var now = DateTime.UtcNow;
            var auditableEntity = (IAuditable)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = now;
            }
            auditableEntity.UpdatedAt = now;
        }
    }
    
}