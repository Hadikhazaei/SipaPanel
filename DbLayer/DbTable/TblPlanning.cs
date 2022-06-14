using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.DbTable.Base;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblPlanning : UpdateEntity {
        [Required]
        [StringLength (50)]
        public string Title { get; set; }

        [Column (TypeName = "smalldatetime")]
        public DateTime BeginDate { get; set; }

        [NotMapped]
        public string PersianBeginDate => BeginDate.ToShortPersianDateString ();

        [Column (TypeName = "smalldatetime")]
        public DateTime FinishDate { get; set; }

        [NotMapped]
        public string PFinishDate => FinishDate.ToShortPersianDateString ();

        [NotMapped]
        public short ConsideredDays => (short) (FinishDate - BeginDate).TotalDays;

        public short SchdeduleCount { get; set; } = 1;

        public ScheduleType ScheduleType { get; set; } = ScheduleType.Single;

        [NotMapped]
        public string ScheduleTitle => EnumExtensions.GetDisplayName ((ScheduleType) ScheduleType);

        // Foreign keys
        [ForeignKey ("TblHallId")]
        public TblHall TblHall { get; set; }
        public long TblHallId { get; set; }

        [ForeignKey ("UserId")]
        public AppUser AppUser { get; set; }
        public string UserId { get; set; }

        // Collections
        public ICollection<TblTonnage> TblTonnage { get; set; }

        public ICollection<TblQControl> TblQControl { get; set; }

        public ICollection<TblStopInfo> TblStopInfo { get; set; }

        public ICollection<TblPlanningInfo> TblPlanningInfo { get; set; }

        public ICollection<TblProductionInfo> TblProductionInfo { get; set; }
    }
}