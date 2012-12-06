using System.ComponentModel.DataAnnotations;

namespace FutureDev.Hello.Models
{
    public class Todo
    {
        public int TodoId { get; set; }

        [Required]
        public string Task { get; set; }
        public bool IsDone { get; set; }
    }
}