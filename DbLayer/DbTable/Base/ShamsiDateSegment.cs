using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

// 
using CldLayer.Persian;

namespace DbLayer.DbTable.Base {
    [Owned]
    [ComplexType]
    public class ShamsiDateSegment {
        public ShamsiDateSegment () { }

        public ShamsiDateSegment (DateTime date) {
            this.Year = date.GetPersianYear ();
            this.Month = (byte) date.GetPersianMonth ();
            this.Day = (byte) date.GetPersianDayOfMonth ();
        }

        [Column (Order = 1)]
        public int Year { get; set; }

        [Column (Order = 2)]
        public byte Month { get; set; }

        [Column (Order = 3)]
        public byte Day { get; set; }
    }
}