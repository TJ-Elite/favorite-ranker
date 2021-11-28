// © 2021 Tuukka Junnikkala

namespace FavoriteRankerLibrary.Models
{
    public class ComparisonModel
    {
        public ComparisonModel(ushort id)
        {
            ID = id;
        }

        public ComparisonModel(ushort id, Relation comparison)
        {
            ID = id;
            Comparison = comparison;
        }

        public ushort ID { get; set; }
        public Relation Comparison { get; set; }
    }
}
