using System.Collections.Generic;
using Magic.Domain;

namespace Magic.Data
{
	public interface IEventRepository
	{
		//void AddPlayer(dbEvent e, dbPlayer dbPlayer);
		//void Create(dbEvent e);
		//List<dbEvent> LoadAllDBEvents();
		//dbEvent LoadDBEvent(string eventName);
		//void Update(dbEvent e);

		void SaveEvent(Event saveEvent);
    Event LoadEvent(string eventName);
		List<Event> LoadAllEvents();
		void CreateEvent(Event createEvent);
		void AddPlayer(Event thisEvent, Player newPlayer);
		dbEvent GetCurrentEvent();
		List<dbEvent> LoadAllDBEvents();
  }
}