using System;
using System.ComponentModel.DataAnnotations;

namespace VideoconferencingBackend.Models.DBModels
{
    public class Message
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public Group Group { get; set; }
        [Required]
        [StringLength(4096, MinimumLength = 1, ErrorMessage = "Message length should be less than 4096 symbols")]
        public string Text { get; set; }
    }
}
