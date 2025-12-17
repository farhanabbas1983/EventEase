using System;
using System.Collections.Generic;
using System.Linq;
using EventEase.Models;

namespace EventEase.Services
{
    public class EventService
    {
        private readonly List<Event> _events = new()
        {
            new Event { Name = "Launch Party", Date = new DateTime(2026, 1, 15, 18, 0, 0), Location = "City Hall" },
            new Event { Name = "Annual Conference", Date = new DateTime(2026, 3, 10), Location = "Convention Center" }
        };

        // Simple lock to keep mutations thread-safe if the app evolves to server-side or parallel calls
        private readonly object _lock = new();

        // Ensure initial ordering
        public EventService()
        {
            _events.Sort((a, b) => a.Date.CompareTo(b.Date));
        }

        // Return a snapshot of current events to avoid callers re-enumerating the internal list and to be resilient to concurrent changes
        public IReadOnlyList<Event> GetAll()
        {
            lock (_lock)
            {
                return _events.ToList();
            }
        }

        public Event? GetById(Guid id) => _events.FirstOrDefault(e => e.Id == id);

        public void Add(Event ev)
        {
            if (ev == null) return;
            // ensure a fresh GUID
            ev.Id = Guid.NewGuid();

            lock (_lock)
            {
                // insert into sorted position by Date to avoid re-sorting later
                var index = _events.FindIndex(e => e.Date > ev.Date);
                if (index < 0)
                    _events.Add(ev);
                else
                    _events.Insert(index, ev);
            }
        }

        public bool UpdateEvent(Guid id, Event updated)
        {
            if (updated == null) return false;
            lock (_lock)
            {
                var existing = GetById(id);
                if (existing == null) return false;

                // update scalar properties, keep registrations
                existing.Name = updated.Name;
                existing.Date = updated.Date;
                existing.Location = updated.Location;

                // date may have changed; re-sort list to maintain ordering
                _events.Sort((a, b) => a.Date.CompareTo(b.Date));
                return true;
            }
        }

        public bool DeleteEvent(Guid id)
        {
            lock (_lock)
            {
                var existing = GetById(id);
                if (existing == null) return false;
                return _events.Remove(existing);
            }
        }

        public bool RemoveRegistration(Guid eventId, Guid registrationId)
        {
            var ev = GetById(eventId);
            if (ev == null) return false;
            var reg = ev.Registrations.FirstOrDefault(r => r.Id == registrationId);
            if (reg == null) return false;
            ev.Registrations.Remove(reg);
            return true;
        }

        public bool SetAttendance(Guid eventId, Guid registrationId, bool attended)
        {
            var ev = GetById(eventId);
            if (ev == null) return false;
            var reg = ev.Registrations.FirstOrDefault(r => r.Id == registrationId);
            if (reg == null) return false;
            reg.Attended = attended;
            return true;
        }

        public bool AddRegistration(Guid eventId, Registration reg)
        {
            if (reg == null) return false;
            lock (_lock)
            {
                var ev = GetById(eventId);
                if (ev == null) return false;
                ev.Registrations.Add(reg);
                return true;
            }
        }
    }
}
