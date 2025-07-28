﻿using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Tests;

public class TestResult : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }

    public long MockTestId { get; set; }
    public MockTest MockTest { get; set; }

    public int ListeningScore { get; set; }
    public int ReadingScore { get; set; }
    public int WritingScore { get; set; }
    public int SpeakingScore { get; set; }
    public int OverallScore { get; set; }

    public TimeSpan TimeTaken { get; set; }
    public DateTime CompletedAt { get; set; }
}
