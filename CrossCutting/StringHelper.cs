﻿using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations;

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

        /*
        cover_small,
        screenshot_med,
        cover_big,
        logo_med,
        screenshot_big,
        screenshot_huge,
        thumb,
        micro,
        a720p,
        a1080p*/

        /*
                         [FromQuery] string search,
                [FromQuery] string genre,
                [FromQuery] string[] keyword,
                [FromQuery] string companie,
                [FromQuery] string language,
                [FromQuery] string theme,
                [FromQuery] string releaseyear
         */


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

            if(querys.Count == 0) { return queryOut; }

            querys.ToList<KeyValuePair<string, string>>().ForEach(param =>
            {
                if (param.Key.ToQueryParam().StartsWith("invalid:"))
                    throw new Exception("Invalid query param.");

                queryOut += $"{param.Key.ToQueryParam()} \"{param.Value}\"; ";
            });


            System.Console.WriteLine($"QUERYOUT = {queryOut}");

            return queryOut;
        }
    }
}
