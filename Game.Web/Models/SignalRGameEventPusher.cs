﻿using System.Collections.Generic;
using Game.ActorModel.External;
using Game.ActorModel.Messages;
using Microsoft.AspNet.SignalR;

namespace Game.Web.Models
{
    public class SignalRGameEventPusher:IGameEventsPusher
    {
        public static readonly IHubContext _gameHubContext;

        static SignalRGameEventPusher()
        {
            _gameHubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
        }

        public void PlayerJoined(string playerName, int playerHealth)
        {
            _gameHubContext.Clients.All.playerJoined(playerName, playerHealth);
        }

        public void UpdatePlayerHealth(string playerName, int playerHealth)
        {
            _gameHubContext.Clients.All.updatePlayerHealth(playerName, playerHealth);
        }

        public void ShowExistedPlayers(List<PlayerProfile> players)
        {
            _gameHubContext.Clients.All.ShowExistedPlayers(players);
        }
    }
}