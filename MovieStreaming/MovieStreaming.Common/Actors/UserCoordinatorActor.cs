using Akka.Actor;
using Akka.Event;
using MovieStreaming.Common.Messages;
using System;
using System.Collections.Generic;

namespace MovieStreaming.Common.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> _users;
        private ILoggingAdapter _logger = Context.GetLogger();

        public UserCoordinatorActor()
        {
            _users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(
                message =>
                {
                    CreateChildUserIfNotExists(message.UserId);
                    IActorRef childActorRef = _users[message.UserId];
                    childActorRef.Tell(message);
                });
            Receive<StopMovieMessage>(
                message =>
                {
                    CreateChildUserIfNotExists(message.UserId);
                    IActorRef childActorRef = _users[message.UserId];
                    childActorRef.Tell(message);
                });
        }

        private void CreateChildUserIfNotExists(int userId)
        {
            if (!_users.ContainsKey(userId))
            {
                var newChildActorRef = Context.ActorOf(Props.Create(() => new UserActor(userId)), $"User{userId}");
                _users.Add(userId, newChildActorRef);
                _logger.Debug($"UserCoordinatorActor created new child UserActor for {userId} (Total Users: {_users.Count})");
            }
        }

        protected override void PreStart()
        {
            _logger.Debug("UserCoordinatorActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("UserCoordinatorActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug($"UserCoordinatorActor PreRestart because: {reason}");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug($"UserCoordinatorActor PostRestart because: {reason}");
            base.PostRestart(reason);
        }
    }
}
