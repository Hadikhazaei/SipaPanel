using DbLayer.Interface;

namespace DbLayer.DbTable.Base {
    public class FileEntity : KeyEntity, IFileEntity {
        public string FileUrl { get; set; }

        public string ThumbnailsUrl { get; set; }
    }
}