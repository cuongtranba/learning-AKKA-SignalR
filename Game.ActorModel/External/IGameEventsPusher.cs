using System.Collections.Generic;
using Game.ActorModel.Messages;

namespace Game.ActorModel.External
{
    public interface IGameEventsPusher
    {
        void PlayerJoined(string playerName, int playerHealth);
        void UpdatePlayerHealth(string playerName, int playerHealth);
        void ShowExistedPlayers(List<PlayerProfile> players);
    }
}
