using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class StumpNetworkRunner : MonoBehaviour, INetworkRunnerCallbacks
{
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    #region Input
    private bool _mouse0Down;
    private bool _mouse0Up;
    private bool _mouse1Down;
    private bool _mouse1Up;

    private bool _alpha0Down;
    private bool _alpha1Down;
    private bool _alpha2Down;
    private bool _alpha3Down;
    private bool _alpha4Down;
    private bool _alpha5Down;
    private bool _alpha6Down;
    private bool _alpha7Down;
    private bool _alpha8Down;
    private bool _alpha9Down;

    private void Update()
    {
        _mouse0Down |= Input.GetMouseButtonDown(0);
        _mouse0Up |= Input.GetMouseButtonUp(0);
        _mouse1Down |= Input.GetMouseButtonDown(1);
        _mouse1Up |= Input.GetMouseButtonUp(1);

        _alpha0Down |= Input.GetKeyDown(KeyCode.Alpha0);
        _alpha1Down |= Input.GetKeyDown(KeyCode.Alpha1);
        _alpha2Down |= Input.GetKeyDown(KeyCode.Alpha2);
        _alpha3Down |= Input.GetKeyDown(KeyCode.Alpha3);
        _alpha4Down |= Input.GetKeyDown(KeyCode.Alpha4);
        _alpha5Down |= Input.GetKeyDown(KeyCode.Alpha5);
        _alpha6Down |= Input.GetKeyDown(KeyCode.Alpha6);
        _alpha7Down |= Input.GetKeyDown(KeyCode.Alpha7);
        _alpha8Down |= Input.GetKeyDown(KeyCode.Alpha8);
        _alpha9Down |= Input.GetKeyDown(KeyCode.Alpha9);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkedInputData { MousePosition = Input.mousePosition };

        if (_mouse0Down) data.Buttons |= NetworkedInputMapping.MOUSEBUTTON0_DOWN;
        if (_mouse0Up) data.Buttons |= NetworkedInputMapping.MOUSEBUTTON0_UP;
        if (_mouse1Down) data.Buttons |= NetworkedInputMapping.MOUSEBUTTON1_DOWN;
        if (_mouse1Up) data.Buttons |= NetworkedInputMapping.MOUSEBUTTON1_UP;

        if (_alpha0Down) data.Buttons |= NetworkedInputMapping.ALPHA0_DOWN;
        if (_alpha1Down) data.Buttons |= NetworkedInputMapping.ALPHA1_DOWN;
        if (_alpha2Down) data.Buttons |= NetworkedInputMapping.ALPHA2_DOWN;
        if (_alpha3Down) data.Buttons |= NetworkedInputMapping.ALPHA3_DOWN;
        if (_alpha4Down) data.Buttons |= NetworkedInputMapping.ALPHA4_DOWN;
        if (_alpha5Down) data.Buttons |= NetworkedInputMapping.ALPHA5_DOWN;
        if (_alpha6Down) data.Buttons |= NetworkedInputMapping.ALPHA6_DOWN;
        if (_alpha7Down) data.Buttons |= NetworkedInputMapping.ALPHA7_DOWN;
        if (_alpha8Down) data.Buttons |= NetworkedInputMapping.ALPHA8_DOWN;
        if (_alpha9Down) data.Buttons |= NetworkedInputMapping.ALPHA9_DOWN;

        ResetInputs();

        input.Set(data);
    }

    private void ResetInputs()
    {
        _mouse0Down = false;
        _mouse0Up = false;
        _mouse1Down = false;
        _mouse1Up = false;

        _alpha0Down = false;
        _alpha1Down = false;
        _alpha2Down = false;
        _alpha3Down = false;
        _alpha4Down = false;
        _alpha5Down = false;
        _alpha6Down = false;
        _alpha7Down = false;
        _alpha8Down = false;
        _alpha9Down = false;
    }
    #endregion
    
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}
