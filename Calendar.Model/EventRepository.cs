using Calendar.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    public class EventRepository
    {
        public List<Event> GetEvents(DateTime dateFrom,DateTime dateTo,int userId)
        {
                var allEventsInTheSystem = new List<Event>();
            var usersEvents = allEventsInTheSystem.Where(x => x.UserId == userId);


            return usersEvents.Where(x => x.StartDate >= dateFrom && x.StartDate <= dateTo).ToList();
        
        }
    }
}
