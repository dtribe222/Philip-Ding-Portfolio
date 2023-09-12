﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace quiz.Dto
{
    public class ItemInput
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public float? StartBid { get; set; }
    }
}