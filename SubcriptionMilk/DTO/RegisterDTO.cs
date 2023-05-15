﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;

namespace SubcriptionMilk.DTO
{
    public partial class RegisterDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Gender { get; set; }
    }
}
