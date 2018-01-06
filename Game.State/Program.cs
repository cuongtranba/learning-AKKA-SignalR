using System;
using Akka.Actor;
using Game.ActorModel.Actors;

namespace Game.State
{
    class Program
    {
        private static ActorSystem actorSystem;
        static void Main(string[] args)
        {
            actorSystem = ActorSystem.Create("GameSystem");
            var gamecontroller = actorSystem.ActorOf<GameControllerActor>("GameController");
            Console.ReadLine();
        }
    }
}
