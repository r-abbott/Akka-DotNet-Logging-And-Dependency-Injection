using Akka.Actor;
using Akka.Event;
using MovieStreaming.Common.Messages;
using System;

namespace MovieStreaming.Common.Actors
{
    public class UserActor : ReceiveActor
    {
        private int _id;
        private string _currentlyWatching;
        private ILoggingAdapter _logger = Context.GetLogger();

        public UserActor(int userId)
        {
            _id = userId;
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message => _logger.Warning($"UserActor {_id} cannot start playing another movie before stopping existing one"));
            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());
            _logger.Info($"UserActor {_id} behavior has now become Playing");
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message => _logger.Warning($"UserActor {_id} cannot stop if nothing is playing"));
            _logger.Info($"UserActor {_id} behavior has now become Stopped");
        }

        private void StartPlayingMovie(string movieTitle)
        {
            _currentlyWatching = movieTitle;
            _logger.Info($"UserActor {_id} is currently watching '{_currentlyWatching}'");

            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(movieTitle));

            Become(Playing);
        }

        private void StopPlayingCurrentMovie()
        {
            _logger.Info($"UserActor {_id} has stopped watching '{_currentlyWatching}'");
            _currentlyWatching = null;
            Become(Stopped);
        }

        protected override void PreStart()
        {
            _logger.Debug($"UserActor {_id} PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug($"UserActor {_id} PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"UserActor {_id} PreRestart because: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"UserActor {_id} PostRestart because: {reason}");

            base.PostRestart(reason);
        }
    }
}
