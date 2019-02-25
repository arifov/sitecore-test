CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Email] NVARCHAR(255) NOT NULL,
	[Password] NVARCHAR(255) NOT NULL
)

GO 

CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email])

