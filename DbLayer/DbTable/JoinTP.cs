using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.DbTable.Base;

namespace DbLayer.DbTable {
    public class JoinTP : KeyEntity {
        // Helper
        public bool IsReady { get; set; }

        public short Time { get; set; } = 1;

        public short ProductionCycle { get; set; } = 0;

        public short ManualFusionTime { get; set; } = 0;

        public short PoringFusionTime { get; set; } = 0;

        // Weight
        [StringLength (50)]
        public string WeightCode { get; set; }

        [Column (TypeName = "smalldatetime")]
        public DateTime? WeightDate { get; set; }

        [NotMapped]
        public string PersianWeightDate => WeightDate.ToShortPersianDateString ();

        // Product
        public long TblProductId { get; set; }

        public int ProductCount { get; set; }

        [ForeignKey ("TblProductId")]
        public TblProduct TblProduct { get; set; }

        public int ProductWeight { get; set; } = 0;

        [NotMapped]
        public double ProductWeightAsKg => (double) (ProductCount * ProductWeight) / 1000;

        [NotMapped]
        public string ProductDisplayAsKg => ((double) ProductWeight / 1000).ToString ();

        // Template
        public long TblTemplateId { get; set; }

        [ForeignKey ("TblTemplateId")]
        public TblTemplate TblTemplate { get; set; }

        public int TemplateWeight { get; set; } = 0;

        [NotMapped]
        public double TemplateWeightAsKg => (double) TemplateWeight / 1000;

        [NotMapped]
        public string TemplateDisplayAsKg => ((double) TemplateWeight / 1000).ToString ();

        // 
        // Not map
        // 
        [NotMapped]
        public double WholeWeightAsKg => (double) ProductWeightAsKg + TemplateWeightAsKg;
    }
}