using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 
using DbLayer.Interface;

namespace DbLayer.DbTable.Base {
    public abstract class KeyEntity : IKeyEntity {
        [Display (Name = "Id", Order = 0)]
        [Key, DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }
    }

    public abstract class KeyEntity<Key> : IKeyEntity<Key> {
        [Display (Name = "Id", Order = 0)]
        [Key, DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public virtual Key Id { get; set; }
    }

    // public abstract class DeleteEntity : EntityBase, IDeleteEntity {
    //     public bool IsDeleted { get; set; }
    // }
}