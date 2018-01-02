using Akka.Actor;
using Game.ActorModel.Messages;

namespace Game.ActorModel.Actors
{
    public class PlayerActor : ReceiveActor
    {
        public static Props Props(string playerName) => Akka.Actor.Props.Create(() => new PlayerActor(playerName));
        private readonly string _playerName;
        private int _health;

        public PlayerActor(string playerName)
        {
            _playerName = playerName;
            _health = 100;
            Receive<AttackPlayerMessage>(message =>
            {
                this._health -= 20;
                Sender.Tell(new PlayerHealthChangedMessage(_playerName, _health));
            });
            Receive<RefreshPlayerStatusMessage>(message =>
            {
                Sender.Tell(new PlayerStatusMessage(_playerName, _health));
            });
        }
    }
}
