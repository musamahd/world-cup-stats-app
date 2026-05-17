using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Team
    {
        
        public int Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        public string AlternateName { get; set; }

        [JsonProperty("fifa_code")]
        public string FIFA_Code { get; set; }

        public int Group_Id { get; set; }
        public string Group_Letter { get; set; }
        public object AverageAttendance { get; set; }

        public int MatchesPlayed { get; set; } = 0;

        [JsonProperty("goals")]
        public int GoalsScored { get; set; }

        [JsonProperty("penalties")]
        public int Penalties { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("draws")]
        public int Draws { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("goals_conceded")]
        public int GoalsConceded { get; set; }

        public int GoalDifference { get; set; }

        public override string ToString()
        {
            return $"{Country} {FIFA_Code}";
            ;
        }
    }
}
