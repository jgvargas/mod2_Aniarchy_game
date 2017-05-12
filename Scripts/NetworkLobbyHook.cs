using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        Controller_Player localPlayer = gamePlayer.GetComponent<Controller_Player>();

        localPlayer.pname = lobby.playerName;
        localPlayer.playerMaterial = lobby.playerMaterial;
    }

}
