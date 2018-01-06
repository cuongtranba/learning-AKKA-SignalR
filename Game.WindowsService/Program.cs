using System;
using Akka.Actor;
using Game.ActorModel.Actors;
using Topshelf;

namespace Game.WindowsService
{
    public class GameStateService
    {
        private ActorSystem ActorSystemInstance;

        public void Start()
        {
            ActorSystemInstance = ActorSystem.Create("GameSystem");
            var gameController = ActorSystemInstance.ActorOf<GameControllerActor>("GameController");
        }

        public void Stop()
        {
            ActorSystemInstance.Terminate().Wait(TimeSpan.FromSeconds(2));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                
            {
                x.Service<GameStateService>(s =>                
                {
                    s.ConstructUsing(name => new GameStateService()); 
                    s.WhenStarted(tc => tc.Start());              
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Sample Topshelf Host");        
                x.SetDisplayName("AKKA.Game");                       
                x.SetServiceName("AKKA.Game");                       
            });                                                  
        }
    }
}
