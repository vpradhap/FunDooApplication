﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class UserResetModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
