using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using System;

namespace MovieStreaming.Common.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
            Context.ActorOf(Context.DI().Props<TrendingMoviesActor>(), "TrendingMovies");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                exception =>
                {
                    if (exception is ActorInitializationException)
                    {
                        _logger.Error(exception, "PlaybackStatisticsActor supervisor strategy stopping child due to ActorInitializationException");
                        return Directive.Restart;
                    }
                    if (exception is SimulatedTerribleMovieException)
                    {
                        var terribleMovieEx = (SimulatedTerribleMovieException)exception;
                        _logger.Warning($"PlaybackStatisticsActor supervisor strategy resuming child due to terrible movie '{terribleMovieEx.MovieTitle}'.");
                        return Directive.Resume;
                    }

                    _logger.Error(exception, "PlaybackStatisticsActor supervisor strategy restarting child due to unexpected exception");
                    return Directive.Restart;
                }
                );
        }

        protected override void PreStart()
        {
            _logger.Debug("PlaybackStatisticsActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("PlaybackStatisticsActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"PlaybackStatisticsActor PreRestart because: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"PlaybackStatisticsActor PostRestart because: {reason}");

            base.PostRestart(reason);
        }
    }
}
