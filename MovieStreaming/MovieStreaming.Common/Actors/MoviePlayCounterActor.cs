using Akka.Actor;
using System.Collections.Generic;
using System;
using MovieStreaming.Common.Messages;
using Akka.Event;

namespace MovieStreaming.Common.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private Dictionary<string, int> _moviePlayCounts;
        private ILoggingAdapter _logger = Context.GetLogger();

        public MoviePlayCounterActor()
        {
            _moviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if (!_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts.Add(message.MovieTitle, 0);
            }
            _moviePlayCounts[message.MovieTitle]++;

            // Simulated bugs
            if(message.MovieTitle == "Partial Recoil")
            {
                throw new SimulatedTerribleMovieException(message.MovieTitle);
            }

            if(message.MovieTitle == "Partial Recoil 2")
            {
                throw new InvalidOperationException("Simulated exception");
            }

            _logger.Debug(
                $"MoviePlayerCounterActor '{message.MovieTitle}' has been watched {_moviePlayCounts[message.MovieTitle]} times");
        }

        protected override void PreStart()
        {
            _logger.Debug("MoviePlayCounterActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("MoviePlayCounterActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"MoviePlayCounterActor PreRestart because: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"MoviePlayCounterActor PostRestart because: {reason}");

            base.PostRestart(reason);
        }
    }
}