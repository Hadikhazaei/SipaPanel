using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum HallType : byte {
        [Display (Name = "پیش فرض")]
        Default = 1,

        // 
        [Display (Name = "چدن")]
        CastIron = 2,

        // 
        [Display (Name = "آلومینیوم")]
        Aluminium = 3,

        // 
        [Display (Name = "CNC")]
        CNC = 4,
    }
}