﻿using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
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
    
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationUser> NotificationUser { get; set; }
    
    public DbSet<DeviceToken> DeviceTokens { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    // Incident Aggregate
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<IncidentResponder> IncidentResponders { get; set; }
    
    public DbSet<IncidentReport> IncidentReports { get; set; }
    
    public DbSet<IncidentMessage> IncidentMessages { get; set; }
    
    // Course Aggregate
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseType> CourseTypes { get; set; }
    public DbSet<CourseUser> CourseUser { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Ignore<User>();
        
        modelBuilder.Entity<Aed>().UseTpcMappingStrategy();
        modelBuilder.Entity<PersonalAed>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .IsRequired();
        
        modelBuilder.Entity<PublicAed>()
            .OwnsOne(a => a.Availability);
        
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
            .WithMany(user => user.Groups)
            .HasForeignKey(a => a.UserId)
            .IsRequired();
        
        modelBuilder.Entity<NotificationUser>()
            .HasKey(notificationUser => new { notificationUser.NotificationId, notificationUser.UserId });
        
        modelBuilder.Entity<NotificationUser>()
            .HasOne<ApplicationUser>()
            .WithMany(user => user.Notifications)
            .HasForeignKey(n => n.UserId)
            .IsRequired();
        
        modelBuilder.Entity<IncidentResponder>()
            .HasKey(incidentResponder => new { incidentResponder.IncidentId, incidentResponder.ResponderId });
        
        modelBuilder.Entity<IncidentResponder>()
            .HasOne<ApplicationUser>()
            .WithMany(user => user.Incidents)
            .HasForeignKey(a => a.ResponderId)
            .IsRequired();

        modelBuilder.Entity<IncidentResponder>()
            .HasOne<IncidentReport>(i => i.Report)
            .WithOne()
            .HasForeignKey<IncidentResponder>(a => a.ReportId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<CourseType>()
            .HasMany<Course>()
            .WithOne(c => c.CourseType)
            .HasForeignKey(c => c.CourseTypeId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<CourseUser>()
            .HasKey(courseUser => new { courseUser.CourseId, courseUser.UserId });
        
        modelBuilder.Entity<CourseUser>()
            .HasOne<ApplicationUser>()
            .WithMany(user => user.Courses)
            .HasForeignKey(c => c.UserId)
            .IsRequired();
        
        modelBuilder.Entity<IncidentMessage>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.SenderId)
            .IsRequired();
        
        modelBuilder.Entity<DeviceToken>()
            .HasOne<ApplicationUser>()
            .WithMany(user => user.DeviceTokens)
            .HasForeignKey(a => a.UserId)
            .IsRequired();
        
        modelBuilder.Entity<RefreshToken>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired();
        
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(a => a.Token)
            .IsUnique();
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
    
    // Inspired by https://stackoverflow.com/questions/45429719/automatic-createdat-and-updatedat-fields-onmodelcreating-in-ef6
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