using System;

namespace MovieStreaming.Common.Actors
{
    [Serializable]
    internal class SimulatedTerribleMovieException : Exception
    {
        public string MovieTitle { get; private set; }

        public SimulatedTerribleMovieException(string movieTitle) : base($"{movieTitle} is a terrible movie")
        {
            MovieTitle = movieTitle;
        }
    }
}