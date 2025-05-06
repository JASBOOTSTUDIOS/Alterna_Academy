IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MusikOn')
BEGIN
    CREATE DATABASE MusikOn;
END;
GO

USE MusikOn;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Companies')
BEGIN
    CREATE TABLE Companies (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Address NVARCHAR(200) NOT NULL
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Position NVARCHAR(100) NOT NULL,
        CompanyId INT NOT NULL,
        FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
    );
END;
GO

IF NOT EXISTS (SELECT * FROM Companies)
BEGIN
    INSERT INTO Companies (Name, Address) VALUES
        ('TechCorp', '123 Main St, Anytown'),
        ('Global Solutions', '456 Oak Ave, Somecity'),
        ('Innovate Inc', '789 Pine Ln, Othertown');
END;
GO

IF NOT EXISTS (SELECT * FROM Employees)
BEGIN
    INSERT INTO Employees (Name, Position, CompanyId) VALUES
        ('John Smith', 'Manager', 1),
        ('Jane Doe', 'Developer', 1),
        ('Bob Johnson', 'Analyst', 2),
        ('Alice Brown', 'Designer', 3),
        ('Peter Jones', 'Engineer', 3);
END;
GO
