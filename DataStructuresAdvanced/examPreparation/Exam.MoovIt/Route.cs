using System.Collections.Generic;

namespace Exam.MoovIt
{
    public class Route
    {
        public string Id { get; set; }

        public double Distance { get; set; }

        public int Popularity { get; set; }

        public bool IsFavorite { get; set; }

        public List<string> LocationPoints { get; set; } = new List<string>();

        public Route(string id, double distance, int popularity, bool isFavorite, List<string> locationPoints)
        {
            this.Id = id;
            this.Distance = distance;
            this.Popularity = popularity;
            this.IsFavorite = isFavorite;
            this.LocationPoints = locationPoints;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Route;

            return this.Distance == other.Distance
                && this.LocationPoints[0] == other.LocationPoints[0]
                && this.LocationPoints[this.LocationPoints.Count - 1]
                == other.LocationPoints[other.LocationPoints.Count - 1];
        }

        public override int GetHashCode()
        {
            return this.Distance.GetHashCode() 
                * this.LocationPoints[0].GetHashCode()
                * this.LocationPoints[this.LocationPoints.Count - 1].GetHashCode();
        }
    }
} 
