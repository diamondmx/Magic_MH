using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MagicConsole
{
	[Table(Name="Players")]
	public class Player
	{
		[Column(IsPrimaryKey=true)]
		public string Name;
	}

	[Table(Name="Matches")]
	public class Match
	{
		[Column()] public string Player1;
		[Column()] public string Player2;
		[Column()] public int Round;
		public string Event;
		public int Player1Wins;
		public int Player2Wins;
		public int Draws;
		public bool InProgress;
	}

	class Program
	{
		static void Main(string[] args)
		{
			var db = new DataContext(@"Data Source=localhost\sqlexpress12;Initial Catalog=Magic;Integrated Security=True");
			var playersTable = db.GetTable<Player>();
			

			foreach(var p in playersTable)
				System.Console.Write(p.Name + Environment.NewLine);

			var ignore = System.Console.ReadKey();
		}
	}
}
