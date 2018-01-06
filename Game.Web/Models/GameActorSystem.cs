using System;
using Akka.Actor;
using Game.ActorModel.Actors;
using Game.ActorModel.External;

namespace Game.Web.Models
{
    public static class GameActorSystem
    {
        private static ActorSystem ActorSystem;
        private static IGameEventsPusher _gameEventsPusher;
        public static void Create()
        {
            _gameEventsPusher = new SignalRGameEventPusher();

            ActorSystem = Akka.Actor.ActorSystem.Create("GameSystem");
            ActorReferences.GameController = ActorSystem.ActorSelection("akka.tcp://GameSystem@127.0.0.1:8091/user/GameController").ResolveOne(TimeSpan.FromSeconds(3)).Result;


            ActorReferences.SignalRBridge = ActorSystem.ActorOf(SignalRBridgeActor.Props(_gameEventsPusher, ActorReferences.GameController), "SignalRBridge");
        }

        public static void ShutDown()
        {
            ActorSystem.Terminate().Wait(1000);//1 min
        }

        public static class ActorReferences
        {
            public static IActorRef GameController { get; set; }
            public static IActorRef SignalRBridge { get; set; }
        }
    }
}