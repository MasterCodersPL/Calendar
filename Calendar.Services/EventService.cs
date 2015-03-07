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
        private static int maxNumberOfOccurencesOfUnretrictedEvents = 1000;
        public static List<Event> GetEventsForUsersEventSeries(List<EventSeries> eventSeries)
        {
            var result = new List<Event>();
            foreach (EventSeries series in eventSeries)
            {
                int currentEventOccurances = 0;

                var firstEvent = new Event();
                firstEvent.Description = series.Description;
                firstEvent.Location = series.Location;
                firstEvent.StartDate = series.FirstOccuranceStartDate;
                firstEvent.EndDate = series.FirstOccurenceEndDate;
                firstEvent.EventSeriesId = series.Id;
                firstEvent.NotyfiactionInterval = series.NotyfiactionInterval;
                firstEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
                result.Add(firstEvent);
                currentEventOccurances++;
                DateTime previousEventStartDate = series.FirstOccuranceStartDate;
                DateTime previousEventEndDate = series.FirstOccurenceEndDate;
                TimeSpan eventDuration = series.FirstOccurenceEndDate - series.FirstOccuranceStartDate;
                if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Day)
                {

                    if (series.MaxNumerOfOccurences.HasValue)
                    {
                        while (currentEventOccurances < series.MaxNumerOfOccurences.Value)
                        {
                            var nextEvent = CreateNextDaylyRepeatedEvent(result, series, previousEventStartDate, previousEventEndDate);
                            currentEventOccurances++;
                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }
                    }
                    else if (series.LastOccurenceDate.HasValue)
                    {
                        while (previousEventStartDate < series.LastOccurenceDate.Value)
                        {
                            var nextEvent = CreateNextDaylyRepeatedEvent(result, series, previousEventStartDate, previousEventEndDate);

                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }
                    }
                    else
                    {
                        while (currentEventOccurances < maxNumberOfOccurencesOfUnretrictedEvents)
                        {
                            var nextEvent = CreateNextDaylyRepeatedEvent(result, series, previousEventStartDate, previousEventEndDate);
                            currentEventOccurances++;
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
                            while (currentEventOccurances < series.MaxNumerOfOccurences.Value)
                            {
                             

                                CreateNextMonthRepeatedEvent(result, series, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration);
                                currentEventOccurances++;
                            }
                        }
                        else if (series.LastOccurenceDate.HasValue)
                        {
                            while (previousEventStartDate < series.LastOccurenceDate.Value)
                            {
                                CreateNextMonthRepeatedEvent(result, series, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration);
                              

                            }
                        }

                        else
                        {
                            while (currentEventOccurances < maxNumberOfOccurencesOfUnretrictedEvents)
                            {

                                CreateNextMonthRepeatedEvent(result, series, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration);

                            }
                        }
                    }
                    else
                    {
                        while(currentEventOccurances < maxNumberOfOccurencesOfUnretrictedEvents)
                        {
                            CreateNextMonthRepeatedEvent(result, series, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration);
                            currentEventOccurances++;
                        }
                    }

                }

                else if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Week)
                {
                    if (series.RepeatWeekDays.Count() == 0)
                    {
                        while (currentEventOccurances < maxNumberOfOccurencesOfUnretrictedEvents)
                        {
                            var nextEvent = CreateNextWeeklyRepeatedEvent(result, series, previousEventStartDate, previousEventEndDate);
                            currentEventOccurances++;
                            previousEventStartDate = nextEvent.StartDate;
                            previousEventEndDate = nextEvent.EndDate;
                        }
                    }
                    else
                    {


                        if (series.MaxNumerOfOccurences.HasValue)
                        {
                            while (currentEventOccurances < series.MaxNumerOfOccurences.Value)
                            {
                                foreach (DayOfWeek day in series.RepeatWeekDays)
                                {
                                    CreateWeekdayRepeatedEvent(result, series, ref currentEventOccurances, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration, day);

                                }
                            }
                        }
                        else if (series.LastOccurenceDate.HasValue)
                        {
                            while (previousEventStartDate < series.LastOccurenceDate.Value)
                            {
                                foreach (DayOfWeek day in series.RepeatWeekDays)
                                {

                                    CreateWeekdayRepeatedEvent(result, series, ref currentEventOccurances, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration, day);

                                }
                            }
                        }

                        else
                        {
                            while (currentEventOccurances < maxNumberOfOccurencesOfUnretrictedEvents)
                            {

                                foreach (DayOfWeek day in series.RepeatWeekDays)
                                {

                                    CreateWeekdayRepeatedEvent(result, series, ref currentEventOccurances, ref previousEventStartDate, ref previousEventEndDate, ref eventDuration, day);

                                }
                            }
                        }
                    }
                }
                else if (series.RepeatPartOfTheYear == RepeatPartOfTheYear.Year)
                {

                    if (series.MaxNumerOfOccurences.HasValue)
                    {
                        while (currentEventOccurances < series.MaxNumerOfOccurences.Value)
                        {
                            CreateNextYearRepeatedEvent(result, series, ref currentEventOccurances, ref previousEventStartDate, ref previousEventEndDate);
                        }
                    }
                    else if (series.LastOccurenceDate.HasValue)
                    {
                        while (previousEventStartDate < series.LastOccurenceDate.Value)
                        {
                            CreateNextYearRepeatedEvent(result, series, ref currentEventOccurances, ref previousEventStartDate, ref previousEventEndDate);
                        }
                    }

                    else
                    {
                        while (currentEventOccurances < maxNumberOfOccurencesOfUnretrictedEvents)
                        {
                            CreateNextYearRepeatedEvent(result, series, ref currentEventOccurances, ref previousEventStartDate, ref previousEventEndDate);
                          
                        }
                    }
                }


            }

            return result;
        }

        private static void CreateNextYearRepeatedEvent(List<Event> result, EventSeries series, ref int currentOccurences, ref DateTime previousEventStartDate, ref DateTime previousEventEndDate)
        {
            var nextEvent = new Event();
            nextEvent.Description = series.Description;
            nextEvent.Location = series.Location;
            nextEvent.StartDate = previousEventStartDate.AddYears(series.NumerOfPartsOfYearToRepeat);
            nextEvent.EndDate = previousEventEndDate.AddYears(series.NumerOfPartsOfYearToRepeat);
            nextEvent.EventSeriesId = series.Id;
            nextEvent.NotyfiactionInterval = series.NotyfiactionInterval;
            nextEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
            result.Add(nextEvent);
            previousEventStartDate = nextEvent.StartDate;
            previousEventEndDate = nextEvent.EndDate;
            currentOccurences++;
        }

        private static void CreateNextMonthRepeatedEvent(List<Event> result, EventSeries series, ref DateTime previousEventStartDate, ref DateTime previousEventEndDate, ref TimeSpan eventDuration)
        {
            DateTime nextEventStartDate = previousEventStartDate.AddMonths(series.NumerOfPartsOfYearToRepeat);
            if (previousEventStartDate.Day != nextEventStartDate.Day)
            {
                nextEventStartDate = new DateTime(nextEventStartDate.Year, nextEventStartDate.Month, previousEventStartDate.Day, previousEventStartDate.Hour, previousEventStartDate.Minute, previousEventStartDate.Second);
            }
            DateTime nextEventEndDate = nextEventStartDate + eventDuration;

            var nextEvent = new Event();
            nextEvent.Description = series.Description;
            nextEvent.Location = series.Location;
            nextEvent.StartDate = nextEventStartDate;
            nextEvent.EndDate = nextEventEndDate;
            nextEvent.EventSeriesId = series.Id;
            nextEvent.NotyfiactionInterval = series.NotyfiactionInterval;
            nextEvent.NumerOfNotyfiationIntervals = series.NumerOfNotyfiationIntervals;
            result.Add(nextEvent);
            previousEventStartDate = nextEvent.StartDate;
            previousEventEndDate = nextEvent.EndDate;
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



        private static Event CreateNextDaylyRepeatedEvent(List<Event> result, EventSeries series, DateTime previousEventStartDate, DateTime previousEventEndDate)
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
        private static Event CreateNextWeeklyRepeatedEvent(List<Event> result, EventSeries series, DateTime previousEventStartDate, DateTime previousEventEndDate)
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
