using System;

namespace TaskManagementAPI.Models
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;         
        public string? Description { get; set; }           

        public TaskStatus Status { get; set; }             
        public int Priority { get; set; }                  
        public int UserId { get; set; }                    

        public DateTime? DueDate { get; set; }             
        public DateTime CreatedAt { get; set; }            
        public DateTime UpdatedAt { get; set; }            
        public DateTime? CompletedAt { get; set; }        
    }
}
