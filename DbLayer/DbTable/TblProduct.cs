using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;

namespace DbLayer.DbTable {
    public class TblProduct : KeyEntity {
        [Required]
        [StringLength (50)]
        public string Title { get; set; }

        [StringLength (50)]
        public string StoreCode { get; set; }

        [StringLength (50)]
        public string TechnicalCode { get; set; }

        // Foreign keys
        [ForeignKey ("TblFusionId")]
        public TblFusion TblFusion { get; set; }
        public long TblFusionId { get; set; }

        // Collections
        public ICollection<JoinTP> JoinTP { get; set; }

        public ICollection<TblQControl> TblQControl { get; set; }
    }
}