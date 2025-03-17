using Mirror;
using Steamworks;

public class SteamTransport : TelepathyTransport
{
    public override void StartClient()
    {
        base.StartClient();
        SteamNetworking.SendP2PPacket(hostId, new byte[] { 1 }, 1); // Пример подключения
    }

    public override void StartServer()
    {
        base.StartServer();
    }
}