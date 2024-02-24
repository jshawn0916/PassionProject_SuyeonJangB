using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_SuyeonJangB.Models.ViewModels
{
    public class DetailsJourney
    {
        public JourneyDto SelectedJourney { get; set; }
        public IEnumerable<DestinationDto> JourneyDestinations { get; set; }
    }
}