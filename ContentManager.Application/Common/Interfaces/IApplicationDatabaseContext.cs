using ContentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentManager.Application.Common.Interfaces
{
    internal interface IApplicationDatabaseContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Publication> Publications
        {
            get; set;
        }
}
