using Steamworks;
using UnityEngine;

public class SteamP2PManager : MonoBehaviour
{
    private CSteamID hostId;
    private byte[] messageBuffer = new byte[1024];

    public void SendDataToHost(byte[] data)
    {
        SteamNetworking.SendP2PPacket(hostId, data, (uint)data.Length);
    }

    private void Update()
    {
        while (SteamNetworking.IsP2PPacketAvailable(out uint size))
        {
            var packet = new byte[size];
            SteamNetworking.ReadP2PPacket(packet, size, out uint bytesRead, out CSteamID sender);

            HandleReceivedData(sender, packet);
        }
    }

    private void HandleReceivedData(CSteamID sender, byte[] data)
    {
        // Обработка полученных данных
        Debug.Log("Received data from: " + sender);
    }
}