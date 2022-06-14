using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblDefect : KeyEntity {
        [Required]
        [StringLength (50)]
        public string Title { get; set; }

        public DefectType DefectType { get; set; }

        [NotMapped]
        public string DefectTitle => EnumExtensions.GetDisplayName ((DefectType) DefectType);

        public DefectLineType DefectLineType { get; set; }

        [NotMapped]
        public string DefectLineTitle => EnumExtensions.GetDisplayName ((DefectLineType) DefectLineType);

        // Collections
        public ICollection<TblQControlInfo> TblQControlInfo { set; get; }
    }
}