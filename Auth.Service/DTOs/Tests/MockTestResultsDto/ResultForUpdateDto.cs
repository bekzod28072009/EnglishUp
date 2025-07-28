namespace Auth.Service.DTOs.Tests.MockTestResultsDto;

public class ResultForUpdateDto
{
    public int ListeningScore { get; set; }
    public int ReadingScore { get; set; }
    public int WritingScore { get; set; }
    public int SpeakingScore { get; set; }
    public int OverallScore { get; set; }

    public TimeSpan TimeTaken { get; set; }
    public DateTime CompletedAt { get; set; }
}
