/*Same as table generation, but using queries, for StudySystem.mdf*/

IF OBJECT_ID('Students') IS NULL
                CREATE TABLE Students(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                FirstName VARCHAR(50) NOT NULL,
                LastName VARCHAR(50) NOT NULL,
                Age INT NOT NULL
                );

IF OBJECT_ID('Assignments') IS NULL
                CREATE TABLE Assignments(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                Date DATETIME,
                Description VARCHAR(300),
                Type INT NOT NULL
                );

IF OBJECT_ID('Teachers') IS NULL
                CREATE TABLE Teachers(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                FirstName VARCHAR(50) NOT NULL,
                LastName VARCHAR(50) NOT NULL
                );


IF OBJECT_ID('AssignmentsStatistics') IS NULL
                CREATE TABLE AssignmentsStatistics(
                StudentId INT NOT NULL,
                AssignmentId INT NOT NULL,
                Points INT
                );

IF OBJECT_ID('Courses') IS NULL
                CREATE TABLE Courses(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                Name VARCHAR(50) NOT NULL,
                TeacherId INT NOT NULL
                );

IF OBJECT_ID('Drafts') IS NULL
                CREATE TABLE Drafts(
                StudentId INT NOT NULL,
                CourseId INT NOT NULL
                );