using Steamworks;
using UnityEngine;

public class SteamLobbyManager : MonoBehaviour
{
    private CSteamID currentLobby;

    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
    }

    public void JoinLobby(CSteamID lobbyId)
    {
        SteamMatchmaking.JoinLobby(lobbyId);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            currentLobby = new CSteamID(callback.m_ulSteamIDLobby);
            Debug.Log("Lobby created: " + currentLobby);
        }
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (callback.m_EChatRoomEnterResponse == (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            Debug.Log("Joined lobby: " + callback.m_ulSteamIDLobby);
        }
    }
}