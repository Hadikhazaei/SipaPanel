using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum FurnaceType : byte {

        [Display (Name = "کوره 1", Description = "پیش فرض")]
        FurnaceOne = 1,

        [Display (Name = "کوره 3")]
        FurnaceThree = 3,

        [Display (Name = "کوره 4")]
        FurnaceFour = 4,

        [Display (Name = "کوره 5")]
        FurnaceFive = 5,

        [Display (Name = "کوره 6")]
        FurnaceSix = 6,

        [Display (Name = "کوره 7")]
        FurnaceSeven = 7
    }
}