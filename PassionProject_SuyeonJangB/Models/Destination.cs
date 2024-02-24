using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_SuyeonJangB.Models
{
    public class Destination
    {
        //what discribes a destination?
        //name, category, location
        [Key]
        public int DestinationId { get; set; }

        public string DestinationName { get; set; }

        public string DestinationCategory { get; set; }

        public string DestinationLocation { get; set; }

        //an Destination can be taken care of by many Journeys
        public ICollection<Journey> Journeys { get; set; }
    }
    public class DestinationDto
    {
        public int DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string DestinationCategory { get; set; }
        public string DestinationLocation { get; set; }
    }
}