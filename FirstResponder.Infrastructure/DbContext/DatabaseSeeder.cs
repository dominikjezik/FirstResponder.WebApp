using Bogus;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.Infrastructure.Identity;

namespace FirstResponder.Infrastructure.DbContext;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    
    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public void Seed()
    {
        // Seed database
        var faker = new Faker();
        
        // Seed languages
        var languages = new List<Language>
        {
            new Language { Id = Guid.NewGuid(), Name = "Slovenský" },
            new Language { Id = Guid.NewGuid(), Name = "Anglický" },
        };
        
        _context.AedLanguages.AddRange(languages);
        
        // Seed manufacturers
        var fakerManufacturers = new Faker<Manufacturer>()
            .RuleFor(m => m.Id, f => Guid.NewGuid())
            .RuleFor(m => m.Name, f => f.Company.CompanyName());
        
        var manufacturers = fakerManufacturers.Generate(10);
        _context.AedManufacturers.AddRange(manufacturers);
        
        // Seed models
        var fakerModels = new Faker<Model>()
            .RuleFor(m => m.Id, f => Guid.NewGuid())
            .RuleFor(m => m.Name, f => f.Commerce.ProductName())
            .RuleFor(m => m.ManufacturerId, f => manufacturers[f.Random.Int(0, manufacturers.Count - 1)].Id);
        
        var models = fakerModels.Generate(100);
        _context.AedModels.AddRange(models);
        
        float minLatitude = 48.394886f;
        float maxLatitude = 49.388951f;
        float minLongitude = 17.820355f;
        float maxLongitude = 22.095368f;
        
        // Seed public AEDs
        var fakerPublicAeds = new Faker<PublicAed>()
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.Holder, f => f.Company.CompanyName())
            .RuleFor(a => a.Address, f => f.Address.StreetAddress())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
            .RuleFor(a => a.Region, f => f.PickRandom<RegionOfState>())
            .RuleFor(a => a.Latitude, f => f.Random.Float(minLatitude, maxLatitude))
            .RuleFor(a => a.Longitude, f => f.Random.Float(minLongitude, maxLongitude))
            .RuleFor(a => a.DescriptionLocation, f => f.Lorem.Sentence())
            .RuleFor(a => a.ContactPerson, f => f.Name.FullName())
            .RuleFor(a => a.ContactPersonPhone, f => f.Phone.PhoneNumber())
            .RuleFor(a => a.ContactPersonEmail, f => f.Internet.Email())
            .RuleFor(a => a.State, f => f.PickRandom<AedState>())
            .RuleFor(a => a.SerialNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(a => a.FullyAutomatic, f => f.Random.Bool())
            .RuleFor(a => a.ElectrodesAdults, f => f.Random.Bool())
            .RuleFor(a => a.ElectrodesChildren, f => f.Random.Bool())
            .RuleFor(a => a.Notes, f => f.Lorem.Sentence())
            .RuleFor(a => a.LanguageId, f => languages[f.Random.Int(0, languages.Count - 1)].Id)
            .RuleFor(a => a.ManufacturerId, f => manufacturers[f.Random.Int(0, manufacturers.Count - 1)].Id)
            .RuleFor(a => a.ModelId, f => models[f.Random.Int(0, models.Count - 1)].Id);
        
        var publicAeds = fakerPublicAeds.Generate(100);
        _context.PublicAeds.AddRange(publicAeds);
        
        // Seed application users with password "Password1"
        var fakerUsers = new Faker<ApplicationUser>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FullName, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
            .RuleFor(u => u.UserName, (f, u) => u.Email)
            .RuleFor(u => u.NormalizedUserName, (f, u) => u.Email.ToUpper())
            .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.ToUpper())
            .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(u => u.PasswordHash, f => "AQAAAAIAAYagAAAAEEBlNmh1Dg1K0iOOd5OpuZWN75eNALqi89MQFEyZC/V6y7j+p0iYOEsQL3EFbF/hcw==")
            .RuleFor(u => u.SecurityStamp, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.ConcurrencyStamp, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.PhoneNumberConfirmed, f => f.Random.Bool())
            .RuleFor(u => u.TwoFactorEnabled, f => false)
            .RuleFor(u => u.LockoutEnd, f => null)
            .RuleFor(u => u.LockoutEnabled, f => false)
            .RuleFor(u => u.AccessFailedCount, f => 0);
        
        var users = fakerUsers.Generate(100);
        _context.Users.AddRange(users);
        
        // Seed personal AEDs
        var fakerPersonalAeds = new Faker<PersonalAed>()
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.State, f => f.PickRandom<AedState>())
            .RuleFor(a => a.SerialNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(a => a.FullyAutomatic, f => f.Random.Bool())
            .RuleFor(a => a.ElectrodesAdults, f => f.Random.Bool())
            .RuleFor(a => a.ElectrodesChildren, f => f.Random.Bool())
            .RuleFor(a => a.Notes, f => f.Lorem.Sentence())
            .RuleFor(a => a.LanguageId, f => languages[f.Random.Int(0, languages.Count - 1)].Id)
            .RuleFor(a => a.ManufacturerId, f => manufacturers[f.Random.Int(0, manufacturers.Count - 1)].Id)
            .RuleFor(a => a.ModelId, f => models[f.Random.Int(0, models.Count - 1)].Id)
            .RuleFor(a => a.OwnerId, f => users[f.Random.Int(0, users.Count - 1)].Id);
        
        var personalAeds = fakerPersonalAeds.Generate(100);
        _context.PersonalAeds.AddRange(personalAeds);
        
        // Seed groups
        var fakerGroups = new Faker<Group>()
            .RuleFor(g => g.Id, f => Guid.NewGuid())
            .RuleFor(g => g.Name, f => f.Company.CompanyName())
            .RuleFor(g => g.Description, f => f.Lorem.Sentence());
        
        var groups = fakerGroups.Generate(15);
        _context.Groups.AddRange(groups);
        
        // Seed group users
        var groupUsers = new List<GroupUser>();
        foreach (var group in groups)
        {
            var groupUsersCount = faker.Random.Int(0, 5);
            for (int i = 0; i < groupUsersCount; i++)
            {
                var user = users[faker.Random.Int(0, users.Count - 1)];
                groupUsers.Add(new GroupUser { GroupId = group.Id, UserId = user.Id });
            }
            
            groupUsers = groupUsers.Distinct().ToList();
        }
        
        _context.GroupUser.AddRange(groupUsers);
        
        // Seed incidents
        var fakerIncidents = new Faker<Incident>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.State, f => (f.Random.Float() < 0.8) ? IncidentState.Completed : IncidentState.Canceled)
            .RuleFor(i => i.ResolvedAt, f => f.Date.Past())
            .RuleFor(i => i.Address, f => f.Address.StreetAddress())
            .RuleFor(i => i.Patient, f => f.Name.FullName())
            .RuleFor(i => i.Diagnosis, f => f.Lorem.Sentence())
            .RuleFor(i => i.Latitude, f => f.Random.Float(minLatitude, maxLatitude))
            .RuleFor(i => i.Longitude, f => f.Random.Float(minLongitude, maxLongitude));
        
        var incidents = fakerIncidents.Generate(10);
        _context.Incidents.AddRange(incidents);
        
        
        _context.SaveChanges();
    }
}