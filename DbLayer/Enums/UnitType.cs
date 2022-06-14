using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum UnitType : byte {
        [Display (Name = "پیش فرض", Description = "پیش فرض")]
        Default = 1,

        [Display (Name = "Kg", Description = "کیلوگرم")]
        KiloGram = 2,

        // 
        [Display (Name = "قالب", Description = "قالب")]
        Template = 3,
    }
}