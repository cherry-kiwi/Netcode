using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public TMP_InputField inputField;

    private void Start()
    {
        hostButton.onClick.AddListener(() => HostButtonClick());
        clientButton.onClick.AddListener(() => ClientButtonClick());
    }

    public async void HostButtonClick()
    {
        var data = await RelayManager.SetupRelay(10, "production");
        //await을 쓰는 이유: 인터넷에 연결할 때 시간이 걸리는데 그동안 아예 게임이 멈춰버리면 안되니까
        
        NetworkManager.Singleton.GetComponent<UnityTransport>()
            .SetRelayServerData(data.IPv4Address, data.port, data.allocationIDBytes, data.key, data.connectionData);

        inputField.text = data.joinCode;

        NetworkManager.Singleton.StartHost();
    }

    public async void ClientButtonClick()
    {
        var data = await RelayManager.JoinRelay(inputField.text, "production");

        NetworkManager.Singleton.GetComponent<UnityTransport>()
            .SetRelayServerData(data.IPv4Address, data.port, data.allocationIDBytes, data.key, data.connectionData, data.hostConnectionData);
        
        NetworkManager.Singleton.StartClient();
    }
}
