using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblPlanningInfo : CreateEntity {
        public short Order { get; set; } = 1;

        public int TemplateCount { get; set; }

        [StringLength (50)]
        public string WeightCode { get; set; }

        public UnitType UnitType { get; set; } = UnitType.Default;

        [NotMapped]
        public string UnitTitle => EnumExtensions.GetDisplayName ((UnitType) UnitType);

        public string Explanation { get; set; }

        // Foreign keys
        [ForeignKey ("TblPlanningId")]
        public TblPlanning TblPlanning { get; set; }
        public long TblPlanningId { get; set; }

        [ForeignKey ("TblTemplateId")]
        public TblTemplate TblTemplate { get; set; }
        public long TblTemplateId { get; set; }
    }
}