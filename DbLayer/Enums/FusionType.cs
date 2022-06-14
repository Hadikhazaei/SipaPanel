using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum FusionType : byte {
        [Display (Name = "خاکستری")]
        Gray = 1,

        [Display (Name = "نشکن")]
        Nashkan = 2,

        [Display (Name = "آلومینیوم ریزی")]
        Aluminium = 3
    }
}