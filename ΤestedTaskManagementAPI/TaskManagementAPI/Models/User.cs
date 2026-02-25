namespace TaskManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;      
        public string Email { get; set; } = null!;         
        public string PasswordHash { get; set; } = null!;  

        public string? FirstName { get; set; }            
        public string? LastName { get; set; }              

        public DateTime CreatedAt { get; set; }            
        public DateTime UpdatedAt { get; set; }           
        public bool IsActive { get; set; }                 
    }
}
