using System.Collections.Generic;
using System.Linq;

namespace MovieStreaming.Common.Statistics
{
    public class SimpleTrendingMovieAnalyzer : ITrendingMovieAnalyzer
    {
        public SimpleTrendingMovieAnalyzer()
        {
        }

        public string CalculateMostPopularMovie(IEnumerable<string> movieTitles)
        {
            var movieCounts = movieTitles.GroupBy(title => title,
                (key, values) => new { MovieTitle = key, PlayCount = values.Count() });

            return movieCounts.OrderByDescending(x => x.PlayCount).First().MovieTitle;
        }
    }
}