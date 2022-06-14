using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblProductionInfo : CreateEntity {
        public TblProductionInfo () {
            CreatedDate = DateTime.Now;
        }

        private DateTime _createdDate;

        [Column (TypeName = "smalldatetime")]
        public override DateTime CreatedDate {
            get => _createdDate;
            set {
                _createdDate = value;
                ShamsiDate = new ShamsiDateSegment (_createdDate);
            }
        }

        public ShamsiDateSegment ShamsiDate { get; set; }

        public int PutCount { get; set; }

        public int TakeCount { get; set; }

        [StringLength (50)]
        public string WeightCode { get; set; }

        public ChargeType ChargeType { get; set; } = ChargeType.Poring;

        [NotMapped]
        public string ChargeTitle => EnumExtensions.GetDisplayName ((ChargeType) ChargeType);

        public ScheduleType ScheduleType { get; set; } = ScheduleType.Single;

        [NotMapped]
        public string ScheduleTitle => EnumExtensions.GetDisplayName ((ScheduleType) ScheduleType);

        public long HallId { get; set; }

        // Foreign keys
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