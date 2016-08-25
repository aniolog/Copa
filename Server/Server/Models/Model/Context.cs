using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Context : DbContext
    {


        public Context()
            : base("Database")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Bill> Bills { set; get; }

        public DbSet<CrewMember> CrewMembers { set; get; }

        public DbSet<Currency> Currencies { set; get; }

        public DbSet<Device> Devices { set; get; }

        public DbSet<LogisticsDelegate> LogisticDelegates { set; get; }

        public DbSet<Place> Places { set; get; }

        public DbSet<Provider> Providers { set; get; }

        public DbSet<Rate> Rates { set; get; }

        public DbSet<Request> Requests { set; get; }

        public DbSet<TeamMember> TeamMembers { set; get; }

        public DbSet<Vehicle> Vehicles { set; get; }

        public DbSet<RequestedVehicle> RequestedVehicles { set; get; }

    }
}