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
    [SerializeField] private float keepAlive = 5;

    private TcpConfig tcpConfig;
    private SimpleWebClient client;
    private UriBuilder builder;

    // Start is called before the first frame update
    void Start()
    {
        //tcpConfig = new TcpConfig(noDelay: true, sendTimeout: 10000, receiveTimeout: 20000);
        //client = SimpleWebClient.Create(ushort.MaxValue, 5000, tcpConfig);
        var tcpConfig = new TcpConfig(true, 5000, 5000);
        client = SimpleWebClient.Create(32000, 500, tcpConfig);

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
            Debug.Log("GOT HERE");
            Debug.LogException(ex);
        }

    }

    private void Client_onError(Exception obj)
    {
        Debug.Log("onError...");
        Debug.Log(obj.Message);
    }

    private void Client_onData(ArraySegment<byte> obj)
    {
        string message = System.Text.Encoding.Default.GetString(obj);
        Debug.Log("onData: " + message);
    }

    private void Client_onDisconnect()
    {
        Debug.Log("Disconnected from " + scheme + "://" + host + ":" + port);
        Connect();
    }

    private void Client_onConnect()
    {
        Debug.Log("Connected to " + scheme + "://" + host + ":" + port);
    }

    private void OnDestroy()
    {
        client?.Disconnect();
    }

    void Update()
    {
        client?.ProcessMessageQueue();
        if (keepAlive < Time.time)
        {
            client?.Send(new ArraySegment<byte>(new byte[1] { 0 }));
            keepAlive = Time.time + 1;
        }
    }
}
