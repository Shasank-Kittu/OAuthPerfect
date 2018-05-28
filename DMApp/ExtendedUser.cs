using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace UserManager.Models
{
    public class ExtendedUser:IdentityUser
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string Country { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ExtendedUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
    public class ExtendedUserDbContext:IdentityDbContext<ExtendedUser>
    {
        public ExtendedUserDbContext() : base("DefaultConnection")
        {

        }
        public static ExtendedUserDbContext Create()
        {
            return new ExtendedUserDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var user = modelBuilder.Entity<ExtendedUser>();
            user.Property(x => x.Name).IsRequired();
        }
    }
}