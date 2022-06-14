using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum StopType : byte {
        [Display (Name = "فنی(چدن)")]
        TechCI = 1,
        // 
        [Display (Name = "غیرفنی(چدن)")]
        NonTechCI = 2,
        // 
        [Display (Name = "تولیدی")]
        ProduceAL = 3,
        // 
        [Display (Name = "تدارکاتی")]
        SupplyAL = 4,
        // 
        [Display (Name = "قالب سازی")]
        MakeTemplateAl = 5,
        // 
        [Display (Name = "آماده سازی")]
        PreparingAl = 6,
        // 
        [Display (Name = "فنی(آلومینیوم)")]
        TechAl = 7,
    }
}