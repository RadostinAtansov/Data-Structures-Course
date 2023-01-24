using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        HashSet<Route> routes = new HashSet<Route>();

        public int Count => this.routes.Count;

        public void AddRoute(Route route)
        {
            if (this.routes.Contains(route))
            {
                throw new ArgumentException();
            }
            this.routes.Add(route);
        }

        public void ChooseRoute(string routeId)
        {
            var route = this.routes.Where(r => r.Id == routeId).FirstOrDefault(); // might be slow
            if (route == null)
            {
                throw new ArgumentException();
            }
            route.Popularity++;

        }

        public bool Contains(Route route)
        {
            return this.routes.Contains(route);
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
        {
            return routes.Where(r => r.IsFavorite && r.LocationPoints.IndexOf(destinationPoint) >= 1)
                .OrderBy(r => r.Distance)
                .ThenByDescending(r => r.Popularity);
        }

        public Route GetRoute(string routeId)
        {
            var route = this.routes.Where(r => r.Id == routeId).FirstOrDefault(); // might be slow
            if (route == null)
            {
                throw new ArgumentException();
            }
            return route;
        }

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
        {
            return this.routes
                .OrderByDescending(r => r.Popularity)
                .ThenBy(r => r.Distance)
                .ThenBy(r => r.LocationPoints.Count)
                .Take(5);
        }

        public void RemoveRoute(string routeId)
        {
            var route = this.routes.Where(r => r.Id == routeId).FirstOrDefault(); // might be slow
            if (route == null)
            {
                throw new ArgumentException();
            }

            this.routes.Remove(route);
        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
        {
            return this.routes
            .Where(r => r.LocationPoints.Contains(startPoint)
                     && r.LocationPoints.Contains(endPoint)
                     && r.LocationPoints.IndexOf(startPoint) < r.LocationPoints.IndexOf(endPoint))
            .OrderBy(r => r.IsFavorite)
            .ThenBy(r => r.LocationPoints.IndexOf(endPoint) - r.LocationPoints.IndexOf(startPoint))
            .ThenByDescending(r => r.Popularity);
        }
    }
}
