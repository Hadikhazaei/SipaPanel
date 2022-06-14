using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DbLayer.DbTable.Identity {
    public class AppUser : IdentityUser {
        public string FullName { get; set; }

        [NotMapped]
        public string DisplayName => FullName ?? UserName;

        [ForeignKey ("TblHallId")]
        public TblHall TblHall { get; set; }
        public long? TblHallId { get; set; }

        public ICollection<TblStopInfo> TblStopInfo { get; set; }

        public ICollection<TblPlanning> TblPlanning { get; set; }

        public ICollection<TblProductionInfo> TblProductionInfo { get; set; }
    }
}