using Calendar.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Services
{
    public class EventService
    {
        private static int maxOccurenceOfUnretrictedEvents = 1000;
        public static List<Event> GetEventsForUsersEventSeries(List<EventSeries> eventSeries)
        {
            var result = new List<Event>();
            foreach (EventSeries series in eventSeries)
            {
                int currentOccurences = 0;

                var firstEvent = new Event();
                firstEvent.Description = series.Description;
                firstEvent.Location = series.Location;
                firstEvent.StartDate = series.FirstOccuranceStartDate;
                firstEvent.EndDate = series.FirstOccurenceEndDate;
                firstEvent.EventSeriesId = series.Id;
                firstEvent.NotyfiactionInterval = series.NotyfiactionInterval;
                firstEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
                result.Add(firstEvent);
                currentOccurences++;
                DateTime previousEventStartDate = series.FirstOccuranceStartDate;
                DateTime previousEventEndDate = series.FirstOccurenceEndDate;
                TimeSpan eventDuration = series.FirstOccurenceEndDate - series.FirstOccuranceStartDate;
                if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Day)
                {

                    if (series.MaxNumerOfOccurences.HasValue)
                    {
                        while (currentOccurences < series.MaxNumerOfOccurences.Value)
                        {
                            var nextEvent = CreateNextDayEvent(result, series, previousEventStartDate, previousEventEndDate);
                            currentOccurences++;
                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }
                    }
                    else if (series.LastOccurenceDate.HasValue)
                    {
                        while (previousEventStartDate < series.LastOccurenceDate.Value)
                        {
                            var nextEvent = CreateNextDayEvent(result, series, previousEventStartDate, previousEventEndDate);

                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }
                    }
                    else
                    {
                        while (currentOccurences < maxOccurenceOfUnretrictedEvents)
                        {
                            var nextEvent = CreateNextDayEvent(result, series, previousEventStartDate, previousEventEndDate);
                            currentOccurences++;
                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }


                    }
                }
                else if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Month)
                {
                    if (series.MonthRepeatType == Model.Enums.MonthRepeatType.DayOfTheMonth)
                    {
                        if (series.MaxNumerOfOccurences.HasValue)
                        {
                            while (currentOccurences < series.MaxNumerOfOccurences.Value)
                            {
                                //DateTime nextEventStartDate= new DateTime()
                            }
                        }
                        else if (series.LastOccurenceDate.HasValue)
                        {
                            while (previousEventStartDate < series.LastOccurenceDate.Value)
                            {

                            }
                        }

                        else
                        {
                            while (currentOccurences < maxOccurenceOfUnretrictedEvents)
                            {


                            }
                        }
                    }
                    else
                    {

                    }

                }

                else if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Week)
                {
                    if (series.RepeatWeekDays.Count() == 0)
                    {
                        while (currentOccurences < maxOccurenceOfUnretrictedEvents)
                        {
                            var nextEvent = CreateNextWeekEvemt(result, series, previousEventStartDate, previousEventEndDate);
                            currentOccurences++;
                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }
                    }
                    else
                    {


                        if (series.MaxNumerOfOccurences.HasValue)
                        {
                            while (currentOccurences < series.MaxNumerOfOccurences.Value)
                            {
                                foreach (DayOfWeek day in series.RepeatWeekDays)
                                {
                                    CreateWeekdayRepeatedEvent(result, series, ref currentOccurences, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration, day);

                                }
                            }
                        }
                        else if (series.LastOccurenceDate.HasValue)
                        {
                            while (previousEventStartDate < series.LastOccurenceDate.Value)
                            {
                                foreach (DayOfWeek day in series.RepeatWeekDays)
                                {

                                    CreateWeekdayRepeatedEvent(result, series, ref currentOccurences, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration, day);

                                }
                            }
                        }

                        else
                        {
                            while (currentOccurences < maxOccurenceOfUnretrictedEvents)
                            {

                                foreach (DayOfWeek day in series.RepeatWeekDays)
                                {

                                    CreateWeekdayRepeatedEvent(result, series, ref currentOccurences, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration, day);

                                }
                            }
                        }
                    }
                }
                else if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Year)
                {

                }


            }

            return result;
        }

        private static void CreateWeekdayRepeatedEvent(List<Event> result, EventSeries series, ref int currentOccurences, ref DateTime previousEventStartDate, ref DateTime previousEventEndDate, ref TimeSpan duration, DayOfWeek day)
        {
            var nextEvent = new Event();
            nextEvent.Description = series.Description;
            nextEvent.Location = series.Location;

            nextEvent.EventSeriesId = series.Id;
            nextEvent.NotyfiactionInterval = series.NotyfiactionInterval;
            nextEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
            currentOccurences++;
            DateTime nextEventStartDate = GetNextWeekday(previousEventStartDate, day);
            DateTime nextEventEndDate = nextEventStartDate + duration;
            nextEvent.StartDate = nextEventStartDate;
            nextEvent.EndDate = nextEventEndDate;
            previousEventStartDate = nextEventStartDate;
            previousEventEndDate = nextEventEndDate;
            result.Add(nextEvent);
        }



        private static Event CreateNextDayEvent(List<Event> result, EventSeries series, DateTime previousEventStartDate, DateTime previousEventEndDate)
        {
            var nextEvent = new Event();
            nextEvent.Description = series.Description;
            nextEvent.Location = series.Location;
            nextEvent.StartDate = previousEventStartDate.AddDays(series.NumerOfPartsOfYearToRepeat);
            nextEvent.EndDate = previousEventEndDate.AddDays(series.NumerOfPartsOfYearToRepeat);
            nextEvent.EventSeriesId = series.Id;
            nextEvent.NotyfiactionInterval = series.NotyfiactionInterval;
            nextEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
            result.Add(nextEvent);
            return nextEvent;
        }
        private static Event CreateNextWeekEvemt(List<Event> result, EventSeries series, DateTime previousEventStartDate, DateTime previousEventEndDate)
        {
            var nextEvent = new Event();
            nextEvent.Description = series.Description;
            nextEvent.Location = series.Location;
            nextEvent.StartDate = previousEventStartDate.AddDays(series.NumerOfPartsOfYearToRepeat * 7);
            nextEvent.EndDate = previousEventEndDate.AddDays(series.NumerOfPartsOfYearToRepeat * 7);
            nextEvent.EventSeriesId = series.Id;
            nextEvent.NotyfiactionInterval = series.NotyfiactionInterval;
            nextEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
            result.Add(nextEvent);
            return nextEvent;
        }
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }


    }
}
