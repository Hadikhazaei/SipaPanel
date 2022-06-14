using System;
using System.Globalization;
using System.Linq;

// References
using CldLayer.Gregorian;

namespace CldLayer.Persian {
    #region ### Utilities ###
    /// <summary>
    /// کلاسی برای محاسبات تاریخ شمیسی
    /// </summary>
    public static class PersianDateUtilities {
        #region :: ToLongPersianDateString ::
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToLongPersianDateString (this DateTime dt) {
            return dt.ToPersianDateTimeString (PersianCulture.Instance.DateTimeFormat.LongDatePattern);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToLongPersianDateString (this DateTime? dt) {
            return dt == null ? string.Empty : ToLongPersianDateString (dt.Value);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static string ToLongPersianDateString (this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return dt == null ? string.Empty : ToLongPersianDateString (dt.Value.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static string ToLongPersianDateString (this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return ToLongPersianDateString (dt.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }
        #endregion

        #region :: ToLongPersianDateTimeString ::
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395، 10:20:02 ق.ظ
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToLongPersianDateTimeString (this DateTime dt) {
            return dt.ToPersianDateTimeString (
                $"{PersianCulture.Instance.DateTimeFormat.LongDatePattern}، {PersianCulture.Instance.DateTimeFormat.LongTimePattern}");
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395، 10:20:02 ق.ظ
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToLongPersianDateTimeString (this DateTime? dt) {
            return dt == null ? string.Empty : ToLongPersianDateTimeString (dt.Value);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395، 10:20:02 ق.ظ
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static string ToLongPersianDateTimeString (this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return dt == null ? string.Empty : ToLongPersianDateTimeString (dt.Value.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 21 دی 1395، 10:20:02 ق.ظ
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static string ToLongPersianDateTimeString (this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return ToLongPersianDateTimeString (dt.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }
        #endregion

        #region :: ToShortPersianDateString ::
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21
        /// </summary>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateString (this DateTimeOffset? dt,
            DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return dt == null ? string.Empty : ToShortPersianDateString (dt.Value.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21
        /// </summary>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateString (this DateTimeOffset dt,
            DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return ToShortPersianDateString (dt.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateString (this DateTime dt) {
            return dt.ToPersianDateTimeString (PersianCulture.Instance.DateTimeFormat.ShortDatePattern);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateString (this DateTime? dt) {
            return dt == null ? string.Empty : ToShortPersianDateString (dt.Value);
        }
        #endregion

        #region :: ToShortPersianDateTimeString ::
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21 10:20
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateTimeString (this DateTime dt) {
            return dt.ToPersianDateTimeString (
                $"{PersianCulture.Instance.DateTimeFormat.ShortDatePattern} {PersianCulture.Instance.DateTimeFormat.ShortTimePattern}");
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21 10:20
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateTimeString (this DateTime? dt) {
            return dt == null ? string.Empty : ToShortPersianDateTimeString (dt.Value);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21 10:20
        /// </summary>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateTimeString (this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return dt == null ? string.Empty : ToShortPersianDateTimeString (dt.Value.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// با قالبی مانند 1395/10/21 10:20
        /// </summary>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToShortPersianDateTimeString (this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return ToShortPersianDateTimeString (dt.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }
        #endregion

        #region :: ToGregorianDateTime ::
        /// <summary>
        /// تبدیل تاریخ و زمان رشته‌ای شمسی به میلادی
        /// با قالب‌های پشتیبانی شده‌ی ۹۰/۸/۱۴ , 1395/11/3 17:30 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴
        /// </summary>
        /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
        /// <returns>تاریخ و زمان میلادی</returns>
        public static DateTime? ToGregorianDateTime (this string persianDateTime) {
            if (string.IsNullOrWhiteSpace (persianDateTime)) {
                return null;
            }
            persianDateTime = persianDateTime.Trim ().ToEnglishNumbers ();
            var splitedDateTime = persianDateTime.Split (new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var rawTime = splitedDateTime.FirstOrDefault (s => s.Contains (':'));
            var rawDate = splitedDateTime.FirstOrDefault (s => !s.Contains (':'));

            var splitedDate = rawDate?.Split ('/', ',', '؍', '.', '-');
            if (splitedDate?.Length != 3) {
                return null;
            }

            var day = GetDay (splitedDate[2]);
            if (!day.HasValue) {
                return null;
            }

            var month = GetMonth (splitedDate[1]);
            if (!month.HasValue) {
                return null;
            }

            var year = GetYear (splitedDate[0]);
            if (!year.HasValue) {
                return null;
            }

            if (!IsValidPersianDate (year.Value, month.Value, day.Value)) {
                return null;
            }

            var hour = 0;
            var minute = 0;
            var second = 0;

            if (!string.IsNullOrWhiteSpace (rawTime)) {
                var splitedTime = rawTime.Split (new [] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                hour = int.Parse (splitedTime[0]);
                minute = int.Parse (splitedTime[1]);
                if (splitedTime.Length > 2) {
                    var lastPart = splitedTime[2].Trim ();
                    var formatInfo = PersianCulture.Instance.DateTimeFormat;
                    if (lastPart.Equals (formatInfo.PMDesignator, StringComparison.OrdinalIgnoreCase)) {
                        if (hour < 12) {
                            hour += 12;
                        }
                    } else {
                        int.TryParse (lastPart, out second);
                    }
                }
            }

            var persianCalendar = new PersianCalendar ();
            return persianCalendar.ToDateTime (year.Value, month.Value, day.Value, hour, minute, second, 0);
        }

        /// <summary>
        /// تبدیل تاریخ و زمان رشته‌ای شمسی به میلادی
        /// با قالب‌های پشتیبانی شده‌ی ۹۰/۸/۱۴ , 1395/11/3 17:30 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴
        /// </summary>
        /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
        /// <returns>تاریخ و زمان میلادی</returns>
        public static DateTime ToGregorianDateTimeOrDefault (this string persianDateTime) {
            var dateTime = ToGregorianDateTime (persianDateTime);
            return dateTime.HasValue ? dateTime.Value : DateTime.Now;
        }

        /// <summary>
        /// تبدیل تاریخ و زمان رشته‌ای شمسی به میلادی
        /// با قالب‌های پشتیبانی شده‌ی ۹۰/۸/۱۴ , 1395/11/3 17:30 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴
        /// </summary>
        /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
        /// <returns>تاریخ و زمان میلادی</returns>
        public static DateTimeOffset? ToGregorianDateTimeOffset (this string persianDateTime) {
            var dateTime = persianDateTime.ToGregorianDateTime ();
            if (dateTime == null) {
                return null;
            }
            return new DateTimeOffset (dateTime.Value, GregorianDateUtilities.IranStandardTime.BaseUtcOffset);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        public static string ToPersianDateTimeString (this DateTime dateTime, string format) {
            return dateTime.ToString (format, PersianCulture.Instance);
        }
        #endregion

        #region :: Etc ::
        /// <summary>
        /// تعیین اعتبار تاریخ شمسی
        /// </summary>
        /// <param name="persianYear">سال شمسی</param>
        /// <param name="persianMonth">ماه شمسی</param>
        /// <param name="persianDay">روز شمسی</param>
        public static bool IsValidPersianDate (int persianYear, int persianMonth, int persianDay) {
            if (persianDay > 31 || persianDay <= 0) {
                return false;
            }

            if (persianMonth > 12 || persianMonth <= 0) {
                return false;
            }

            if (persianMonth <= 6 && persianDay > 31) {
                return false;
            }

            if (persianMonth >= 7 && persianDay > 30) {
                return false;
            }

            if (persianMonth == 12) {
                var persianCalendar = new PersianCalendar ();
                var isLeapYear = persianCalendar.IsLeapYear (persianYear);

                if (isLeapYear && persianDay > 30) {
                    return false;
                }

                if (!isLeapYear && persianDay > 29) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// تعیین اعتبار تاریخ و زمان رشته‌ای شمسی
        /// با قالب‌های پشتیبانی شده‌ی ۹۰/۸/۱۴ , 1395/11/3 17:30 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴
        /// </summary>
        /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
        public static bool IsValidPersianDateTime (this string persianDateTime) {
            try {
                var dt = persianDateTime.ToGregorianDateTime ();
                return dt.HasValue;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه‌ی حاصل‌
        /// </summary>
        /// <param name="gregorianDate">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static PersianDay ToPersianYearMonthDay (this DateTimeOffset? gregorianDate, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return gregorianDate == null ?
                throw new ArgumentNullException (nameof (gregorianDate)) : ToPersianYearMonthDay (gregorianDate.Value.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه‌ی حاصل‌
        /// </summary>
        public static PersianDay ToPersianYearMonthDay (this DateTime? gregorianDate) {
            return gregorianDate == null ?
                throw new ArgumentNullException (nameof (gregorianDate)) : ToPersianYearMonthDay (gregorianDate.Value);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه‌ی حاصل‌
        /// </summary>
        /// <param name="gregorianDate">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static PersianDay ToPersianYearMonthDay (this DateTimeOffset gregorianDate,
            DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return ToPersianYearMonthDay (gregorianDate.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه‌ی حاصل‌
        /// </summary>
        public static PersianDay ToPersianYearMonthDay (this DateTime gregorianDate) {
            var persianCalendar = new PersianCalendar ();
            var persianYear = persianCalendar.GetYear (gregorianDate);
            var persianMonth = persianCalendar.GetMonth (gregorianDate);
            var persianDay = persianCalendar.GetDayOfMonth (gregorianDate);
            return new PersianDay { Year = persianYear, Month = persianMonth, Day = persianDay };
        }

        private static int? GetDay (string part) {
            var day = part.ToNumber ();
            if (!day.Item1) return null;
            var pDay = day.Item2;
            if (pDay == 0 || pDay > 31) return null;
            return pDay;
        }

        private static int? GetMonth (string part) {
            var month = part.ToNumber ();
            if (!month.Item1) return null;
            var pMonth = month.Item2;
            if (pMonth == 0 || pMonth > 12) return null;
            return pMonth;
        }

        private static int? GetYear (string part) {
            var year = part.ToNumber ();
            if (!year.Item1) return null;
            var pYear = year.Item2;
            if (part.Length == 2) pYear += 1300;
            return pYear;
        }

        private static Tuple<bool, int> ToNumber (this string data) {
            int number;
            bool result = int.TryParse (data, out number);
            return new Tuple<bool, int> (result, number);
        }
        #endregion
    }
    #endregion

    #region ### Friendly ###
    /// <summary>
    /// نمایش دوستانه‌ی تاریخ و ساعت انگلیسی به شمسی
    /// </summary>
    public static class PersianFriendly {
        #region :: ToPersianDateTextify ::
        /// <summary>
        /// نمایش فارسی روز دریافتی
        /// مانند سه شنبه ۲۱ دی ۱۳۹۵
        /// </summary>
        public static string ToPersianDateTextify (this DateTime dt) {
            var dateParts = dt.ToPersianYearMonthDay ();
            return ToPersianDateTextify (dateParts.Year, dateParts.Month, dateParts.Day);
        }

        /// <summary>
        /// نمایش فارسی روز دریافتی
        /// مانند سه شنبه ۲۱ دی ۱۳۹۵
        /// </summary>
        public static string ToPersianDateTextify (this DateTime? dt) {
            return dt == null ? null : ToPersianDateTextify (dt.Value);
        }

        /// <summary>
        /// نمایش فارسی روز دریافتی شمسی
        /// مانند سه شنبه ۲۱ دی ۱۳۹۵
        /// </summary>
        public static string ToPersianDateTextify (int persianYear, int persianMonth, int persianDay) {
            if (persianYear <= 99) {
                persianYear += 1300;
            }

            var strDay = PersianCulture.GetPersianWeekDayName (persianYear, persianMonth, persianDay);
            var strMonth = PersianCulture.PersianMonthNames[persianMonth];
            return $"{strDay} {persianDay} {strMonth} {persianYear}".ToPersianNumbers ();
        }

        /// <summary>
        /// نمایش فارسی روز دریافتی
        /// مانند سه شنبه ۲۱ دی ۱۳۹۵
        /// </summary>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static string ToPersianDateTextify (this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return ToPersianDateTextify (dt.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }

        /// <summary>
        /// نمایش فارسی روز دریافتی
        /// مانند سه شنبه ۲۱ دی ۱۳۹۵
        /// </summary>
        /// <param name="dt">تاریخ و زمان</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        public static string ToPersianDateTextify (this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return dt == null ? string.Empty : ToPersianDateTextify (dt.Value.GetDateTimeOffsetPart (dateTimeOffsetPart));
        }
        #endregion

        #region :: FriendlyPersianDateTextify ::
        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مبنای محاسبه هم اکنون
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTimeOffset? dt) {
            if (dt == null) {
                return string.Empty;
            }
            var comparisonBase = DateTime.UtcNow.ToIranTimeZoneDateTime ();
            var iranLocalTime = dt.Value.GetDateTimeOffsetPart (DateTimeOffsetPart.IranLocalDateTime);
            return ToFriendlyPersianDateTextify (iranLocalTime, comparisonBase);
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <param name="comparisonBase">مبنای مقایسه مانند هم اکنون</param>
        /// <param name="appendHhMm">آیا ساعت نیز به نتیجه‌اضافه شود؟</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTime dt, DateTime comparisonBase, bool appendHhMm = true) {
            return $"{UnicodeConstants.RleChar}{ToFriendlyPersianDate(dt, comparisonBase, appendHhMm).ToPersianNumbers()}";
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مبنای محاسبه هم اکنون
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <param name="appendHhMm">آیا ساعت نیز به نتیجه‌اضافه شود؟</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTime dt, bool appendHhMm = true) {
            var comparisonBase = dt.Kind.GetNow ().ToIranTimeZoneDateTime ();
            return $"{UnicodeConstants.RleChar}{ToFriendlyPersianDate(dt.ToIranTimeZoneDateTime(), comparisonBase, appendHhMm).ToPersianNumbers()}";
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <param name="comparisonBase">مبنای مقایسه مانند هم اکنون</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        /// <param name="appendHhMm">آیا ساعت نیز به نتیجه‌اضافه شود؟</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTimeOffset dt, DateTime comparisonBase,
            DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime, bool appendHhMm = true) {
            return $"{UnicodeConstants.RleChar}{ToFriendlyPersianDate(dt.GetDateTimeOffsetPart(dateTimeOffsetPart), comparisonBase, appendHhMm).ToPersianNumbers()}";
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مبنای محاسبه هم اکنون
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <param name="appendHhMm">آیا ساعت نیز به نتیجه‌اضافه شود؟</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTimeOffset dt, bool appendHhMm = true) {
            var comparisonBase = DateTime.UtcNow.ToIranTimeZoneDateTime ();
            var iranLocalTime = dt.GetDateTimeOffsetPart (DateTimeOffsetPart.IranLocalDateTime);
            return $"{UnicodeConstants.RleChar}{ToFriendlyPersianDate(iranLocalTime, comparisonBase, appendHhMm).ToPersianNumbers()}";
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <param name="comparisonBase">مبنای مقایسه مانند هم اکنون</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTime? dt, DateTime comparisonBase) {
            return dt == null ? string.Empty : ToFriendlyPersianDateTextify (dt.Value, comparisonBase);
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مبنای محاسبه هم اکنون
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTime? dt) {
            if (dt == null) {
                return string.Empty;
            }
            var comparisonBase = dt.Value.Kind.GetNow ().ToIranTimeZoneDateTime ();
            return ToFriendlyPersianDateTextify (dt.Value.ToIranTimeZoneDateTime (), comparisonBase);
        }

        /// <summary>
        /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
        /// مانند ۱۰ روز قبل، سه شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
        /// </summary>
        /// <param name="dt">تاریخ ورودی</param>
        /// <param name="comparisonBase">مبنای مقایسه مانند هم اکنون</param>
        /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
        /// <returns>نمایش دوستانه</returns>
        public static string ToFriendlyPersianDateTextify (this DateTimeOffset? dt, DateTime comparisonBase,
            DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) {
            return dt == null ? string.Empty : ToFriendlyPersianDateTextify (dt.Value.GetDateTimeOffsetPart (dateTimeOffsetPart), comparisonBase);
        }
        #endregion

        private static string ToFriendlyPersianDate (this DateTime dt, DateTime comparisonBase, bool appendHhMm) {
            var persianDate = dt.ToPersianYearMonthDay ();

            //1388/10/22
            var persianYear = persianDate.Year;
            var persianMonth = persianDate.Month;
            var persianDay = persianDate.Day;

            //13:14
            var hour = dt.Hour;
            var min = dt.Minute;
            var hhMm = $"{hour.ToString("00", CultureInfo.InvariantCulture)}:{min.ToString("00", CultureInfo.InvariantCulture)}";

            var date = new PersianCalendar ().ToDateTime (persianYear, persianMonth, persianDay, hour, min, 0, 0);
            var diff = date - comparisonBase;
            var totalSeconds = Math.Round (diff.TotalSeconds);
            var totalDays = Math.Round (diff.TotalDays);

            var suffix = " بعد";
            if (totalSeconds < 0) {
                suffix = " قبل";
                totalSeconds = Math.Abs (totalSeconds);
                totalDays = Math.Abs (totalDays);
            }

            var dateTimeToday = DateTime.Today;
            var yesterday = dateTimeToday.AddDays (-1);
            var today = dateTimeToday.Date;
            var tomorrow = dateTimeToday.AddDays (1);

            hhMm = appendHhMm ? $"، ساعت {hhMm}" : string.Empty;

            if (today == date.Date) {
                // Less than one minute ago.
                if (totalSeconds < 60) {
                    return "هم اکنون";
                }

                // Less than 2 minutes ago.
                if (totalSeconds < 120) {
                    return $"یک دقیقه{suffix}{hhMm}";
                }

                // Less than one hour ago.
                if (totalSeconds < 3600) {
                    return string.Format (CultureInfo.InvariantCulture, "{0} دقیقه",
                        ((int) Math.Floor (totalSeconds / 60))) + suffix + hhMm;
                }

                // Less than 2 hours ago.
                if (totalSeconds < 7200) {
                    return $"یک ساعت{suffix}{hhMm}";
                }

                // Less than one day ago.
                if (totalSeconds < 86400) {
                    return
                    string.Format (
                        CultureInfo.InvariantCulture,
                        "{0} ساعت",
                        ((int) Math.Floor (totalSeconds / 3600))
                    ) + suffix + hhMm;
                }
            }

            if (yesterday == date.Date) {
                return $"دیروز {PersianCulture.GetPersianWeekDayName(persianYear, persianMonth, persianDay)}{hhMm}";
            }

            if (tomorrow == date.Date) {
                return $"فردا {PersianCulture.GetPersianWeekDayName(persianYear, persianMonth, persianDay)}{hhMm}";
            }

            var dayStr = $"، {ToPersianDateTextify(persianYear, persianMonth, persianDay)}{hhMm}";

            if (totalSeconds < 30 * TimeConstants.Day) {
                return $"{(int)Math.Abs(totalDays)} روز{suffix}{dayStr}";
            }

            if (totalSeconds < 12 * TimeConstants.Month) {
                int months = Convert.ToInt32 (Math.Floor ((double) Math.Abs (diff.Days) / 30));
                return months <= 1 ? $"1 ماه{suffix}{dayStr}" : $"{months} ماه{suffix}{dayStr}";
            }

            var years = Convert.ToInt32 (Math.Floor ((double) Math.Abs (diff.Days) / 365));
            var daysMonths = (double) Math.Abs (diff.Days) / 30;
            var nextMonths = Convert.ToInt32 (Math.Truncate (daysMonths)) - (years * 12) - 1;
            var nextMonthsStr = nextMonths <= 0 ? "" : $"{(years >= 1 ? " و " : "")}{nextMonths} ماه";

            if (years < 1) {
                return $"{nextMonthsStr}{suffix}{dayStr}";
            }

            return $"{years} سال{nextMonthsStr}{suffix}{dayStr}";
        }
    }
    #endregion
}