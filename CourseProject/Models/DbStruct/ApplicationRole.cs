﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class ApplicationRole: IdentityRole
    {
        public IEnumerable<ApplicationUser> Members { get; set; }
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
    }
    public class RoleEditModel
    {
        public ApplicationRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}