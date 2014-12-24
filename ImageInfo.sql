CREATE TABLE [dbo].[ImageInfo]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] NCHAR(50) NOT NULL, 
	[ContentType] NCHAR(100) NOT NULL,
    [ExifDate] DATETIME NOT NULL, 
    [ExifLattitude] NCHAR(25) NULL, 
    [ExifLongitude] NCHAR(25) NULL, 
    [StorageType] NCHAR(10) NULL, 
    [ImageId] INT NULL,
	CONSTRAINT [FK_Images_Id] FOREIGN KEY ([ImageId])
		REFERENCES [Image] ([Id])
)
