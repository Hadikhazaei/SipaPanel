using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// 
using DbLayer.DbTable;
using DbLayer.DbTable.Identity;

namespace DbLayer.Context {
    public class AppDbContext : IdentityDbContext<AppUser> {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<TblHall> TblHall { get; set; }
        public DbSet<TblHallSchedule> TblHallSchedule { get; set; }
        public DbSet<TblHallTemplate> TblHallTemplate { get; set; }
        public DbSet<TblStop> TblStop { get; set; }
        public DbSet<TblStopInfo> TblStopInfo { get; set; }
        public DbSet<TblFusion> TblFusion { get; set; }
        public DbSet<TblTemplate> TblTemplate { get; set; }
        public DbSet<TblProduct> TblProduct { get; set; }
        public DbSet<JoinTP> JoinTP { get; set; }
        public DbSet<TblDefect> TblDefect { get; set; }
        public DbSet<TblQControl> TblQControl { get; set; }
        public DbSet<TblQControlInfo> TblQControlInfo { get; set; }
        public DbSet<TblSchedule> TblSchedule { get; set; }
        public DbSet<TblTonnage> TblTonnage { get; set; }
        public DbSet<TblPlanning> TblPlanning { get; set; }
        public DbSet<TblPlanningInfo> TblPlanningInfo { get; set; }
        public DbSet<TblProductionInfo> TblProductionInfo { get; set; }
        public DbSet<TblKarset> TblKarset { get; set; }

        protected override void OnModelCreating (ModelBuilder builder) {
            #region :: hall ::
            builder.Entity<TblHall> ()
                .HasIndex (u => u.Line)
                .IsUnique (true);
            // 
            builder.Entity<TblHallTemplate> ()
                .HasKey (x => new { x.TblHallId, x.TblTemplateId });
            builder.Entity<TblHallTemplate> ()
                .HasOne (a => a.TblHall)
                .WithMany (ar => ar.TblHallTemplate)
                .HasForeignKey (a => a.TblHallId)
                .OnDelete (DeleteBehavior.Cascade);
            builder.Entity<TblHallTemplate> ()
                .HasOne (r => r.TblTemplate)
                .WithMany (ar => ar.TblHallTemplate)
                .HasForeignKey (r => r.TblTemplateId)
                .OnDelete (DeleteBehavior.Cascade);
            // builder.AddHallMapping ();
            #endregion

            #region :: template ::
            builder.Entity<TblTemplate> ()
                .HasIndex (u => u.Code)
                .IsUnique (true);
            #endregion

            #region :: schedule ::
            // foreach (var e in Enum.GetValues (typeof (ScheduleType)).Cast<ScheduleType> ()) {
            //     var id = (long) e;
            //     builder.Entity<TblSchedule> ().HasData (new TblSchedule { Id = id, ScheduleType = e });
            //     for (var i = 0; i < (byte) e; i++) {
            //         var subsetId = (id * 2) + (i + 1);
            //         builder.Entity<TblSchedule> ().HasData (new TblSchedule {
            //             Id = subsetId,
            //                 SubsetId = id,
            //                 ScheduleType = e,
            //                 BeginTime = TimeSpan.FromHours (0),
            //                 FinishTime = TimeSpan.FromHours (0)
            //         });
            //     }
            // }
            #endregion

            #region :: stop ::
            // builder.Entity<TblStop> ()
            //     .HasMany<TblStopInfo> (s => s.TblStopInfo)
            //     .WithOne (ta => ta.TblStop)
            //     .HasForeignKey (u => u.TblStopId)
            //     .OnDelete (DeleteBehavior.Restrict);
            #endregion

            #region :: planning ::
            // builder.Entity<TblPlanning> ()
            //     .HasMany<TblStopInfo> (s => s.TblStopInfo)
            //     .WithOne (ta => ta.TblPlanning)
            //     .HasForeignKey (u => u.TblPlanningId)
            //     .OnDelete (DeleteBehavior.Restrict);
            // builder.Entity<TblPlanning> ()
            //     .HasMany<TblQControl> (s => s.TblQControl)
            //     .WithOne (ta => ta.TblPlanning)
            //     .HasForeignKey (u => u.TblPlanningId)
            //     .OnDelete (DeleteBehavior.Restrict);
            #endregion

            #region :: template ::
            // builder.Entity<TblTemplate> ()
            //     .HasMany<TblStopInfo> (s => s.TblStopInfo)
            //     .WithOne (ta => ta.TblTemplate)
            //     .HasForeignKey (u => u.TblTemplateId)
            //     .OnDelete (DeleteBehavior.Restrict);
            // builder.Entity<TblTemplate> ()
            //     .HasMany<TblPlanningInfo> (s => s.TblPlanningInfo)
            //     .WithOne (ta => ta.TblTemplate)
            //     .HasForeignKey (u => u.TblTemplateId)
            //     .OnDelete (DeleteBehavior.Restrict);
            // builder.Entity<TblTemplate> ()
            //     .HasMany<TblProductionInfo> (s => s.TblProductionInfo)
            //     .WithOne (ta => ta.TblTemplate)
            //     .HasForeignKey (u => u.TblTemplateId)
            //     .OnDelete (DeleteBehavior.Restrict);
            #endregion

            #region :: identity ::
            // IdentityMapping.AddIdentityMapping (builder);
            #endregion

            #region ### cascade setting ###
            // var cascadeFKs = builder.Model.GetEntityTypes ().SelectMany (t => t.GetForeignKeys ())
            //     .Where (fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            // foreach (var fk in cascadeFKs) {
            //     fk.DeleteBehavior = DeleteBehavior.Restrict;
            // }
            #endregion

            base.OnModelCreating (builder);
        }
    }
}