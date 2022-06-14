using System;

namespace DbLayer.Interface {
    public interface IKeyEntity {
        long Id { get; set; }
    }

    public interface IKeyEntity<IKey> {
        IKey Id { get; set; }
    }
}