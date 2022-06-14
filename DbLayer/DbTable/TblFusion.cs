using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblFusion : KeyEntity {
        public string Title { get; set; }

        public FusionType FusionType { get; set; }

        [NotMapped]
        public string TypeTitle => EnumExtensions.GetDisplayName ((FusionType) FusionType);

        [NotMapped]
        public string CompleteTitle => $"{EnumExtensions.GetDisplayName ((FusionType) FusionType)}-{Title}";

        // Collections
        public ICollection<TblProduct> TblProduct { get; set; }
    }
}