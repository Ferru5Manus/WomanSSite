using System.ComponentModel.DataAnnotations;

namespace WomanSite.Models
{
    public class Question
    {
        [Key]
        public int id { get; set; }
        public string personName { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string guess { get; set; }
    }
}
