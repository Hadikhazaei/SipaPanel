using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

// 
using CldLayer.Persian;

namespace ZyPanel.Helper.Utils {
    public static class StaticHelper {
        public static string ToFriendlyRoute (this object value) {
            if (value != null) {
                string text = value.ToString ();
                List<char> illegalChars = new List<char> () { ' ', '.', '#', '%', '&', '*', '{', '}', '\\', ':', '<', '>', '?', '/', ';', '@', '=', '+', '$', ',' };
                illegalChars.ForEach (c => {
                    text = text.Replace (c.ToString (), "-");
                });
                return text;
            }
            return null;
        }

        public static bool ContainsNumber (this string inputText) {
            return !string.IsNullOrWhiteSpace (inputText) && inputText.ToEnglishNumbers ().Any (char.IsDigit);
        }

        public static bool HasConsecutiveChars (this string inputText, int sequenceLength = 3) {
            var charEnumerator = StringInfo.GetTextElementEnumerator (inputText);
            var currentElement = string.Empty;
            var count = 1;
            while (charEnumerator.MoveNext ()) {
                if (currentElement == charEnumerator.GetTextElement ()) {
                    if (++count >= sequenceLength) {
                        return true;
                    }
                } else {
                    count = 1;
                    currentElement = charEnumerator.GetTextElement ();
                }
            }
            return false;
        }

        public static bool IsNumeric (this string inputText) {
            if (string.IsNullOrWhiteSpace (inputText)) {
                return false;
            }
            long inputNumber;
            return long.TryParse (inputText.ToEnglishNumbers (), out inputNumber);
        }

        public static bool IsEmailAddress (this string inputText) {
            return !string.IsNullOrWhiteSpace (inputText) && new EmailAddressAttribute ().IsValid (inputText);
        }

        public static Tuple<DateTime, DateTime> GetStartAndEndWeek (DateTime date) {
            if (date.DayOfWeek != DayOfWeek.Saturday) {
                var numberOfWeek = (int) (date.DayOfWeek + 1);
                date = date.AddDays (-numberOfWeek);
            }
            var finishWeek = date.AddDays (6);
            return Tuple.Create<DateTime, DateTime> (date, finishWeek);
        }
    }
}