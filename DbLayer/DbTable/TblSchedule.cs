using System;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblSchedule : KeyEntity {
        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public ScheduleType ScheduleType { get; set; } = ScheduleType.Single;

        [NotMapped]
        public string ScheduleTitle => EnumExtensions.GetDisplayName ((ScheduleType) ScheduleType);

        // Foreign keys
        [ForeignKey ("SubsetId")]
        public TblSchedule Subset { get; set; }
        public long? SubsetId { get; set; }
    }
}