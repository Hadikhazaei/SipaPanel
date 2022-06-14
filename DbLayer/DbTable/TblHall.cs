using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;
using DbLayer.DbTable.Identity;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblHall : KeyEntity {
        public HallType HallType { get; set; }

        [NotMapped]
        public string HallTitle => EnumExtensions.GetDisplayName ((HallType) HallType);

        [NotMapped]
        public string FullTitle => $"{HallTitle}-{Line}";

        [Required]
        [StringLength (50)]
        public string Line { get; set; }

        public bool IsWorking { get; set; } = true;

        // Collection
        public ICollection<TblStop> TblStop { set; get; }

        public ICollection<AppUser> AppUser { set; get; }

        public ICollection<TblPlanning> TblPlanning { set; get; }

        public ICollection<TblHallSchedule> TblHallSchedule { set; get; }

        // Many to many
        public ICollection<TblHallTemplate> TblHallTemplate { set; get; }
    }

    // Many to many
    public class TblHallTemplate {
        public long TblHallId { get; set; }
        public TblHall TblHall { get; set; }

        public long TblTemplateId { get; set; }
        public TblTemplate TblTemplate { get; set; }
    }

    public class TblHallSchedule : KeyEntity {
        public int Year { get; set; }

        public TimeSpan Interval { get; set; }

        //Foreign keys
        [ForeignKey ("TblHallId")]
        public TblHall TblHall { get; set; }

        public long TblHallId { get; set; }
    }
}