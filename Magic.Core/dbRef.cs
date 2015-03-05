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

        public dbMatch(Magic.Core.Match m)
        {
            Player1 = m.Player1Name;
            Player2 = m.Player2Name;
            Event = m.Event;
            Round = m.Round;
            Player1Wins = m.Player1Wins;
            Player2Wins = m.Player2Wins;
            Draws = m.Draws;
            InProgress = m.InProgress;
        }

        public dbMatch()
        { }

        public void Copy(dbMatch m)
        {
            this.Player1 = m.Player1;
            this.Player2 = m.Player2;
            this.Event = m.Event;
            this.Round = m.Round;
            this.Player1Wins = m.Player1Wins;
            this.Player2Wins = m.Player2Wins;
            this.Draws = m.Draws;
            this.InProgress = m.InProgress;
        }

		public static List<dbMatch> LoadDBMatches(string mtgEvent)
		{
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var matchesTable = db.GetTable<dbMatch>().Where(m => m.Event == mtgEvent).ToList();
			return matchesTable;
		}

        public void Update()
        {
            var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
            db.ExecuteQuery<dbMatch>(String.Format("UPDATE [Magic]..[Matches] SET [Player1Wins]={0}, [Player2Wins]={1}, [Draws]={2}, [InProgress]='{3}' WHERE [Player1]='{4}' AND [Player2]='{5}' AND [Event]='{6}' AND [Round]={7}", Player1Wins, Player2Wins, Draws, InProgress, Player1, Player2, Event, Round));
            db.ExecuteQuery<dbMatch>(String.Format("UPDATE [Magic]..[Matches] SET [Player2Wins]={0}, [Player1Wins]={1}, [Draws]={2}, [InProgress]='{3}' WHERE [Player2]='{4}' AND [Player1]='{5}' AND [Event]='{6}' AND [Round]={7}", Player1Wins, Player2Wins, Draws, InProgress, Player1, Player2, Event, Round));
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

        public bool Read(string eventName, int round, string p1, string p2)
        {
            var allMatches = dbMatch.LoadDBMatches(eventName);
            var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
            var roundMatches = eventMatches.Where(m => m.Round == round).ToList();
            
            var matchesAsP1 = roundMatches.Where(m => m.Player1 == p1 && m.Player2 == p2);
            if(matchesAsP1.Any())
            {
                var foundMatch = matchesAsP1.First();
                Copy(foundMatch);
                return true;

            }
            else
            {
                var matchesAsP2 = roundMatches.Where(m => m.Player1 == p2 && m.Player2 == p1);
                if(matchesAsP2.Any())
                {
                    var foundMatch = matchesAsP2.First();
                    Copy(foundMatch);
                    return true;
                }
            }

            return false;
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

