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

        [Fact]
        public void CreateRepeatedEventseries_ShouldRepeatEventCorrentNumberOfTimes()
        {
            //Prepare
            var series = new EventSeries()
            {
                Description = "aa",
                FirstOccuranceStartDate = DateTime.Today,
                FirstOccurenceEndDate = DateTime.Today.AddDays(1),
                Location = "aa",
                RepeatPartOfTheYear = Model.Entities.RepeatPartOfTheYear.Month,
                NumerOfPartsOfYearToRepeat =1,
                MaxNumerOfOccurences=3,
                MonthRepeatType = Model.Enums.MonthRepeatType.DayOfTheMonth


            };
            var list = new List<EventSeries>();
            list.Add(series);

            //ACT
            var events = EventService.GetEventsForUsersEventSeries(list);
            //Assert
            Assert.Equal(3, events.Count());
        }

        [Fact]
        public void CreateEventeriesRepeatedOnAWeekDay_ShouldAllBeOnThisWeeDay()
        {
            //prepare
            var series = new EventSeries()
            {
                Description = "aa",
                FirstOccuranceStartDate = new DateTime(2015,03,9),
                FirstOccurenceEndDate = new DateTime(2015,03,10),
                Location = "aa",
                RepeatPartOfTheYear = Model.Entities.RepeatPartOfTheYear.Week,
                NumerOfPartsOfYearToRepeat = 1,
                RepeatWeekDays = new List<DayOfWeek>(){DayOfWeek.Monday}

            };
            var list = new List<EventSeries>();
            list.Add(series);

            //ACT
            var events = EventService.GetEventsForUsersEventSeries(list);
            //Assert
            Assert.Equal(1000, events.Count());
            foreach(var ev  in events)
            {
                Assert.Equal(DayOfWeek.Monday,ev.StartDate.DayOfWeek);
            }
        }
        [Fact]
        public void CreateEventSeries_ShoudReturnCorectEventsIfSetRepeatedPartsOfTheYearIsGreaterThanOne()
        {
            //prepare
            var series = new EventSeries()
            {
                Description = "aa",
                FirstOccuranceStartDate = DateTime.Today,
                FirstOccurenceEndDate = DateTime.Today.AddDays(1),
                Location = "aa",
                RepeatPartOfTheYear = Model.Entities.RepeatPartOfTheYear.Year,
                NumerOfPartsOfYearToRepeat = 2,
                MaxNumerOfOccurences = 5,
              

            };
            var list = new List<EventSeries>();
            list.Add(series);

            //ACT
            var events = EventService.GetEventsForUsersEventSeries(list);
            //Assert
            Assert.Equal(5, events.Count());
            for (int i = 1; i < events.Count(); i++)
            {
                Assert.Equal(2,events[i].StartDate.Year - events[i-1].StartDate.Year);
            }
        }
        [Fact]
        public void CreateEventSeries_ShoudReturnCorectEventsIfSetRepeatedPartsOfTheYearIsGreaterThanOneForDayRepeat()
        {
            //prepare
            var series = new EventSeries()
            {
                Description = "aa",
                FirstOccuranceStartDate = DateTime.Today,
                FirstOccurenceEndDate = DateTime.Today.AddDays(1),
                Location = "aa",
                RepeatPartOfTheYear = Model.Entities.RepeatPartOfTheYear.Day,
                NumerOfPartsOfYearToRepeat = 2,
                MaxNumerOfOccurences = 5,


            };
            var list = new List<EventSeries>();
            list.Add(series);

            //ACT
            var events = EventService.GetEventsForUsersEventSeries(list);
            //Assert
            Assert.Equal(5, events.Count());
            for (int i = 1; i < events.Count(); i++)
            {
                Assert.Equal(2, events[i].StartDate.Day - events[i-1].StartDate.Day);
            }
        }
    }
}
