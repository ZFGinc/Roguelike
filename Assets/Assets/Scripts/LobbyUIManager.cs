using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    // ������ �� UI ��������
    public Button createLobbyButton;
    public Button joinLobbyButton;
    public InputField lobbyIdInputField;
    public Text statusText;

    private CSteamID currentLobby;

    private void Start()
    {
        // ������������� Steam API
        if (!SteamClient.IsValid)
        {
            Debug.LogError("Steamworks is not initialized!");
            return;
        }

        // �������� ������� � �������
        createLobbyButton.onClick.AddListener(CreateLobby);
        joinLobbyButton.onClick.AddListener(JoinLobby);

        // �������� �� ������� Steam
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
    }

    private void CreateLobby()
    {
        // ������� ����� � ������������ ����������� ������� (4)
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        UpdateStatus("Creating lobby...");
    }

    private void JoinLobby()
    {
        // �������� ID ����� �� ���������� ����
        if (string.IsNullOrEmpty(lobbyIdInputField.text))
        {
            UpdateStatus("Error: Enter a valid Lobby ID.");
            return;
        }

        ulong lobbyId = ulong.Parse(lobbyIdInputField.text);
        SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
        UpdateStatus("Joining lobby...");
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            currentLobby = new CSteamID(callback.m_ulSteamIDLobby);
            UpdateStatus($"Lobby created successfully! ID: {currentLobby}");
        }
        else
        {
            UpdateStatus("Failed to create lobby.");
        }
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (callback.m_EChatRoomEnterResponse == (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            currentLobby = new CSteamID(callback.m_ulSteamIDLobby);
            UpdateStatus($"Joined lobby successfully! ID: {currentLobby}");
        }
        else
        {
            UpdateStatus("Failed to join lobby.");
        }
    }

    private void UpdateStatus(string message)
    {
        statusText.text = message;
        Debug.Log(message);
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
    }
}