# EnglishUp

# 📦 Domain Models – EnglishUp Platform
C# .NET 9 | Clean Architecture | Domain Layer
These models define the structure of users, subscriptions, learning content, test tracking, and gamification for an IELTS preparation platform.

Here's all domain models of the project, including their enums :
### ✅ Auditable Base Class
```
public abstract class Auditable
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
}
```

-----------------------------------------------------------

### 👤 USER MANAGEMENT
```
public class User : Auditable
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Age { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public ICollection<UserCourse> UserCourses { get; set; }
    public ICollection<UserHomework> UserHomeworks { get; set; }
    public ICollection<MockTestResult> MockTestResults { get; set; }
    public ICollection<PointTransaction> PointTransactions { get; set; }
}
```
```
public class Role : Auditable
{
    public string Name { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; }
}
```

-----------------------------------------------------------

### 📘 COURSES
```
public class Course : Auditable
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CourseLevel Level { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
    public ICollection<UserCourse> UserCourses { get; set; }
}
```
```
public enum CourseLevel
{
    Beginner,
    Intermediate,
    Advanced
}
```
```
public class Lesson : Auditable
{
    public string Title { get; set; } = string.Empty;
    public long CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<LessonPart> Parts { get; set; }
    public ICollection<Homework> Homeworks { get; set; }
}
```
```
public class LessonPart : Auditable
{
    public string Type { get; set; } = string.Empty; // Vocabulary, Grammar, etc.
    public string Content { get; set; } = string.Empty;
    public long LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
```

```
public class UserCourse : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public long CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
```

-----------------------------------------------------------

### 📝 HOMEWORK
```
public class Homework : Auditable
{
    public string Question { get; set; } = string.Empty;
    public long LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
```
```
public class UserHomework : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public long HomeworkId { get; set; }
    public Homework Homework { get; set; } = default!;
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
}
```
```
public class PointTransaction : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public int Points { get; set; }
    public string Reason { get; set; } = string.Empty;
}
```

-----------------------------------------------------------

### 🧪 MOCK TEST
```
public class MockTest : Auditable
{
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public ICollection<MockTestResult> Results { get; set; }
}
```
```
public class MockTestResult : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public long MockTestId { get; set; }
    public MockTest MockTest { get; set; } = default!;
    public int ListeningScore { get; set; }
    public int ReadingScore { get; set; }
    public int WritingScore { get; set; }
    public int SpeakingScore { get; set; }
}
```

-----------------------------------------------------------

### 💳 SUBSCRIPTION
```
public class Subscription : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => EndDate >= DateTime.UtcNow;
}
```
```
public enum SubscriptionType
{
    FullCourse,
    BeginnerOnly,
    IntermediateOnly,
    AdvancedOnly
}
```

-----------------------------------------------------------

### 🔥 GAMIFICATION
```
public class DailyChallenge : Auditable
{
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
```
```
public class Streak : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public int DaysInRow { get; set; }
    public DateTime LastActive { get; set; }
}
```


## These are the all models of the project.
