﻿using ScadaIssuesPortal.Core.Interfaces.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using ScadaIssuesPortal.Core;

namespace ScadaIssuesPortal.App.Security
{
    public class AppIdentityInitializer : IAppIdentityInitializer
    {

        public UserManager<IdentityUser> UserManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public IConfiguration Configuration { get; set; }

        /**
         * This method seeds admin, guest role and admin user
         * **/
        public void SeedData()
        {
            // get admin params from configuration
            UserInitVariables initVariables = new UserInitVariables();
            initVariables.InitializeFromConfig(Configuration);
            // seed roles
            SeedUserRoles(RoleManager);
            // seed admin user
            SeedAdminUser(UserManager, initVariables);
        }

        /**
         * This method seeds admin user
         * **/
        public void SeedAdminUser(UserManager<IdentityUser> userManager, UserInitVariables initVariables)
        {
            string AdminUserName = initVariables.AdminUserName;
            string AdminEmail = initVariables.AdminEmail;
            string AdminPassword = initVariables.AdminPassword;

            // check if admin user doesn't exist
            if (userManager.FindByNameAsync(AdminUserName).Result == null)
            {
                // create desired admin user object
                IdentityUser user = new IdentityUser
                {
                    UserName = AdminUserName,
                    Email = AdminEmail
                };

                // push desired admin user object to DB
                IdentityResult result = userManager.CreateAsync(user, AdminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, SecurityConstants.AdminRoleString).Wait();
                }
            }
        }

        /**
         * This method seeds roles
         * **/
        public void SeedUserRoles(RoleManager<IdentityRole> roleManager)
        {
            // check if role doesn't exist
            if (!roleManager.RoleExistsAsync(SecurityConstants.GuestRoleString).Result)
            {
                // create desired role object
                IdentityRole role = new IdentityRole
                {
                    Name = SecurityConstants.GuestRoleString,
                };
                // push desired role object to DB
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync(SecurityConstants.AdminRoleString).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = SecurityConstants.AdminRoleString,
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
