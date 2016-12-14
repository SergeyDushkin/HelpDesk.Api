using Microsoft.EntityFrameworkCore;

namespace servicedesk.api
{
    public class HelpDeskDbContext : DbContext
    {
        public HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // add this:
            modelBuilder.Entity<LOCATION>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<LOCATION_TYPE>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<BRAND>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<USER>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<ROLE>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<REQUEST>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<REQUEST_JOB>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<REQUEST_TIME_CATEGORY>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<CONNECTION>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<CONNECTION_TYPE>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<REQUEST_NOTIFICATION_ITEM>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<INCIDENT_NOTIFICATION_ITEM>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<INCIDENT>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<LOCATION_STORE_TYPES>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<CACHE>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<AUDIT>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<REMINDER_MESSAGES>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<TEMPLATE>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            modelBuilder.Entity<USER_CONTACT>().Property(p => p.GUID_RECORD).ValueGeneratedOnAdd();
            
            //modelBuilder.Entity<LOCATION>()
            //    .HasOne(p => p.CONTACT)
            //    .WithOne(i => i.REFERENCE)
            //    .HasForeignKey<LOCATION_CONTACT_INFO>(b => b.REFERENCE_GUID);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LOCATION> Locations { get; set; }
        public DbSet<LOCATION_TYPE> LocationTypes { get; set; }
        public DbSet<LOCATION_CONTACT_INFO> LocationContacts { get; set; }
        public DbSet<BRAND> Brands { get; set; }
        public DbSet<USER> Users { get; set; }
        public DbSet<ROLE> Roles { get; set; }
        public DbSet<USER_ROLE> UserRoles { get; set; }
        public DbSet<REQUEST> Requests { get; set; }
        public DbSet<REQUEST_JOB> RequestJobs { get; set; }
        public DbSet<AUDITOR> Auditors { get; set; }
        public DbSet<REQUEST_TIME_CATEGORY> RequestTimeCategories { get; set; }
        public DbSet<CONNECTION> Connections { get; set; }
        public DbSet<CONNECTION_TYPE> ConnectionTypes { get; set; }
        public DbSet<REQUEST_NOTIFICATION_ITEM> RequestNotificationList { get; set; }
        public DbSet<INCIDENT_NOTIFICATION_ITEM> IncidentNotificationList { get; set; }
        public DbSet<INCIDENT> Incidents { get; set; }
        public DbSet<LOCATION_STORE_TYPES> LOCATION_STORE_TYPES { get; set; }
        public DbSet<CACHE> CACHES { get; set; }
        public DbSet<AUDIT> AUDITS { get; set; }
        public DbSet<REMINDER_MESSAGES> REMINDER_MESSAGES { get; set; }
        public DbSet<TEMPLATE> Templates { get; set; }
        public DbSet<USER_CONTACT> UserContact { get; set; }
    }
}
