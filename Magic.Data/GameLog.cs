using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Magic.Data
{
	public interface IGameLog
	{
		void Add(string description, string user, string eventName, object details);
	}

	public class GameLog : IGameLog
	{
		IDataContextWrapper _dataContext;

		public GameLog(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<GameLogEntry> GetAll()
		{
			return _dataContext.GetTable<GameLogEntry>();
		}

		public void Add(string description, string user, string eventName, object details)
		{
			var newEntry = new GameLogEntry
			{
				Description = description,
				User = user,
				Details = details
			};
			
			
			//string detailsJson = new JavaScriptSerializer().Serialize(newEntry.Details);

			string insertSql = $"INSERT INTO [GameLog] ([Description], [User], [TimeStamp], [Event], [Details]) VALUES";
			insertSql += $"('{newEntry.Description}', '{newEntry.User}', getdate(), '{newEntry.EventName}', NULL)";
      _dataContext.ExecuteCommand(insertSql);
		}
	}
}
