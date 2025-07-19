# EnglishUp

# 📦 Domain Models – EnglishUp Platform
C# .NET 9 | Clean Architecture | Domain Layer
These models define the structure of users, subscriptions, learning content, test tracking, and gamification for an IELTS preparation platform.

### 👤 User
```
Id (Guid)
FullName (string)
Email (string)
PasswordHash (string)
Age (int)
RoleId (Guid)
Role (Role)
Subscriptions (List<Subscription>)
UserCourses (List<UserCourse>)
HomeworkSubmissions (List<HomeworkSubmission>)
PointTransactions (List<PointTransaction>)
MockTests (List<MockTest>)
```

### 🔐 Role
```
Id (Guid)
Name (string)
Users (List<User>)
```

### 📝 Course
```
Id (Guid)
Title (string)
Level (CourseLevel)
UserCourses (List<UserCourse>)
Homeworks (List<Homework>)
```

### 🎓 UserCourse
```
UserId (Guid)
CourseId (Guid)
EarnedPoints (int)
IsCompleted (bool)
User (User)
Course (Course)
```

### 📚 Homework
```
Id (Guid)
CourseId (Guid)
Title (string)
Question (string)
CorrectAnswer (string)
Course (Course)
Submissions (List<HomeworkSubmission>)
```

### ✍️ HomeworkSubmission
```
Id (Guid)
UserId (Guid)
HomeworkId (Guid)
UserAnswer (string)
IsCorrect (bool)
SubmittedAt (DateTime)
User (User)
Homework (Homework)
```

### ⭐ PointTransaction
```
Id (Guid)
UserId (Guid)
Points (int)
Description (string)
EarnedAt (DateTime)
User (User)
```

### 🧪 MockTest
```
Id (Guid)
UserId (Guid)
TestType (MockTestType)
TakenAt (DateTime)
User (User)
Results (List<MockTestResult>)
```

### 📊 MockTestResult
```
Id (Guid)
MockTestId (Guid)
Section (TestSection)
Score (double)
MockTest (MockTest)
```

### 📬 Subscription
```
Id (Guid)
UserId (Guid)
Type (SubscriptionType)
StartDate (DateTime)
ExpiryDate (DateTime)
User (User)
```

-------------------------------------


# 🔁 Enums
### 🏷️ SubscriptionType
```
    CourseOnly = 0,
    Monthly = 1,
    Quarterly = 2,
    Yearly = 3
```

### 🧭 CourseLevel
```
    Beginner = 0,
    Elementary = 1,
    PreIntermediate = 2,
    Intermediate = 3,
    UpperIntermediate = 4,
    Advanced = 5,
    IELTS = 6,
    TOEFL = 7
```
### 🧪 MockTestType
```
    IELTS,
    TOEFL
```

### 📚 TestSection
```
    Listening,
    Reading,
    Writing,
    Speaking
```
These are the all models of the project.
