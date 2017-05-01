using System;

namespace Magic.Data
{
	internal class Logger
	{
		private DataContextWrapper dataContext;

		public Logger(DataContextWrapper dataContext)
		{
			this.dataContext = dataContext;
		}

		public void LogError(string message, string user, string source, Exception exception)
		{
			string sql = $"INSERT INTO MagicLogging (message, user, source, exception) VALUES (@message, @user, @source, @exception)";
			dataContext.ExecuteCommand(sql, message, user, source, exception);
		}
	}
}