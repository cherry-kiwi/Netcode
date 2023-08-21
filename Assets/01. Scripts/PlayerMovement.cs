using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerMovement : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> nickName = new NetworkVariable<FixedString32Bytes>();

    private bool w;
    private bool a;
    private bool s;
    private bool d;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }

        SendNickNameServerRpc($"Player {OwnerClientId}");
    }

    private void FixedUpdate()
    {
        //클라이언트에게만 입력을 받기
        //if (IsClient && IsOwner)
        //{
        //    SendInputServerRpc(w, a, s, d);
        //}

        if (!IsOwner) { return; }

        w = Input.GetKey(KeyCode.W);
        a = Input.GetKey(KeyCode.A);
        s = Input.GetKey(KeyCode.S);
        d = Input.GetKey(KeyCode.D);

        SendInputServerRpc(w, a, s, d); //26 ~ 29 라인과 같음, 입력을 서버로 보냄
    }

    //서버 -> 클라이언트 RPC 함수
    [ServerRpc]
    public void SendInputServerRpc(bool w, bool a, bool s, bool d) //서버에서 입력을 받음
    {
        Vector3 input = Vector3.zero;

        if (w)
        {
            input += Vector3.forward;
        }
        if (a)
        {
            input += Vector3.left;
        }
        if (s)
        {
            input += Vector3.back;
        }
        if (d)
        {
            input += Vector3.right;
        }

        transform.Translate(input * 0.05f, Space.World);
    }

    //서버 -> 클라이언트 RPC 함수
    [ServerRpc]
    public void SendNickNameServerRpc(string s)
    {
        nickName.Value = s;
        Debug.Log(nickName.Value);
    }
}
