using System;
using System.ComponentModel.DataAnnotations.Schema;

// 
using CldLayer.Persian;
using DbLayer.Interface;

namespace DbLayer.DbTable.Base {
    public abstract class CreateEntity : KeyEntity, ICreateEntity {

        [Column (TypeName = "smalldatetime")]
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        [NotMapped]
        public virtual string PersianCreatedDate => CreatedDate.ToShortPersianDateTimeString ();
    }

    public abstract class UpdateEntity : CreateEntity, IUpdadteEntity {

        [Column (TypeName = "smalldatetime")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }

        [NotMapped]
        public virtual string PersianUpdatedDate => UpdatedDate.ToShortPersianDateTimeString ();
    }

}