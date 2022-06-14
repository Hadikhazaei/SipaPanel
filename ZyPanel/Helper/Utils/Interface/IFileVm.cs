using Microsoft.AspNetCore.Http;

namespace ZyPanel.Helper.Utils.Interface {
    public interface IFileVm {
        string FileUrl { get; set; }

        string ThumbnailsUrl { get; set; }

        IFormFile FormFile { get; set; }

        int WidthImage { get; set; }

        int HeightImage { get; set; }
    }
}