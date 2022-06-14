using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum RoleType : byte {
        [Display (Name = "مدیر")]
        CoRole = 1,

        [Display (Name = "برنامه ریزی")]
        PlanningRole = 2,

        [Display (Name = "مدیر تولید")]
        ProductionManagerRole = 3,

        [Display (Name = "اپراتور تولید")]
        ProductionClerkRole = 4,

        [Display (Name = "کنترل کیفی")]
        QControlRole = 5,
    }

    public enum PolicyType : byte {
        [Display (Name = "مدیر")]
        CoPolicy = 1,

        [Display (Name = "تولید")]
        ProductionManagerPolicy = 3,

        [Display (Name = "اپراتور تولید")]
        ProductionClerkPolicy = 4,

        [Display (Name = "کنترل کیفی")]
        QControlPolicy = 5,
    }
}