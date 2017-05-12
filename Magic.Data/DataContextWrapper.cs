using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public interface IDataContextWrapper
	{
		IEnumerable<T> GetTable<T>() where T : class;
		int ExecuteCommand(string sql);
		int ExecuteCommand(string obj, params object[] parameters);
		IEnumerable<T> ExecuteQuery<T>(string sql);
		DataContext DEBUG_GetDataContext();
  }

	public class DataContextWrapper : IDataContextWrapper
	{
		private readonly DataContext _dataContext;

		public DataContextWrapper(string connectionString)
		{
			_dataContext = new DataContext(connectionString);
		}

		public DataContext DEBUG_GetDataContext()
		{
			return _dataContext;
		}

		public IEnumerable<T> GetTable<T>() where T : class
		{
			return _dataContext.GetTable<T>();
		}

		public int ExecuteCommand(string sql)
		{
			return _dataContext.ExecuteCommand(sql);
		}

		public int ExecuteCommand(string sql, params object[] parameters)
		{
			return _dataContext.ExecuteCommand(sql, parameters);
		}

		public IEnumerable<T> ExecuteQuery<T>(string sql)
		{
			return _dataContext.ExecuteQuery<T>(sql);
		}
	}
}
