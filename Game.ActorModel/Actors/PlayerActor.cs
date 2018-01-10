using Akka.Actor;
using Akka.Persistence;
using Game.ActorModel.Messages;

namespace Game.ActorModel.Actors
{
    public class PlayerActor : ReceivePersistentActor
    {
        public static Props Props(string playerName) => Akka.Actor.Props.Create(() => new PlayerActor(playerName));
        private readonly string _playerName;
        private int _health;

        public PlayerActor(string playerName)
        {
            _playerName = playerName;
            _health = 100;
            Command<AttackPlayerMessage>(message =>
            {
                Persist(message, playerMessage =>
                {
                    this._health -= 20;
                    Sender.Tell(new PlayerHealthChangedMessage(_playerName, _health));
                });
            });
            Command<RefreshPlayerStatusMessage>(message =>
            {
                Sender.Tell(new PlayerStatusMessage(_playerName, _health));
            });

            Recover<AttackPlayerMessage>(message =>
            {
                this._health -= 20;
            });
        }

        public override string PersistenceId => _playerName;
    }
}
