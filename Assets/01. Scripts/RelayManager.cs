using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public struct RelayHostData
    {
        public string joinCode;
        public string IPv4Address;
        public ushort port;
        public Guid allocationID;
        public byte[] allocationIDBytes;
        public byte[] connectionData;
        public byte[] key;
    }

    public struct RelayJoinData
    {
        public string IPv4Address;
        public ushort port;
        public Guid allocationID;
        public byte[] allocationIDBytes;
        public byte[] connectionData;
        public byte[] hostConnectionData;
        public byte[] key;
    }

    public static async Task<RelayHostData> SetupRelay(int maxConn, string environment)
    {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn) //로그인이 안되어있다면
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); //익명으로 시작하기
        }

        Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);

        RelayHostData data = new RelayHostData
        {
            IPv4Address = allocation.RelayServer.IpV4,
            port = (ushort)allocation.RelayServer.Port,

            allocationID = allocation.AllocationId,
            allocationIDBytes = allocation.AllocationIdBytes,
            connectionData = allocation.ConnectionData,
            key = allocation.Key,
        };

        data.joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return data;
    }

    public static async Task<RelayJoinData> JoinRelay(string joinCode, string environment)
    {
        Debug.LogError($"Start Join by{joinCode}");
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn) //로그인이 안되어있다면
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); //익명으로 시작하기
        }

        JoinAllocation allocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayJoinData data = new RelayJoinData
        {
            IPv4Address = allocation.RelayServer.IpV4,
            port = (ushort)allocation.RelayServer.Port,

            allocationID = allocation.AllocationId,
            allocationIDBytes = allocation.AllocationIdBytes,
            connectionData = allocation.ConnectionData,
            hostConnectionData = allocation.HostConnectionData,
            key = allocation.Key,
        };

        return data;   
    }
}
