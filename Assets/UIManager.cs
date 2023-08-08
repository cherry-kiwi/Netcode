using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(() => HostButtonClick());
        clientButton.onClick.AddListener(() => ClientButtonClick());
    }

    public void HostButtonClick()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ClientButtonClick()
    {
        NetworkManager.Singleton.StartClient();
    }
}
