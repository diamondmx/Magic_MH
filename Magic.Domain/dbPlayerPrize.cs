using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name = "PlayerPrizes")]
	public class dbPlayerPrize
	{
		[Column] public string Player;
		[Column] public int PlayerID;
		[Column] public string EventName;
		[Column] public int Round;
		[Column] public int Position;
		[Column] public int Packs;
		[Column] public int Recieved;
		[Column] public string Notes;
		[Column] public bool Complete;

		public override bool Equals(object obj)
		{
			try
			{
				var playerPrize = obj as dbPlayerPrize;

				if (Player == playerPrize.Player &&
						EventName == playerPrize.EventName &&
						Round == playerPrize.Round &&
						Position == playerPrize.Position &&
						Packs == playerPrize.Packs)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}

			
		}
	}
}