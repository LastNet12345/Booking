﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public DateTime TimeOfRegistration { get; set; }
        public ICollection<ApplicationUserGymClass> AttendingClasses { get; set; } = new List<ApplicationUserGymClass>();
    }
}
