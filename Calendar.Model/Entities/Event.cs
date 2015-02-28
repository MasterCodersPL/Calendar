using Calendar.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model.Entities
{
    public class Event
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EventSeriesId { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }



        public NotyfiactionInterval NotyfiactionInterval { get; set; }

        public int NumerOfNotyfiationIntervals { get; set; }

    }
}
