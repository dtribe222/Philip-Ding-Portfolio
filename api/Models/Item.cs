using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace quiz.Model
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float StartBid { get; set; }
        public float CurrentBid { get; set; }
        public string State {get; set;}
    }
}