using JamesFrowen.SimpleWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketManager : MonoBehaviour
{
    [SerializeField] private string scheme = "ws";
    [SerializeField] private string host = "172.16.200.200";
    [SerializeField] private int port = 7777;
    [SerializeField] private WebSocketEvent OnKeypadPress;
    
    private SimpleWebClient client;
    private UriBuilder builder;

    void Start()
    {
        var tcpConfig = new TcpConfig(noDelay: true, sendTimeout: 10000, receiveTimeout: 20000);
        client = SimpleWebClient.Create(ushort.MaxValue, 5000, tcpConfig);
    
        client.onConnect += Client_onConnect;
        client.onDisconnect += Client_onDisconnect;
        client.onData += Client_onData;
        client.onError += Client_onError;

        builder = new UriBuilder
        {
            Scheme = scheme,
            Host = host,
            Port = port
        };

        Connect();
    }

    private void Connect()
    {
        try
        {
            Debug.Log("Trying to connect to " + scheme + "://" + host + ":" + port);
            client.Connect(builder.Uri);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

    }

    private void Client_onError(Exception obj)
    {
        Debug.Log("WebSocketManager: onError()");
        Debug.Log(obj.Message);
    }

    private void Client_onData(ArraySegment<byte> obj)
    {
        string message = System.Text.Encoding.Default.GetString(obj);
        Debug.Log("WebSocketManager onData(): " + message);
        OnKeypadPress.Raise(new Assets.ScriptableObjects.WebSocketMessage() { Type = Assets.ScriptableObjects.MessageType.KEYPRESS, Data = "1" });
    }

    private void Client_onDisconnect()
    {
        Debug.Log("WebSocketManager:  Disconnected from " + scheme + "://" + host + ":" + port);
        Connect();
    }

    private void Client_onConnect()
    {
        Debug.Log("WebSocketManager:  Connected to " + scheme + "://" + host + ":" + port);
    }

    private void OnDestroy()
    {
        client?.Disconnect();
    }

    void Update()
    {
        client?.ProcessMessageQueue();
    }
}
