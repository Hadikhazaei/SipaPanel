using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum KarsetType : byte {
        [Display (Name = "داکتیل")]
        Daktil = 1,

        // 
        [Display (Name = "خاکستری")]
        Gray = 2,

        // 
        [Display (Name = "آلومینیوم")]
        Aluminium = 3,
    }
}