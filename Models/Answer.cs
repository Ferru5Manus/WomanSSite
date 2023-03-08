using System.ComponentModel.DataAnnotations;

namespace WomanSite.Models
{
    public class Answer
    {
        [Key]
        public int id { get; set; }
        public int questionId { get; set; }
        public string userLogin { get; set; }
        public string answer { get; set; }
    }
}
