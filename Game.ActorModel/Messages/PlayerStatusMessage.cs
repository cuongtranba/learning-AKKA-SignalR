using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ActorModel.Messages
{
    public class PlayerStatusMessage
    {
        public PlayerStatusMessage(string playerName, int health)
        {
            PlayerName = playerName;
            Health = health;
        }

        public string PlayerName { get; private set; }
        public int Health { get; private set; }
    }
}
