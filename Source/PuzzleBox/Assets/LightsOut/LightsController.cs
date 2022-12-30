using JamesFrowen.SimpleWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
//using static UnityEditor.ObjectChangeEventStream;

public class LightsController : MonoBehaviour
{
    [SerializeField] private Light [] lights;
    [SerializeField] private int matrixWidth = 4;
    [SerializeField] private int matrixHeight = 4;
    [SerializeField] private string scheme = "ws";
    [SerializeField] private string host = "172.16.1.10";
    [SerializeField] private int port = 7777;

    private TcpConfig tcpConfig;
    private SimpleWebClient client;
    private UriBuilder builder;
    private Light[,] matrix;

    // Start is called before the first frame update
    void Start()
    {
        tcpConfig = new TcpConfig(noDelay: false, sendTimeout: 10000, receiveTimeout: 20000);
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

        client.Connect(builder.Uri);

        SetupLights();

        //RandomizeLights();
    }

    private void SetupLights()
    {
        matrix = new Light[matrixWidth, matrixHeight];

        int count = 0;
        for (int w = 0; w < matrixWidth; w++)
        {
            for (int h = 0; h < matrixHeight; h++)
            {
                matrix[w, h] = lights[count++];
            }
        }
    }

    private void RandomizeLights()
    {
        
        Random.Range(1, 10);

        for (int i = 0; i < Random.Range(3, 10); i++)
        {
            int x = Random.Range(0, matrixWidth);
            int y = Random.Range(0, matrixHeight);
            matrix[x,y].State = !matrix[x, y].State;
        }

        if (CheckAllLightsOn())
        {
            int index = Random.Range(0, lights.Length);
            lights[index].State = true;
        }
    }

    private bool CheckAllLightsOn()
    {
        foreach (var light in lights)
        {
            if (light.State)
            {
                return false;
            }
        }

        return true;
    }

    private void LightSpreadHandler(int row, int col)
    {
        matrix[row,col].State = !matrix[row, col].State;
        
        if (row > 0) //Top
        {
            matrix[row - 1, col].State = !matrix[row - 1, col].State;
        }

        if (row < matrixHeight - 1) //Bottom
        {
            matrix[row + 1, col].State = !matrix[row + 1, col].State;
        }

        if (col >= 0 && col < matrixWidth - 1) // Right
        {
            matrix[row, col + 1].State = !matrix[row, col + 1].State;
        }

        if (col > 0 && col < matrixWidth) // Left
        {
            matrix[row, col - 1].State = !matrix[row, col - 1].State;
        }
    }

    private void Client_onError(Exception obj)
    {
        Debug.Log("onError...");
        //throw new NotImplementedException();
    }

    private void Client_onData(ArraySegment<byte> obj)
    {
        Debug.Log("inside onData()");
        string message = System.Text.Encoding.Default.GetString(obj);
        Debug.Log("onData: " + message);

        /*var index = Array.FindIndex(lights, element => element.Character == message[0]);
        int col = (index / matrixWidth);
        int row = (index % matrixWidth);

        LightSpreadHandler(col, row);
        */


        //var light = Array.Find(lights, element => element.Character == message[0]);

        //Debug.Log(index);


        //Debug.Log("mod=" + (index % matrixWidth) + "; " + "col=" + (index / matrixWidth));
        //Debug.Log("x=" + x + "; " + "y=" + y);
        /*if (index < matrixWidth)
        {
            Debug.Log("x=" + index + "; y=" + 0);
        } else {
            Debug.Log("mod=" + (index % matrixWidth) + "; " + "col=" + (index / matrixWidth));

        }*/
        // if greater than matrixwidth then index mod matrixwidth and index divided by matrix width;  divided will be row, mod will be column

        //light.State = !light.State;

        //throw new NotImplementedException();
    }

    private void Client_onDisconnect()
    {
        Debug.Log("onDisconnect...");
        //throw new NotImplementedException();
        client.Connect(builder.Uri);
    }

    private void Client_onConnect()
    {
        Debug.Log("onConnect()... : " + builder.Uri);
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


