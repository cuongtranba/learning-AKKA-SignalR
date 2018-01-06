using System.Collections.Generic;

namespace Game.ActorModel.Messages
{
    public class PlayerExistedMessage
    {
        public PlayerExistedMessage(List<PlayerProfile> playerNames)
        {
            PlayerNames = playerNames;
        }

        public List<PlayerProfile> PlayerNames { get; private set; }
    }

    public class PlayerProfile
    {
        public PlayerProfile(int health, string name)
        {
            Health = health;
            Name = name;
        }

        public int Health { get; set; }
        public string Name { get; set; }
    }
}
