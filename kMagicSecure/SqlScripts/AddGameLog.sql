CREATE TABLE [dbo].[GameLog](
	[Description] nvarchar NOT NULL,
	[User] nvarchar(50) NULL,
	[Timestamp] datetime NULL,
	[Event] nvarchar(50) NULL,
	[Details] xml NULL
)