CREATE TABLE Registration
(
    RegistrationId INT IDENTITY(1,1) PRIMARY KEY,

    Name VARCHAR(50) NOT NULL,

    Username NVARCHAR(50) NOT NULL UNIQUE,

    PasswordHash NVARCHAR(255) NOT NULL,

    DateOfBirth DATE NOT NULL,

    Gender NVARCHAR(20) NOT NULL,

    Address NVARCHAR(500) NULL,

    StateId INT NOT NULL,

    CityId INT NOT NULL,

    Pincode NVARCHAR(10) NOT NULL,

    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
);


CREATE TABLE State
(
    StateId INT IDENTITY(1,1) PRIMARY KEY,

    StateName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE City
(
    CityId INT IDENTITY(1,1) PRIMARY KEY,

    CityName NVARCHAR(100) NOT NULL,

);

CREATE TABLE Hobby
(
    HobbyId INT IDENTITY(1,1) PRIMARY KEY,

    HobbyName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE RegistrationDocument
(
    DocumentId INT IDENTITY(1,1) PRIMARY KEY,

    RegistrationId INT NOT NULL,

    FileName NVARCHAR(255) NOT NULL,

    FilePath NVARCHAR(500) NOT NULL,

    FileExtension NVARCHAR(20) NOT NULL,

    FileSize BIGINT NOT NULL,

    UploadedDate DATETIME2 NOT NULL DEFAULT GETDATE(),


);



CREATE PROCEDURE SP_SaveOrUpdateUser
(
    @Id INT = 0,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @City NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF (@Id = 0)
    BEGIN
        -- Insert
        INSERT INTO UserDetails
        (
            Name,
            Email,
            City
        )
        VALUES
        (
            @Name,
            @Email,
            @City
        );

        -- Return newly inserted record
        SELECT *
        FROM UserDetails
        WHERE Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update
        UPDATE UserDetails
        SET
            Name = @Name,
            Email = @Email,
            City = @City
        WHERE Id = @Id;

        -- Return updated record
        SELECT *
        FROM UserDetails
        WHERE Id = @Id;
    END
END


CREATE PROCEDURE GetCitiesByState
    @StateId INT
AS
BEGIN
    SELECT
        CityId,
        CityName
    FROM City
    WHERE StateId = @StateId
    ORDER BY CityName;
END


CREATE PROCEDURE SP_Login
    @Username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RegistrationId,
        Name,
        Username,
        PasswordHash
    FROM Registration
    WHERE Username = @Username;
END

--drop table Registration;