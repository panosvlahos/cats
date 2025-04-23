

CREATE DATABASE Cats;
-- Use the database

USE Cats;
GO

-- Create table: TagEntity
CREATE TABLE Tags (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Created DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Create table: CatEntity
CREATE TABLE Cats (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CatId NVARCHAR(100) NOT NULL UNIQUE,
    Width INT NOT NULL,
    Height INT NOT NULL,
    Image VARBINARY(MAX) NOT NULL,
    Created DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Create join table: CatTag
CREATE TABLE CatTags (
    CatEntityId INT NOT NULL,
    TagEntityId INT NOT NULL,
    PRIMARY KEY (CatEntityId, TagEntityId),
    FOREIGN KEY (CatEntityId) REFERENCES Cats(Id) ON DELETE CASCADE,
    FOREIGN KEY (TagEntityId) REFERENCES Tags(Id) ON DELETE CASCADE
);
GO