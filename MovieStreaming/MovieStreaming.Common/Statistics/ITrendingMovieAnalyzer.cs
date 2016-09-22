using System.Collections.Generic;

namespace MovieStreaming.Common.Statistics
{
    public interface ITrendingMovieAnalyzer
    {
        string CalculateMostPopularMovie(IEnumerable<string> movieTitles);
    }
}