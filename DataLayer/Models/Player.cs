using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Player
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shirt_number")]
        public int ShirtNumber { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        public string ImagePath { get; set; }

        public bool IsCaptain { get; set; }
        public bool IsFavorite { get; set; }=false;

        [JsonProperty("goals")]
        public int Goals { get; set; } = 0;

        [JsonProperty("yellow_cards")]
        public int YellowCards { get; set; } = 0;
      

        public override string ToString()
        {
            return $"{Name}";
        }
    }
    
}
