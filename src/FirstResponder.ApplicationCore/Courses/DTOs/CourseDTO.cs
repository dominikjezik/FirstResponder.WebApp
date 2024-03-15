namespace FirstResponder.ApplicationCore.Courses.DTOs;

public class CourseDTO
{
    public Guid CourseId { get; set; }
    
    public CourseFormDTO CourseForm { get; set; }
    
    public string? CourseTypeName { get; set; }
    
    public List<ParticipantItemDTO> Participants { get; set; } = new();
    
    public class ParticipantItemDTO
    {
        public Guid ParticipantId { get; set; }
        
        public string FullName { get; set; }
        
        public string Email { get; set; }
        
        public DateTime? CreatedAt { get; set; }
    }
}