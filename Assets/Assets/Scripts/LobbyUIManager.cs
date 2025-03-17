using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    // Ссылки на UI элементы
    public Button createLobbyButton;
    public Button joinLobbyButton;
    public InputField lobbyIdInputField;
    public Text statusText;

    private CSteamID currentLobby;

    private void Start()
    {
        // Инициализация Steam API
        if (!SteamClient.IsValid)
        {
            Debug.LogError("Steamworks is not initialized!");
            return;
        }

        // Привязка событий к кнопкам
        createLobbyButton.onClick.AddListener(CreateLobby);
        joinLobbyButton.onClick.AddListener(JoinLobby);

        // Подписка на события Steam
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
    }

    private void CreateLobby()
    {
        // Создаем лобби с максимальным количеством игроков (4)
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        UpdateStatus("Creating lobby...");
    }

    private void JoinLobby()
    {
        // Получаем ID лобби из текстового поля
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
        // Отписываемся от событий при уничтожении объекта
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
    }
}