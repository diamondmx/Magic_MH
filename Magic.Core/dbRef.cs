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
	}
}
