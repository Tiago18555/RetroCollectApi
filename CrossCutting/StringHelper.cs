﻿using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Diagnostics;

namespace RetroCollectApi.CrossCutting
{
    public static class StringHelper
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
                _ => "invalid: " + s
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

                queryOut += $"{param.Key.ToQueryParam()} \"{param.Value.CleanKeyword()}\"; ";
            });


            System.Console.WriteLine($"QUERYOUT = {queryOut}");

            return queryOut;
        }

        public static string CleanKeyword(this string k)
        {
            return k.Replace(";", "").Replace("\"", "").Replace("*", "").Replace(",", "").Trim();
        }

        public static string ToCapitalize(this string s, Type t)
        {
            if(t == typeof(OwnershipStatus))
            {
                return s switch
                {
                    "owned" => "Owned",
                    "desired" => "Desired",
                    "traded" => "Traded",
                    "borrowed" => "Borrowed",
                    "sold" => "sold",
                    _ => "Invalid"
                };
            }

            if(t == typeof(Condition))
            {
                return s switch
                {
                    "new" => "New",
                    "likenew" => "LikeNew",
                    "used" => "Used",
                    "fair" => "Fair",
                    "poor" => "Poor",
                    _ => "Invalid",
                };
            }
            else
            {
                throw new ArgumentException("Invalid Type");
            }
        }
    }
}
