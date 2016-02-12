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
		List<T> GetTable<T>() where T : class;
		void ExecuteCommand(string sql);
		void ExecuteCommand(string sql, object o1, object o2);
		void ExecuteCommand(string sql, object o1);
  }

	public class DataContextWrapper : IDataContextWrapper
	{
		private readonly DataContext _dataContext;

		public DataContextWrapper(string connectionString)
		{
			_dataContext = new DataContext(connectionString);
		}

		public List<T> GetTable<T>() where T : class
		{
			return _dataContext.GetTable<T>().ToList();
		}

		public void ExecuteCommand(string sql)
		{
			_dataContext.ExecuteCommand(sql);
		}

		public void ExecuteCommand(string sql, object o1)
		{
			_dataContext.ExecuteCommand(sql, o1);
		}

		public void ExecuteCommand(string sql, object o1, object o2)
		{
			_dataContext.ExecuteCommand(sql, o1, o2);
		}
	}
}
