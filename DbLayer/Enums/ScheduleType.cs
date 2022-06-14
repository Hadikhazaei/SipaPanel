using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum ScheduleType : byte {
        [Display (Name = "تک شیفت")]
        Single = 1,

        // 
        [Display (Name = "دو شیفت")]
        Couple = 2,
    }
}