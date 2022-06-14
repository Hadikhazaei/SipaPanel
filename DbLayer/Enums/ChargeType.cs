using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum ChargeType : byte {
        [Display (Name = "پیش فرض")]
        Default = 1,
        // 
        [Display (Name = "پورینگ")]
        Poring = 2,

        [Display (Name = "دستی")]
        Manual = 3
    }
}