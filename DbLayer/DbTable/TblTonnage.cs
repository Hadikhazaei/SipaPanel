using System;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.DbTable.Base;
using DbLayer.Helper;

namespace DbLayer.DbTable {
    public class TblTonnage : CreateEntity {
        private DateTime _tonnageDate;

        [Column (TypeName = "smalldatetime")]
        public DateTime TonnageDate {
            get => _tonnageDate;
            set {
                _tonnageDate = value;
                ShamsiDate = new ShamsiDateSegment (_tonnageDate);
            }
        }

        public ShamsiDateSegment ShamsiDate { get; set; }

        [NotMapped]
        public string PersianTonnageDate {
            get => TonnageDate.ToShortPersianDateString ();
            set {
                var newValue = DateHelper.AttachTime (value);
                TonnageDate = newValue.ToGregorianDateTimeOrDefault ();
            }
        }

        public int UsedTonnageAsKg { get; set; }

        [NotMapped]
        public string UsedTonnageAsTone => $"{UsedTonnageAsKg / 1000} تن و {UsedTonnageAsKg % 1000} کیلو گرم";

        // Foreign keys
        [ForeignKey ("TblPlanningId")]
        public TblPlanning TblPlanning { get; set; }
        public long TblPlanningId { get; set; }
    }
}