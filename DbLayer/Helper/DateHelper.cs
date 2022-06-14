using System;

namespace DbLayer.Helper {
    public static class DateHelper {
        public static string AttachTime (string date) =>
            $"{date} {DateTime.Now.Hour}:{DateTime.Now.Minute}";
    }
}