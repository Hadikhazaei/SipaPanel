namespace ZyPanel.Helper.Utils {
    public static class FormatHelper {
        public static string GetTimeValue (int value) =>
            value > 60 ? $"{value / 60}h : {value % 60}m" : $"{value}m";

        public static string GetFriendlyTimeValue (int value) =>
            value > 60 ? $"{value / 60}ساعت و {value % 60} دقیقه" : $"{value} دقیقه";

        public static double GetPercentValue (double value, double target) {
            if (target == 0) return 0;
            var result = value * 100 / target;
            return value > target ? -result : result;
        }

        public static string GetPercentFormat (double value) {
            if (value == 0) {
                return $"{value.ToString ("0.##")}%";
            }
            return value > 0 ? $"+{value.ToString ("0.##")}%" : $"{value.ToString ("0.##")}%";
        }

        public static string GetValueAsTwoPoint (this double value) => value.ToString ("0.##");
    }
}