using Magic.Data;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public interface IPrizeManager
	{
		void SavePrizes(List<dbRoundPrize> input);
		IEnumerable<dbPlayerPrize> GetPlayerPrizes(string playerName);
		void AcknowledgeRecievedAll(string name, List<dbPlayerPrize> acknowledgedList);
		IEnumerable<dbPlayerPrize> GetUncollectedPlayerPrizes(string playerName);
	}

	public class PrizeManager : IPrizeManager
	{
		IRoundPrizeRepository _roundPrizeRepo;
		IPlayerPrizeRepository _playerPrizeRepo;

		public PrizeManager(IRoundPrizeRepository roundPrizeRepo, IPlayerPrizeRepository playerPrizeRepo)
		{
			_roundPrizeRepo = roundPrizeRepo;
			_playerPrizeRepo = playerPrizeRepo;
		}

		public void SavePrizes(List<dbRoundPrize> input)
		{
			_roundPrizeRepo.SaveDBRoundPrizes(input);
		}

		public IEnumerable<dbPlayerPrize> GetUncollectedPlayerPrizes(string playerName)
		{
			var list = GetPlayerPrizes(playerName);
			return list.Where(pp => pp.Recieved != pp.Packs);
		}

		public IEnumerable<dbPlayerPrize> GetPlayerPrizes(string playerName)
		{
			return _playerPrizeRepo.GetAwardedPrizes(playerName).Where(p => p.Player == playerName);
		}

		public static string FormatPrizeInfo(IEnumerable<dbPlayerPrize> prizes)
		{
			StringBuilder prizelist = new StringBuilder();
			
			foreach(var prize in prizes)
			{
				int packsRemaining = prize.Packs - prize.Recieved;
				if(packsRemaining!=0)
				{
					prizelist.AppendLine($"({prize.EventName}:{prize.Round}) {packsRemaining} packs");
				}
      }
			
			if(string.IsNullOrWhiteSpace(prizelist.ToString()))
			{
			 	return "";
			}

			return "Prizes to collect: " + prizelist.ToString();
		}

		public void AcknowledgeRecievedAll(string name, List<dbPlayerPrize> acknowledgedList)
		{
			var playerPrizes = GetPlayerPrizes(name);

			foreach(var prize in acknowledgedList)
			{
				if (!playerPrizes.Contains(prize))
				{
					acknowledgedList.Remove(prize);
				}
			}

			_playerPrizeRepo.MarkRecieved(acknowledgedList);

		}
	}
}
