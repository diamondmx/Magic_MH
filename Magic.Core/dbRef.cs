using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Magic.Core
{
	public class Constants
	{
		public const string connectionStringkCura = @"Data Source=P-DV-DSK-MHIL;Initial Catalog=Magic;User ID=mhMagic;Password=mtgMagic";
		public const string connectionStringSekhmet = @"Data Source=SEKHMET\SQLEXPRESS12;Initial Catalog=Magic;User ID=magicData;Password=mtgMagic";
        public const string connectionStringSekhmet2 = @"Data Source=SEKHMET\SQLSEKHMET;Initial Catalog=Magic;User ID=mhMagic;Password=mtgMagic";
        public const string currentConnectionString = connectionStringSekhmet2;
	}

	[System.Data.Linq.Mapping.Table(Name = "Players")]
	public class dbPlayer
	{
		[System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
		public string Name;

        [System.Data.Linq.Mapping.Column()]
        public bool Active;


		public static List<dbPlayer> LoadDBPlayers()
		{
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);

			var playersTable = db.GetTable<dbPlayer>().ToList();
			return playersTable;
		}
	}

	[System.Data.Linq.Mapping.Table(Name = "Matches")]
	public class dbMatch
	{
		[System.Data.Linq.Mapping.Column()]
		public string Player1;
		[System.Data.Linq.Mapping.Column()]
		public string Player2;
		[System.Data.Linq.Mapping.Column()]
		public int Round;
		[System.Data.Linq.Mapping.Column()]
		public string Event;
		[System.Data.Linq.Mapping.Column()]
		public int Player1Wins;
		[System.Data.Linq.Mapping.Column()]
		public int Player2Wins;
		[System.Data.Linq.Mapping.Column()]
		public int Draws;
		[System.Data.Linq.Mapping.Column()]
		public bool InProgress;

		public static List<dbMatch> LoadDBMatches(string mtgEvent)
		{
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var matchesTable = db.GetTable<dbMatch>().Where(m => m.Event == mtgEvent).ToList();
			return matchesTable;
		}

        internal static void Update(Match match)
        {
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
            db.ExecuteQuery<dbMatch>(String.Format("UPDATE [Magic]..[Match] SET [Player1Wins]={0}, [Player2Wins]={1}, [Draws]={2}, [InProgress]={3} WHERE [Player1]={4} AND [Player2]={5} AND [Event]={6} AND [Round]={7}", match.Player1Wins, match.Player2Wins, match.Draws, match.InProgress, match.Player1Name, match.Player2Name, match.Event, match.Round));
            db.ExecuteQuery<dbMatch>(String.Format("UPDATE [Magic]..[Match] SET [Player2Wins]={0}, [Player1Wins]={1}, [Draws]={2}, [InProgress]={3} WHERE [Player2]={4} AND [Player1]={5} AND [Event]={6} AND [Round]={7}", match.Player1Wins, match.Player2Wins, match.Draws, match.InProgress, match.Player1Name, match.Player2Name, match.Event, match.Round));
        }

        internal static bool IsMatch(dbMatch checkedMatch, string player1name, string player2name, string eventname, int round)
        {
            if (eventname == checkedMatch.Event && round == checkedMatch.Round)
            {
                if (player1name == checkedMatch.Player1 && player2name == checkedMatch.Player2)
                    return true;
                else if (player1name == checkedMatch.Player2 && player2name == checkedMatch.Player1)
                    return true;
            }

            return true;
                

            
        }
    }

	[System.Data.Linq.Mapping.Table(Name = "Events")]
	public class dbEvent
	{
		[System.Data.Linq.Mapping.Column()]
		public string Name;

        [System.Data.Linq.Mapping.Column()]
        public DateTime StartDate;
        
        [System.Data.Linq.Mapping.Column()]
		public Int32 rounds;

        public static dbEvent LoadDBEvent(string eventName)
        {
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
            return db.GetTable<dbEvent>().Where(e => e.Name == eventName).First();
        }
	}

	[System.Data.Linq.Mapping.Table(Name = "EventPlayers")]
	public class dbEventPlayers
	{
		[System.Data.Linq.Mapping.Column()]
		public string EventName;

		[System.Data.Linq.Mapping.Column()]
		public string Player;

        public static List<dbEventPlayers> LoadDBEventPlayers(string eventName)
        {
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
            return db.GetTable<dbEventPlayers>().Where(ep => ep.EventName == eventName).ToList();
        }
	}
}
