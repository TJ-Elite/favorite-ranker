// © 2021 Tuukka Junnikkala

using System.Collections.Generic;

namespace FavoriteRankerLibrary.Models
{
    public class FavoriteModel
    {
        public FavoriteModel(ushort id)
        {
            ID = id;
        }

        public ushort ID { get; set; }
        public List<ComparisonModel> Comparisons { get; set; } = new List<ComparisonModel>();
    }
}
