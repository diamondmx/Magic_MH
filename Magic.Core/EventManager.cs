using Magic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Domain;

namespace Magic.Core
{
	public interface IEventManager
	{
		Event LoadEvent(string eventName);
		List<Event> LoadAllEvents();
		void SaveEvent(Event thisEvent);
		void CreateEvent(Event thisEvent);
		void AddPlayer(Event thisEvent, Player newPlayer);
	}

	public class EventManager : IEventManager
	{
		private readonly IEventRepository _eventRepository;

		public EventManager(IEventRepository eventRepo)
		{
			_eventRepository = eventRepo;
		}

		public Event LoadEvent(string eventName)
		{
			return _eventRepository.LoadEvent(eventName);
		}

		public List<Event> LoadAllEvents()
		{
			return _eventRepository.LoadAllEvents();
		}

		public void SaveEvent(Event thisEvent)
		{
			_eventRepository.SaveEvent(thisEvent);
		}

		public void CreateEvent(Event thisEvent)
		{
			_eventRepository.CreateEvent(thisEvent);
		}

		public void AddPlayer(Event thisEvent, Player newPlayer)
		{
			_eventRepository.AddPlayer(thisEvent, newPlayer);
		}
  }
}
