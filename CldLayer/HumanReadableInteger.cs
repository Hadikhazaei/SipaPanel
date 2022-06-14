﻿using System.Collections.Generic;
using System.Linq;

namespace CldLayer {
	/// <summary>
	/// Number to word languages
	/// </summary>
	public enum Language {
		/// <summary>
		/// English Language
		/// </summary>
		English,

		/// <summary>
		/// Persian Language
		/// </summary>
		Persian
	}

	/// <summary>
	/// Digit's groups
	/// </summary>
	public enum DigitGroup {
		/// <summary>
		/// Ones group
		/// </summary>
		Ones,

		/// <summary>
		/// Teens group
		/// </summary>
		Teens,

		/// <summary>
		/// Tens group
		/// </summary>
		Tens,

		/// <summary>
		/// Hundreds group
		/// </summary>
		Hundreds,

		/// <summary>
		/// Thousands group
		/// </summary>
		Thousands
	}

	/// <summary>
	/// Equivalent names of a group
	/// </summary>
	public class NumberWord {
		/// <summary>
		/// Digit's group
		/// </summary>
		public DigitGroup Group { set; get; }

		/// <summary>
		/// Number to word language
		/// </summary>
		public Language Language { set; get; }

		/// <summary>
		/// Equivalent names
		/// </summary>
		public IList<string> Names { set; get; }
	}

	/// <summary>
	/// Convert a number into words
	/// </summary>
	public static class HumanReadableInteger {
		private static readonly IDictionary<Language, string> _and = new Dictionary<Language, string> { { Language.English, " " },
			{ Language.Persian, " و " }
		};
		private static readonly IList<NumberWord> _numberWords = new List<NumberWord> {
			new NumberWord {
				Group = DigitGroup.Ones, Language = Language.English, Names =
					new List<string> { string.Empty, "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" }
			},
			new NumberWord {
				Group = DigitGroup.Ones, Language = Language.Persian, Names =
					new List<string> { string.Empty, "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" }
			},

			new NumberWord {
				Group = DigitGroup.Teens, Language = Language.English, Names =
					new List<string> { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" }
			},
			new NumberWord {
				Group = DigitGroup.Teens, Language = Language.Persian, Names =
					new List<string> { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" }
			},

			new NumberWord {
				Group = DigitGroup.Tens, Language = Language.English, Names =
					new List<string> { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" }
			},
			new NumberWord {
				Group = DigitGroup.Tens, Language = Language.Persian, Names =
					new List<string> { "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" }
			},

			new NumberWord {
				Group = DigitGroup.Hundreds, Language = Language.English, Names =
					new List<string> {
						string.Empty,
						"One Hundred",
						"Two Hundred",
						"Three Hundred",
						"Four Hundred",
						"Five Hundred",
						"Six Hundred",
						"Seven Hundred",
						"Eight Hundred",
						"Nine Hundred"
					}
			},
			new NumberWord {
				Group = DigitGroup.Hundreds, Language = Language.Persian, Names =
					new List<string> { string.Empty, "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" }
			},

			new NumberWord {
				Group = DigitGroup.Thousands, Language = Language.English, Names =
					new List<string> {
						string.Empty,
						" Thousand",
						" Million",
						" Billion",
						" Trillion",
						" Quadrillion",
						" Quintillion",
						" Sextillian",
						" Septillion",
						" Octillion",
						" Nonillion",
						" Decillion",
						" Undecillion",
						" Duodecillion",
						" Tredecillion",
						" Quattuordecillion",
						" Quindecillion",
						" Sexdecillion",
						" Septendecillion",
						" Octodecillion",
						" Novemdecillion",
						" Vigintillion",
						" Unvigintillion",
						" Duovigintillion",
						" 10^72",
						" 10^75",
						" 10^78",
						" 10^81",
						" 10^84",
						" 10^87",
						" Vigintinonillion",
						" 10^93",
						" 10^96",
						" Duotrigintillion",
						" Trestrigintillion"
					}
			},
			new NumberWord {
				Group = DigitGroup.Thousands, Language = Language.Persian, Names =
					new List<string> {
						string.Empty,
						" هزار",
						" میلیون",
						" میلیارد",
						" تریلیون",
						" Quadrillion",
						" Quintillion",
						" Sextillian",
						" Septillion",
						" Octillion",
						" Nonillion",
						" Decillion",
						" Undecillion",
						" Duodecillion",
						" Tredecillion",
						" Quattuordecillion",
						" Quindecillion",
						" Sexdecillion",
						" Septendecillion",
						" Octodecillion",
						" Novemdecillion",
						" Vigintillion",
						" Unvigintillion",
						" Duovigintillion",
						" 10^72",
						" 10^75",
						" 10^78",
						" 10^81",
						" 10^84",
						" 10^87",
						" Vigintinonillion",
						" 10^93",
						" 10^96",
						" Duotrigintillion",
						" Trestrigintillion"
					}
			},
		};
		private static readonly IDictionary<Language, string> _negative = new Dictionary<Language, string> { { Language.English, "Negative " },
			{ Language.Persian, "منهای " }
		};
		private static readonly IDictionary<Language, string> _zero = new Dictionary<Language, string> { { Language.English, "Zero" },
			{ Language.Persian, "صفر" }
		};

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public static string NumberToText (this int number, Language language) {
			return NumberToText ((long) number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public static string NumberToText (this uint number, Language language) {
			return NumberToText ((long) number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public static string NumberToText (this byte number, Language language) {
			return NumberToText ((long) number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public static string NumberToText (this decimal number, Language language) {
			return NumberToText ((long) number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public static string NumberToText (this double number, Language language) {
			return NumberToText ((long) number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public static string NumberToText (this long number, Language language) {
			if (number == 0) {
				return _zero[language];
			}

			if (number < 0) {
				return _negative[language] + NumberToText (-number, language);
			}

			return Wordify (number, language, string.Empty, 0);
		}

		private static string GetName (int idx, Language language, DigitGroup group) {
			return _numberWords.First (x => x.Group == group && x.Language == language).Names[idx];
		}

		private static string Wordify (long number, Language language, string leftDigitsText, int thousands) {
			if (number == 0) {
				return leftDigitsText;
			}

			var wordValue = leftDigitsText;
			if (wordValue.Length > 0) {
				wordValue += _and[language];
			}

			if (number < 10) {
				wordValue += GetName ((int) number, language, DigitGroup.Ones);
			} else if (number < 20) {
				wordValue += GetName ((int) (number - 10), language, DigitGroup.Teens);
			} else if (number < 100) {
				wordValue += Wordify (number % 10, language, GetName ((int) (number / 10 - 2), language, DigitGroup.Tens), 0);
			} else if (number < 1000) {
				wordValue += Wordify (number % 100, language, GetName ((int) (number / 100), language, DigitGroup.Hundreds), 0);
			} else {
				wordValue += Wordify (number % 1000, language, Wordify (number / 1000, language, string.Empty, thousands + 1), 0);
			}

			if (number % 1000 == 0) return wordValue;
			return wordValue + GetName (thousands, language, DigitGroup.Thousands);
		}
	}
}