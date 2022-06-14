using System;

namespace DbLayer.Interface {

    public interface ICreateEntity {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }

        string PersianCreatedDate { get; }
    }

    public interface IUpdadteEntity : ICreateEntity {
        DateTime? UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
        string PersianUpdatedDate { get; }
    }

    public interface IConverterEntity {
        string ThePersiandDate { get; }
    }
}