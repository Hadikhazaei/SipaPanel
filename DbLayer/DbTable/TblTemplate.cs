using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// 
using DbLayer.DbTable.Base;

namespace DbLayer.DbTable {
    public class TblTemplate : KeyEntity {
        [Required]
        [StringLength (50)]
        public string Code { get; set; }

        public byte KaviteCount { get; set; }

        public bool IsActive { get; set; } = true;

        // Collections
        public ICollection<JoinTP> JoinTP { get; set; }

        public ICollection<TblStopInfo> TblStopInfo { get; set; }

        public ICollection<TblPlanningInfo> TblPlanningInfo { get; set; }

        public ICollection<TblProductionInfo> TblProductionInfo { get; set; }

        // Many to many
        public ICollection<TblHallTemplate> TblHallTemplate { set; get; }
    }
}