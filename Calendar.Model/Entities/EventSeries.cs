using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar.Model.Enums;

namespace Calendar.Model.Entities
{
    public class EventSeries
    {
        public int Id { get; set; }

        public DateTime FirstOccuranceStartDate { get; set; }

        public DateTime FirstOccurenceEndDate { get; set; }

        public List<DayOfWeek> RepeatWeekDays { get; set; }


        public RepeatPartOfTheYear RepeatPartOfTheYear { get; set; }

        public int NumerOfPartsOfYearToRepeat { get; set; }

        public MonthRepeatType MonthRepeatType { get; set; }
        public DateTime? FirstOccurenceDate { get; set; }

        public DateTime StartDate { get; set; }

        public int? MaxNumerOfOccurences { get; set; }

        public DateTime? LastOccurenceDate { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public NotyfiactionInterval NotyfiactionInterval{ get; set; }

        public int NumerOfNotyfiationIntervals { get; set; }


    }
}
