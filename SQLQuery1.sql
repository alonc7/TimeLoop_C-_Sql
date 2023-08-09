--create database timeloop_
--COLLATE Hebrew_CI_AS
--go
--use timeloop_
--go
--drop database timeloop_
--go
--use master
----------------------

-- Create the Users table
CREATE TABLE Users (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL, 
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    HashedPassword NVARCHAR(max) NOT NULL, -- Store hashed password as a string
    Birthdate DATE,
    DateOfCreation DATE DEFAULT GETDATE(), -- Using default constraint to set creation date
    LastLogin DATE
);


go
drop table Users
go
drop table tasks
go

-- Create the Tasks table
CREATE TABLE Tasks (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    StartDate DATE,
    DueDate DATE,
    IsOnDate BIT NOT NULL DEFAULT 0,
    IsOnTime BIT NOT NULL DEFAULT 0,
    StartTime TIME,
    DueTime TIME,
    Priority NVARCHAR(10),
    Status NVARCHAR(10),
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);


-- Create lookup tables for Priority and Status
CREATE TABLE PriorityLookup (
    Id INT PRIMARY KEY,
    PriorityName NVARCHAR(10) NOT NULL
);

INSERT INTO PriorityLookup (Id, PriorityName)
VALUES (1, 'Low'), (2, 'Medium'), (3, 'High');
select * from PriorityLookup
go
CREATE TABLE StatusLookup (
    Id INT PRIMARY KEY,
    StatusName NVARCHAR(20) NOT NULL
);

INSERT INTO StatusLookup (Id, StatusName)
VALUES (1, 'Pending'), (2, 'Completed');
select * from StatusLookup

-- Inserting data into the Users table:
INSERT INTO Users (Email, FirstName, LastName, HashedPassword, Birthdate, LastLogin)
VALUES
    ('user3@example.com', 'John', 'Doe', HASHBYTES('SHA2_256', '111111'), '1990-05-15', GETDATE()),
    ('user4@example.com', 'Jane', 'Smith', HASHBYTES('SHA2_256', '111111'), '1985-10-30', GETDATE());

select * from Users



-- Generating random tasks for each user:
DECLARE @StartDateRangeStart DATE = '2023-08-01';
DECLARE @StartDateRangeEnd DATE = '2023-08-31';
DECLARE @DueDateRangeStart DATE = '2023-09-01';
DECLARE @DueDateRangeEnd DATE = '2023-09-30';

-- Declare variables to store user information during the loop
DECLARE @UserId INT;
DECLARE @Email NVARCHAR(255);
DECLARE @TaskTitle NVARCHAR(100);
DECLARE @Priority NVARCHAR(10); -- Updated data type
DECLARE @Status NVARCHAR(10); -- Updated data type

-- Create a cursor to loop through each user
DECLARE UserCursor CURSOR FOR
SELECT Id, Email FROM Users;

-- Open the cursor
OPEN UserCursor;

-- Fetch the first row
FETCH NEXT FROM UserCursor INTO @UserId, @Email;

-- Start the loop
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Generate tasks for the current user (@UserId)
    INSERT INTO Tasks (UserId, Title, StartDate, DueDate, IsOnDate, IsOnTime, StartTime, DueTime, Priority, Status)
    SELECT
        @UserId AS UserId,
        CONCAT('Task ', NEWID()) AS Title,
        DATEADD(DAY, ROUND(RAND() * DATEDIFF(DAY, @StartDateRangeStart, @StartDateRangeEnd), 0), @StartDateRangeStart) AS StartDate,
        DATEADD(DAY, ROUND(RAND() * DATEDIFF(DAY, @DueDateRangeStart, @DueDateRangeEnd), 0), @DueDateRangeStart) AS DueDate,
        CASE WHEN RAND() > 0.5 THEN 1 ELSE 0 END AS IsOnDate,
        CASE WHEN RAND() > 0.5 THEN 1 ELSE 0 END AS IsOnTime,
        CONVERT(VARCHAR(5), DATEADD(MINUTE, CAST(RAND() * 1440 AS INT), '00:00'), 108) AS StartTime,
        CONVERT(VARCHAR(5), DATEADD(MINUTE, CAST(RAND() * 1440 AS INT), '00:00'), 108) AS DueTime,
        CASE WHEN RAND() > 0.5 THEN 'Low' WHEN RAND() > 0.3 THEN 'Medium' ELSE 'High' END AS Priority, -- Random Priority
        CASE WHEN RAND() > 0.5 THEN 'Pending' ELSE 'Completed' END AS Status -- Random Status
    FROM Users WHERE Id = @UserId;

    -- Fetch the next row
    FETCH NEXT FROM UserCursor INTO @UserId, @Email;
END

-- Close and deallocate the cursor
CLOSE UserCursor;
DEALLOCATE UserCursor;

-- Optional: Display all generated tasks for all users
SELECT * FROM Tasks;
select * from users;




CREATE PROCEDURE VerifyUserPassword
    @UserEmail NVARCHAR(255),
    @Password NVARCHAR(100),
    @IsPasswordMatch BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @HashedPassword NVARCHAR(max)

    -- Get the hashed password for the provided user email
    SELECT @HashedPassword = HashedPassword
    FROM Users
    WHERE Email = @UserEmail;

    -- Compare the provided password with the stored hashed password
    IF @HashedPassword IS NOT NULL AND @HashedPassword = HASHBYTES('SHA2_256', @Password)
    BEGIN
        -- Passwords match
        SET @IsPasswordMatch = 1;
    END
    ELSE
    BEGIN
        -- Passwords do not match
        SET @IsPasswordMatch = 0;
    END
END

select StartDate from tasks