using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Match
    {
        [JsonProperty("fifa_id")]
        public string FifaId { get; set; }

        [JsonProperty("home_team")]
        public Team HomeTeam { get; set; } // ✅ Ensure correct mapping

        [JsonProperty("away_team")]
        public Team AwayTeam { get; set; }

        [JsonProperty("home_team_statistics")]
        public TeamStatistics HomeTeamStatistics { get; set; }

        [JsonProperty("away_team_statistics")]
        public TeamStatistics AwayTeamStatistics { get; set; }

        [JsonProperty("home_team_events")]
        public List<TeamEvents> HomeTeamEvents { get; set; }

        [JsonProperty("away_team_events")]
        public List<TeamEvents> AwayTeamEvents { get; set; }
        [JsonProperty("winner")]
        public string Winner { get; set; }



        public override string ToString()
        {
            return $"{HomeTeam} {AwayTeam}";
        }
    }
}
