﻿using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Aed Aggregate
    public DbSet<Aed> Aeds { get; set; }
    public DbSet<Manufacturer> AedManufacturers { get; set; }
    public DbSet<Language> AedLanguages { get; set; }
    public DbSet<Model> AedModels { get; set; }
    
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