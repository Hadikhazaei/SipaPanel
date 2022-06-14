using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.DbTable.Base;

namespace DbLayer.DbTable {
    public class TblStop : KeyEntity {
        [Required]
        [StringLength (50)]
        public string Title { get; set; }

        [Required]
        [StringLength (50)]
        public string Code { get; set; }

        public string Brief { get; set; }

        //Foreign keys
        [ForeignKey ("TblHallId")]
        public TblHall TblHall { get; set; }

        public long TblHallId { get; set; }

        // Collections
        public ICollection<TblStopInfo> TblStopInfo { get; set; }
    }
}