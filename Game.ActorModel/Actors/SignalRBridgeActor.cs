using Akka.Actor;
using Game.ActorModel.External;
using Game.ActorModel.Messages;

namespace Game.ActorModel.Actors
{
    public class SignalRBridgeActor : ReceiveActor
    {
        private IGameEventsPusher _gameEventsPusher;
        private IActorRef _gameController;

        public static Props Props(IGameEventsPusher gameEventsPusher, IActorRef gameController) => Akka.Actor.Props.Create(() => new SignalRBridgeActor(gameEventsPusher, gameController));

        public SignalRBridgeActor(IGameEventsPusher gameEventsPusher, IActorRef gameController)
        {
            _gameEventsPusher = gameEventsPusher;
            _gameController = gameController;

            Receive<JoinGameMessage>(message =>
            {
                _gameController.Tell(message);
            });

            Receive<AttackPlayerMessage>(message =>
            {
                _gameController.Tell(new AttackPlayerMessage(message.PlayerName));
            });

            Receive<PlayerStatusMessage>(message =>
            {
                _gameEventsPusher.PlayerJoined(message.PlayerName, message.Health);
            });

            Receive<PlayerHealthChangedMessage>(message =>
            {
                _gameEventsPusher.UpdatePlayerHealth(message.PlayerName, message.Health);
            });
        }
    }
}
