using JamesFrowen.SimpleWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LightsController : MonoBehaviour
{
    [SerializeField] private Light [] lights;
    [SerializeField] private string scheme = "ws";
    [SerializeField] private string host = "172.16.1.10";
    [SerializeField] private int port = 7777;

    private TcpConfig tcpConfig;
    private SimpleWebClient client;

    // Start is called before the first frame update
    void Start()
    {
        tcpConfig = new TcpConfig(noDelay: false, sendTimeout: 5000, receiveTimeout: 20000);
        client = SimpleWebClient.Create(ushort.MaxValue, 5000, tcpConfig);

        client.onConnect += Client_onConnect;
        client.onDisconnect += Client_onDisconnect;
        client.onData += Client_onData;
        client.onError += Client_onError;

        var builder = new UriBuilder
        {
            Scheme = scheme,
            Host = host,
            Port = port
        };

        client.Connect(builder.Uri);


    }

    private void Client_onError(Exception obj)
    {
        Debug.Log("onError...");
        //throw new NotImplementedException();
    }

    private void Client_onData(ArraySegment<byte> obj)
    {
        string message = System.Text.Encoding.Default.GetString(obj);
        Debug.Log("onData: " + message);

        //throw new NotImplementedException();
    }

    private void Client_onDisconnect()
    {
        Debug.Log("onDisconnect...");
        //throw new NotImplementedException();
    }

    private void Client_onConnect()
    {
        Debug.Log("onConnect...");
        //throw new NotImplementedException();


        

    }

    // Update is called once per frame
    void Update()
    {
        client.ProcessMessageQueue();
        /*client.ProcessMessageQueue();

        byte[] message = Encoding.ASCII.GetBytes("Hello Server");
        client.Send(new ArraySegment<byte>(message));
        */
    }
}


