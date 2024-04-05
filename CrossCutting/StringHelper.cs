using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Domain.Enums;
using Domain.Exceptions;

namespace CrossCutting
{
    public static partial class StringHelper
    {
        public static string ToSize(this string s)
        {
            return s switch
            {
                "cover" => "coversmall",
                "screenshot" => "screenshot_huge",
                "thumb" => "thumb",
                "logo" => "logo_med",
                _ => "invalid"
            };
        }

        public static string ToQueryParam(this string s)
        {
            return s switch
            {
                "search" => "search",
                "genre" => "where genres.name ~",
                "keyword" => "where keywords.name ~",
                "companie" => "where involved_companies.company.name ~",
                "language" => "where language_supports.language ~",
                "theme" => "where themes.name ~",
                "releaseyear" => "where first_release_date ~",
                "limit" => "limit",
                _ => $"invalid: {s}"
            };
        }

        public static string BuildSearchQuery(this Dictionary<string, string> querys, int limit)
        {
            var queryOut = String.Format($"fields name, first_release_date, cover.image_id; limit {limit}; ");

            if (querys.Count == 0) { return queryOut; }

            querys.ToList<KeyValuePair<string, string>>().ForEach(param =>
            {
                if (param.Key.ToQueryParam().StartsWith("invalid:"))
                    throw new Exception("Invalid query param.");

                if (!param.Key.ToQueryParam().StartsWith("limit"))
                    queryOut += $"{param.Key.ToQueryParam()} \"{param.Value.CleanKeyword()}\"; ";
                else
                    queryOut += $"{param.Key.ToQueryParam()} {param.Value.CleanKeyword()}; ";
            });

            System.Console.WriteLine($"QUERYOUT = {queryOut}");

            return queryOut;
        }

        public static string CleanKeyword(this string k)
        {
            return k.Replace(";", "").Replace("\"", "").Replace("*", "").Replace(",", "").Trim();
        }

        /// <summary>
        /// Capitalize the first letters of a string (for Enums)
        /// </summary>
        /// <param name="s">string input</param>
        /// <param name="t">enum type output</param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumValueException"></exception>
        /// <exception cref="InvalidEnumTypeException"></exception>
        public static string ToCapitalize(this string s, Type t)
        {
            if(t == typeof(OwnershipStatus))
            {
                return s.ToLower() switch
                {
                    "owned" => "Owned",
                    "desired" => "Desired",
                    "traded" => "Traded",
                    "borrowed" => "Borrowed",
                    "sold" => "sold",
                    _ => throw new InvalidEnumValueException($"{t}: Invalid Value")

                };
            }

            if(t == typeof(Condition))
            {
                return s.ToLower() switch
                {
                    "new" => "New",
                    "likenew" => "LikeNew",
                    "used" => "Used",
                    "fair" => "Fair",
                    "poor" => "Poor",
                    _ => throw new InvalidEnumValueException($"{t}: Invalid Value")
                };
            }
            else
            {
                throw new InvalidEnumTypeException($"{nameof(t)}: Invalid Type");
            }
        }
    }
}
