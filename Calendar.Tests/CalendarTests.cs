using Calendar.Model.Entities;
using Calendar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Calendar.Tests
{
    public class CalendarTests
    {

        [Fact]
        public void CreateNotRepeatedEventSeries_ShouldReturnASingleEvent()
        {
            //Prepare
            var series = new EventSeries()
            {
                Description = "aa",
                FirstOccuranceStartDate = DateTime.Today,
                FirstOccurenceEndDate = DateTime.Today.AddDays(1),
                Location = "aa",
                RepeatPartOfTheYear = Model.Entities.RepeatPartOfTheYear.OneTime


            };
            var list = new List<EventSeries>();
            list.Add(series);

            //ACT
            var events = EventService.GetEventsForUsersEventSeries(list);
            //Assert
            Assert.Equal(1,events.Count());
        }
    }
}
