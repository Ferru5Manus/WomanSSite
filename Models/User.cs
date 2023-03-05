﻿using System.ComponentModel.DataAnnotations;

namespace WomanSite.Models

{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string login { get; set; }
        public string key { get; set; }
    }
}
