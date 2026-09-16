using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TrainingCatalog.Application;

namespace TrainingCatalog.Api.Tests;

public sealed class TrainingLessonValidationTests
{
    [Fact]
    public async Task AcceptsTrainingWithEightHoursAndTwoFourHourLessons()
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var request = new CreateTrainingRequest("C# Essencial", "Curso de C#", "2026-09-20", 8, 2, 4);

        var response = await client.PostAsJsonAsync("/api/trainings", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var training = await response.Content.ReadFromJsonAsync<Training>();
        Assert.NotNull(training);
        Assert.Equal(request.LessonCount, training.LessonCount);
        Assert.Equal(request.LessonDurationHours, training.LessonDurationHours);
    }

    [Theory]
    [InlineData(3, 4)]
    [InlineData(2, 5)]
    [InlineData(0, 4)]
    [InlineData(2, 0)]
    public async Task RejectsInvalidLessonDetails(int lessonCount, int lessonDurationHours)
    {
        using var factory = new TrainingCatalogApiFactory();
        using var client = factory.CreateClient();
        var request = new CreateTrainingRequest("C# Essencial", "Curso de C#", "2026-09-20", 8, lessonCount, lessonDurationHours);

        var response = await client.PostAsJsonAsync("/api/trainings", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var error = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(error.RootElement.GetProperty("errors").TryGetProperty(
            lessonCount <= 0 ? "lessonCount" : "lessonDurationHours",
            out _));
    }
}
