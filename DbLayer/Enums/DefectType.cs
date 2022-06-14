using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum DefectType : byte {
        [Display (Name = "مشتری")]
        Customer = 1,
        // 
        [Display (Name = "تولیدی")]
        Production = 2,
        // 
        [Display (Name = "دوباره کاری")]
        Remake = 3,
    }
    public enum DefectLineType : byte {
        [Display (Name = "چدن(غیربلوک)")]
        NonBlok = 1,
        // 
        [Display (Name = "چدن(بلوک)")]
        Blok = 2,

        [Display (Name = "آلومینیوم")]
        Aluminum = 3,

        [Display (Name = "سی ان سی")]
        CNC = 4,
    }
}