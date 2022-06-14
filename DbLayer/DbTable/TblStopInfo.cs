using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.DbTable.Base;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblStopInfo : KeyEntity {
        public TblStopInfo () {
            ShamsiDate = new ShamsiDateSegment (BeginDate);
        }

        public string Brief { get; set; }

        public StopType StopType { get; set; }

        [StringLength (50)]
        public string TechnicalNumber { get; set; }

        [NotMapped]
        public string StopTitle => EnumExtensions.GetDisplayName ((StopType) StopType);

        // 
        public ShamsiDateSegment ShamsiDate { get; set; }

        [Column (TypeName = "smalldatetime")]
        public DateTime BeginDate { get; set; } = DateTime.Now;

        public bool ModifyBeginDate { get; set; } = false;

        [NotMapped]
        public string PersianBeginDate => BeginDate.ToShortPersianDateTimeString ();

        [Column (TypeName = "smalldatetime")]
        public DateTime? FinishDate { get; set; }
        public bool ModifyFinishDate { get; set; } = false;

        [NotMapped]
        public string PersianFinishDate => FinishDate.HasValue ? FinishDate.Value.ToShortPersianDateTimeString () : "---";

        [NotMapped]
        public TimeSpan CalculatedTime => FinishDate.HasValue ?
            (FinishDate.Value - BeginDate) : TimeSpan.Zero;

        public ScheduleType ScheduleType { get; set; } = ScheduleType.Single;

        [NotMapped]
        public string ScheduleTitle => EnumExtensions.GetDisplayName ((ScheduleType) ScheduleType);

        // Foreign keys
        [ForeignKey ("TblStopId")]
        public TblStop TblStop { get; set; }
        public long TblStopId { get; set; }

        [ForeignKey ("UserId")]
        public AppUser AppUser { get; set; }
        public string UserId { get; set; }

        [ForeignKey ("TblTemplateId")]
        public TblTemplate TblTemplate { get; set; }
        public long TblTemplateId { get; set; }

        [ForeignKey ("TblPlanningId")]
        public TblPlanning TblPlanning { get; set; }
        public long TblPlanningId { get; set; }
    }
}