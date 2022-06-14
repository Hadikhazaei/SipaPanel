using System;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.DbTable.Base;
using DbLayer.Enums;
using DbLayer.Helper;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblKarset : KeyEntity {

        private DateTime _createDate;

        [Column (TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [NotMapped]
        public string PersianCreatDate {
            get => CreateDate.ToShortPersianDateString ();
            set {
                CreateDate = value.ToGregorianDateTimeOrDefault ();
            }
        }

        public int ProductTonnage { get; set; } = 0;

        [NotMapped]
        public double ProductTonnageAsTone => (double) ProductTonnage / 1000;

        public int PlanningTonnage { get; set; } = 0;

        [NotMapped]
        public double PlanningTonnageAsTone => (double) PlanningTonnage / 1000;

        [NotMapped]
        public double Available => PlanningTonnage > 0 ? ProductTonnage * 100 / PlanningTonnage : 0;

        public KarsetType KarsetType { get; set; }

        [NotMapped]
        public string KarsetTitle => EnumExtensions.GetDisplayName ((KarsetType) KarsetType);
    }
}