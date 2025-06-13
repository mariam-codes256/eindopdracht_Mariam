using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnackLab.WebApp.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

public class UserRole
    {
        public string UserId { get; set; }
        public string  UserName { get; set; }

        public List<Role> UserRoles { get; set; }

        public UserRole()
        {
            UserRoles = new List<Role>();
        }
    }
}

