using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProject_SuyeonJangB.Models
{
    public class Journey
    {
        //what discribes a journeys?
        //journey title, explain, destination
        [Key]
        public int JourneyId { get; set; }
        public string JourneyTitle { get; set; }
        public string JourneyExplain { get; set; }

        //An Journey belongs to one Traveler
        //A Travelers can have many Journeys
        [ForeignKey("Travelers")]
        public int TravelerId { get; set; }
        public virtual Traveler Travelers { get; set; }

        //an Journey can be taken care of by many Destinations
        public ICollection<Destination> Destinations { get; set; }
    }

    public class JourneyDto
    {
        public int JourneyId { get; set; }
        public string JourneyTitle { get; set; }
        public string JourneyExplain { get; set; }
        public int TravelerId { get; set; }
        public string TravelerFirstName { get; set; }

    }
}