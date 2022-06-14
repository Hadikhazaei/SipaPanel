using System.ComponentModel.DataAnnotations;

namespace DbLayer.Enums {
    public enum StationType : byte {
        [Display (Name = "پیش فرض")]
        Default = 0,
        // 
        [Display (Name = "سالن ادیکارنت")]
        S1 = 1,
        // 
        [Display (Name = "سالن قدیمی")]
        S2 = 2,
        // 
        [Display (Name = "سالن میلنگ")]
        S3 = 3,
        // 
        [Display (Name = "قبل از شات")]
        S4 = 4,
        // 
        [Display (Name = "ابتدای سنگ زنی")]
        S5 = 5,
        // 
        [Display (Name = "بعد از سنگ زنی")]
        S6 = 6,
        // 
        [Display (Name = "ایستگاه رنگ")]
        S7 = 7
    }

    public enum DefectPlaceType : byte {
        [Display (Name = "پیش فرض")]
        Default = 0,
        // 
        [Display (Name = "بدنه تای رو")]
        P1 = 1,
        // 
        [Display (Name = "بدنه تای زیر")]
        P2 = 2,
        // 
        [Display (Name = "فیلتر روغن")]
        P3 = 3,
        // 
        [Display (Name = "پولکی ها")]
        P4 = 4,
        // 
        [Display (Name = "کف کارتر")]
        P5 = 5,
        // 
        [Display (Name = "داخل کارتر")]
        P6 = 6,
        // 
        [Display (Name = "داخل پولکی ها")]
        P7 = 7,
        // 
        [Display (Name = "سرسیلندر")]
        P8 = 8,
        // 
        [Display (Name = "بورها")]
        P9 = 9,
        // 
        [Display (Name = "سینی ها")]
        P10 = 10,
        // 
        [Display (Name = "دسته موتور")]
        P11 = 11,
        // 
        [Display (Name = "پایه تک شاخ")]
        P12 = 12,
        // 
        [Display (Name = "پایه دو شاخ")]
        P13 = 13,
        // 
        [Display (Name = "لبه بورها")]
        P14 = 14,
        // 
        [Display (Name = "سینی دوشاخ")]
        P15 = 15,
        // 
        [Display (Name = "سینی تک شاخ")]
        P16 = 16,
        // 
        [Display (Name = "فشنگی ها")]
        P17 = 17,
        // 
        [Display (Name = "پایه ها")]
        P18 = 18,
        // 
        [Display (Name = "بدنه")]
        P19 = 19,
        // 
        [Display (Name = "کیه 1")]
        P20 = 20,
        // 
        [Display (Name = "تای زیر دیواره ی اویل چنل")]
        P21 = 21,
        // 
        [Display (Name = "محل شماره تولید")]
        P22 = 22,
        // 
        [Display (Name = "تای رو پولکی ها")]
        P23 = 23,
        // 
        [Display (Name = "تایل زیر پولکی اویل چنل")]
        P24 = 24,
        // 
        [Display (Name = "واتر جکت")]
        P25 = 25,
        // 
        [Display (Name = "واتر پمپ")]
        P26 = 26,
        // 
        [Display (Name = "تای زیر اویل چنل")]
        P27 = 27,
        // 
        [Display (Name = "شش وجه بلوک")]
        P28 = 28,
        // 
        [Display (Name = "دیواره ی بورها")]
        P29 = 29,
        // 
        [Display (Name = "اویل چنل")]
        P30 = 30,
    }
}