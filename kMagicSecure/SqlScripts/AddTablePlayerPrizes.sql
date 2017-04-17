--namespace Magic.Domain
--{
--	[Table(Name = "PlayerPrizes")]
--	public class dbPlayerPrize
--	{
--		[Column] public string Player;
--		[Column] public string EventName;
--		[Column] public int Round;
--		[Column] public int Position;
--		[Column] public int PacksAwarded;
--		[Column] public int PacksRecieved;
--		[Column] public string Notes;
--	}
--}
/*
CREATE TABLE [dbo].[PlayerPrizes](
	[Player] [nvarchar](50) NOT NULL,
	[EventName] [nvarchar](3) NOT NULL,
	[Round] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[Packs] [int] NOT NULL,
	[Recieved] [int] NOT NULL DEFAULT(0),
	[Notes] [NVARCHAR](MAX) NULL,
	[Complete] [bit] NOT NULL DEFAULT(0)
PRIMARY KEY CLUSTERED 
(
	[Player] ASC,
	[EventName] ASC,
	[Round] DESC,
	[Position] ASC,
)
*/