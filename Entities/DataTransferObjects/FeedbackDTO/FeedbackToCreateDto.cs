﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.FeedbackDTO
{
    public class FeedbackToCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [Range(0, 5, ErrorMessage = "Can be from 0 to 5")]
        public double Rating { get; set; }
        [MaxLength(100, ErrorMessage = "Cant be more than 100")]
        public string Comment { get; set; }
    }
}
