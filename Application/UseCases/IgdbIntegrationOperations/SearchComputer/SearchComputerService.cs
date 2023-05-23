﻿using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer
{
    public class SearchComputerService : ISearchComputerService
    {
        private readonly HttpClient httpClient;
        public SearchComputerService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel> GetById(int id)
        {
            string query = "fields *, platform_logo.image_id, versions.name, versions.platform_logo.image_id, summary, url, websites.url, websites.category; where category = (2, 6); where id = " + id.ToString() + "; ";

            var res = await httpClient.IgdbPostAsync<List<PlatformResponseModel>>(query, "platforms");

            return res.Ok();
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> SearchBy(string name)
        {
            if (string.IsNullOrEmpty(name)) { return GenericResponses.NotFound("Field \"search cannot be empty\""); }

            string query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y;\r\nlimit 50;\r\nwhere category = (2, 6);\r\nsearch \"{name.CleanKeyword()}\"; ";

            Console.WriteLine(query);

            var res = await httpClient.IgdbPostAsync<List<SearchComputerResponseModel>>(query, "platforms");

            return res.Ok();
        }
    }
}
