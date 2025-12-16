using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace LeadRouteOptimizer.Tests.Integration
{
    public sealed class UploadsAndPlansTests : IClassFixture<TestAppFactory>
    {
        private readonly HttpClient _client;

        public UploadsAndPlansTests(TestAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Upload_manager_and_personal_then_create_plan_returns_stops()
        {
            // Integration test: exercises HTTP pipeline + EF + SQL.

            var managerId = await UploadAsync("Manager", Path.Combine(AppContext.BaseDirectory, "TestData", "manager.csv"));
            var personalId = await UploadAsync("Personal", Path.Combine(AppContext.BaseDirectory, "TestData", "personal.csv"));

            var createPlanResp = await _client.PostAsJsonAsync("/plans", new
            {
                homeLatitude = 41.8781m,
                homeLongitude = -87.6298m,
                uploadBatchIds = new[] { managerId, personalId }
            });

            createPlanResp.IsSuccessStatusCode.Should().BeTrue();

            var createPlanJson = await createPlanResp.Content.ReadFromJsonAsync<CreatePlanResponse>();
            createPlanJson.Should().NotBeNull();
            createPlanJson!.Stops.Should().BeGreaterThan(0);
            createPlanJson.TotalDistanceKm.Should().BeGreaterThan(0);

            var getPlanResp = await _client.GetAsync($"/plans/{createPlanJson.PlanId}");
            getPlanResp.IsSuccessStatusCode.Should().BeTrue();

            var planJson = await getPlanResp.Content.ReadFromJsonAsync<GetPlanResponse>();
            planJson.Should().NotBeNull();
            planJson!.Stops.Count.Should().Be(createPlanJson.Stops);
        }

        private async Task<Guid> UploadAsync(string sourceType, string filePath)
        {
            await using var fs = File.OpenRead(filePath);

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(sourceType), "sourceType");

            var fileContent = new StreamContent(fs);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            content.Add(fileContent, "file", Path.GetFileName(filePath));

            var resp = await _client.PostAsync("/uploads", content);
            resp.IsSuccessStatusCode.Should().BeTrue();

            var json = await resp.Content.ReadFromJsonAsync<UploadResponse>();
            json.Should().NotBeNull();
            json!.ValidRows.Should().BeGreaterThan(0);

            return json.UploadBatchId;
        }

        private sealed record UploadResponse(Guid UploadBatchId, int TotalRows, int ValidRows, int InvalidRows);
        private sealed record CreatePlanResponse(Guid PlanId, int Stops, decimal TotalDistanceKm);
        private sealed record GetPlanResponse(Guid PlanId, decimal TotalDistanceKm, List<Stop> Stops);
        private sealed record Stop(int Sequence, Guid LeadId, string LeadName, decimal LegDistanceKm);
    }

}
