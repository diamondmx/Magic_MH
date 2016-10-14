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
		void Add(string description, string eventName, object details);
		void SetPlayerContext(string playerName);
  }

	public class GameLog : IGameLog
	{
		IDataContextWrapper _dataContext;
		string _playerContext = "Not-Set";

		public GameLog(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<GameLogEntry> GetAll()
		{
			return _dataContext.GetTable<GameLogEntry>();
		}

		public void SetPlayerContext(string playerName)
		{
			if(_playerContext != null)
			{
				_playerContext = playerName;
			}
			else
			{
				_playerContext = "Not-Set";
			}
			
		}

		public void Add(string description, string eventName, object details)
		{
			var newEntry = new GameLogEntry
			{
				Description = description,
				User = _playerContext,
				Details = details
			};
			
			//string detailsJson = new JavaScriptSerializer().Serialize(newEntry.Details);

			string insertSql = $"INSERT INTO [GameLog] ([Description], [User], [TimeStamp], [Event], [Details]) VALUES";
			insertSql += $"('{newEntry.Description}', '{newEntry.User}', getdate(), '{newEntry.EventName}', NULL)";
      _dataContext.ExecuteCommand(insertSql);
		}
	}
}
