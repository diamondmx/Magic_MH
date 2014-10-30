using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public class Match
	{
		public string Player1;
		public string Player2;
		public int Round;
		public string Event;
		public int Player1Wins;
		public int Player2Wins;
		public int Draws;
		public bool InProgress;

		public static Match ReadFromSQLInsertString(string inputString)
		{
			var outputMatch = new Match();

			var inputList = inputString.Split(new char[] { ',' }, StringSplitOptions.None);
			outputMatch.Player1 = inputList[0].Trim(new char[] { '\'', '(', ')', ' ' });
			outputMatch.Player2 = inputList[1].Trim(new char[] { '\'', '(', ')', ' ' });
			outputMatch.Round = Convert.ToInt32(inputList[2]);
			outputMatch.Event = inputList[3].Trim(new char[] { '\'', '(', ')', ' ' });
			outputMatch.Player1Wins = Convert.ToInt32(inputList[4]);
			outputMatch.Player2Wins = Convert.ToInt32(inputList[5]);
			outputMatch.Draws = Convert.ToInt32(inputList[6]);

			return outputMatch;
		}
	}
}
