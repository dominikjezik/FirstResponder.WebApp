using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserDTO
{
    public Guid UserId { get; set; }
    
    public UserFormDTO UserForm { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public IEnumerable<Group> Groups { get; set; } = new List<Group>();
    
    public List<IncidentItemDTO> Incidents { get; set; } = new();
    
    public List<CourseItemDTO> Courses { get; set; } = new();
    
    public class IncidentItemDTO
    {
        public Guid IncidentId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string Address { get; set; }
    }
    
    public class CourseItemDTO
    {
        public Guid CourseId { get; set; }
        
        public string Name { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}