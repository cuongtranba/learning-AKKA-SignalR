using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using Game.ActorModel.Messages;

namespace Game.ActorModel.Actors
{
    public class GameControllerActor : ReceivePersistentActor
    {
        public static Props Props() => Akka.Actor.Props.Create(() => new GameControllerActor());
        private readonly Dictionary<string, IActorRef> _players;
        private readonly List<PlayerProfile> _playerProfiles;

        public GameControllerActor()
        {
            _players = new Dictionary<string, IActorRef>();
            _playerProfiles = new List<PlayerProfile>();
            Command<JoinGameMessage>(message =>
            {
                Persist(message, gameMessage =>
                {
                    JoinGame(message);
                });
            });


            Recover<JoinGameMessage>(message =>
            {
                JoinGame(message);
            });

            Command<AttackPlayerMessage>(message =>
            {
                Persist(message, playerMessage =>
                {
                    if (_players.ContainsKey(message.PlayerName))
                    {
                        _players[message.PlayerName].Forward(message);
                    }
                });
            });

            Recover<AttackPlayerMessage>(message =>
            {
                if (_players.ContainsKey(message.PlayerName))
                {
                    _players[message.PlayerName].Forward(message);
                }
            });

            Command<GetPlayerExistedMessage>(message =>
            {
                Sender.Tell(new PlayerExistedMessage(_playerProfiles));
            });

            Command<PlayerHealthChangedMessage>(message =>
            {
                Persist(message, changedMessage =>
                {
                    if (message.Health <= 0)
                    {
                        _players[message.PlayerName].Tell(PoisonPill.Instance);
                        _players.Remove(message.PlayerName);

                        var playerName = _playerProfiles.FirstOrDefault(c => c.Name == message.PlayerName);
                        _playerProfiles.Remove(playerName);

                    }
                    else
                    {
                        var player = _playerProfiles.FirstOrDefault(c => c.Name == message.PlayerName);
                        if (player != null)
                        {
                            player.Health = message.Health;
                        }
                    }
                });
            });

            Recover<PlayerHealthChangedMessage>(message =>
            {
                if (message.Health <= 0)
                {
                    _players[message.PlayerName].Tell(PoisonPill.Instance);
                    _players.Remove(message.PlayerName);

                    var playerName = _playerProfiles.FirstOrDefault(c => c.Name == message.PlayerName);
                    _playerProfiles.Remove(playerName);

                }
                else
                {
                    var player = _playerProfiles.FirstOrDefault(c => c.Name == message.PlayerName);
                    if (player != null)
                    {
                        player.Health = message.Health;
                    }
                }
            });

        }

        private void JoinGame(JoinGameMessage message)
        {
            var playerNeedsCreating = !_players.ContainsKey(message.PlayerName);
            if (playerNeedsCreating)
            {
                _players.Add(message.PlayerName, Context.ActorOf(PlayerActor.Props(message.PlayerName), message.PlayerName));
                _playerProfiles.Add(new PlayerProfile(100, message.PlayerName));
                foreach (var player in _players.Values)
                {
                    player.Tell(new RefreshPlayerStatusMessage(), Sender);
                }
            }
        }

        public override string PersistenceId => "GameControllerActor";
    }
}
