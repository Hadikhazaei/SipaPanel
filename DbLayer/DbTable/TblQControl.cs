using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.DbTable.Base;
using DbLayer.Enums;
using HpLayer.Extensions;

namespace DbLayer.DbTable {
    public class TblQControl : CreateEntity {
        /// <summary>
        /// تعداد ضایع
        /// </summary>
        public int WasteCount { get; set; }

        /// <summary>
        /// تعداد مرجوعی
        /// </summary>
        public int BackCount { get; set; } = 0;

        [NotMapped]
        public string BackTitle => BackCount == 0 ? "تولیدی" : "مشتری";

        [Column (TypeName = "smalldatetime")]
        public DateTime ProductionDate { get; set; }

        [NotMapped]
        public string PersianProductionDate => ProductionDate.ToShortPersianDateString ();

        /// <summary>
        /// بازرس
        /// </summary>
        [StringLength (50)]
        public string Inspecter { get; set; }

        /// <summary>
        /// ایستگاه بازرسی
        /// </summary>
        public StationType StationType { get; set; }

        [NotMapped]
        public string StationTitle => EnumExtensions.GetDisplayName ((StationType) StationType);

        /// <summary>
        /// شرح
        /// </summary>
        public string Explanation { get; set; }

        // Foreign keys
        [ForeignKey (nameof (TblProductId))]
        public TblProduct TblProduct { get; set; }
        public long TblProductId { get; set; }

        [ForeignKey (nameof (TblPlanningId))]
        public TblPlanning TblPlanning { get; set; }
        public long TblPlanningId { get; set; }

        // Collections
        public ICollection<TblQControlInfo> TblQControlInfo { get; set; }
    }

    public class TblQControlInfo : CreateEntity {
        public bool IsWaste { get; set; }

        /// <summary>
        /// کد ردیابی
        /// </summary>
        [StringLength (50)]
        public string TrackCode { get; set; }

        /// <summary>
        /// نوع محفظه
        /// </summary>
        [StringLength (50)]
        public string ShieldType { get; set; }

        /// <summary>
        /// محل عیب
        /// </summary>
        public DefectPlaceType DefectPlaceType { get; set; }

        [NotMapped]
        public string DefectPlaceTitle =>
            EnumExtensions.GetDisplayName ((DefectPlaceType) DefectPlaceType);

        public string Explanation { get; set; }

        // Foreign keys
        [ForeignKey (nameof (TblDefectId))]
        public TblDefect TblDefect { get; set; }
        public long TblDefectId { get; set; }

        [ForeignKey (nameof (TblQControlId))]
        public TblQControl TblQControl { get; set; }
        public long TblQControlId { get; set; }

        public override string PersianCreatedDate => CreatedDate.ToShortPersianDateTimeString ();
    }
}