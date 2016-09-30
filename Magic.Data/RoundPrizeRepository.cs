using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public class RoundPrizeRepository : IRoundPrizeRepository
	{
		private readonly IDataContextWrapper _dataContext;

		public RoundPrizeRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbRoundPrize> LoadDBRoundPrizes(string eventName)
		{
			return _dataContext.GetTable<dbRoundPrize>().Where(ep => ep.EventName == eventName).ToList();
		}

		public void SaveDBRoundPrizes(List<dbRoundPrize> input)
		{
			var eventName = input.First().EventName;
			var round = input.First().Round;
			var positions = string.Join(",", input.Select(i => i.Position));

			string deleteSql = $"DELETE FROM RoundPrizes WHERE EventName='{eventName}' AND Round={round}";
			var updateHeaderSql = $"INSERT INTO RoundPrizes(EventName, Round, Position, Packs, Other) VALUES ";
			string updateSql = updateHeaderSql;
			foreach (var row in input)
			{
				if (updateSql.Last() == ')')
					updateSql += ",";

				updateSql += $"('{eventName}', {round}, {row.Position}, {row.Packs}, '{row.Other}')";
      }



			_dataContext.ExecuteCommand(deleteSql);
			_dataContext.ExecuteCommand(updateSql);
		}
	}
}
