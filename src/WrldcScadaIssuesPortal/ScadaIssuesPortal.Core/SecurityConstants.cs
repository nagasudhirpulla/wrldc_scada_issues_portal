﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScadaIssuesPortal.Core
{
    public static class SecurityConstants
    {
        public const string GuestRoleString = "GuestUser";
        public const string AdminRoleString = "Administrator";
        public static List<string> GetRoles()
        {
            return typeof(SecurityConstants).GetFields().Select(x => x.GetValue(null).ToString()).ToList();
        }
    }
}
