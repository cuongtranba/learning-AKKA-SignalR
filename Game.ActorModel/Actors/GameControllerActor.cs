using System.Collections.Generic;
using Akka.Actor;
using Game.ActorModel.Messages;

namespace Game.ActorModel.Actors
{
    public class GameControllerActor : ReceiveActor
    {
        public static Props Props() => Akka.Actor.Props.Create(() => new GameControllerActor());
        private readonly Dictionary<string, IActorRef> _players;

        public GameControllerActor()
        {
            _players = new Dictionary<string, IActorRef>();
            Receive<JoinGameMessage>(message =>
            {
                JoinGame(message);
            });

            Receive<AttackPlayerMessage>(message =>
            {
                _players[message.PlayerName].Forward(message);
            });
        }

        private void JoinGame(JoinGameMessage message)
        {
            var playerNeedsCreating = !_players.ContainsKey(message.PlayerName);
            if (playerNeedsCreating)
            {
                _players.Add(message.PlayerName, Context.ActorOf(PlayerActor.Props(message.PlayerName),message.PlayerName));

                foreach (var player in _players.Values)
                {
                    player.Tell(new RefreshPlayerStatusMessage(),Sender);
                }
            }
        }
    }
}
