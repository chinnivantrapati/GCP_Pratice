using System.ComponentModel.DataAnnotations;

namespace GCP_Pratice.Models 
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public bool IsCompleted { get; set; }
    }
}