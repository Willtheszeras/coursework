using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Websocket : MonoBehaviour
{
    public Plotter plt;
    public Translator trans;

    //Uri ip = new("ws://192.168.137.167:7777");
    //ArraySegment<byte> stor = new();
    
    TcpClient socket;

    bool plot = false;
    int index = 0;

    //CancellationTokenSource src = new();
    //Task connection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Trigger()
    {
        plt.points.Clear();
        foreach (GameObject i in plt.objs)
        {
            Destroy(i);
        }

        socket = new TcpClient("192.168.137.40", 7777);

        //connection = socket.ConnectAsync(ip, src.Token);
        //Debug.Log(connection);

        for (int i = 0; i < 10000; i++)
        {
            var data = socket.GetStream();
            int byteinput = data.ReadByte();

            if (byteinput == 84)
            {
                plot = true;
            }

            if (plot)
            {
                trans.packet[index] = byteinput;
                index++;
                if (index == 47)
                {
                    index = 0;
                    plt.Plot();
                }
            }
        }

        socket.Close();
        plt.finishedReading = true;
        plt.onfinished();
    }

    // Update is called once per frame
    void Update()
    {
        //if (connection.IsCompleted)
        //{
        //Debug.Log(data.ReadByte());
        //Debug.Log(stor);
        //}
        //for (int i = 0; i < 100; i++)
        //{
        //while (socket.Available > 0)
        //{

        //    var data = socket.GetStream();
        //    int byteinput = data.ReadByte();

        //    if (byteinput == 84)
        //    {
        //        plot = true;
        //    }

        //    if (plot)
        //    {
        //        trans.packet[index] = byteinput;
        //        index++;
        //        if (index == 47)
        //        {
        //            index = 0;
        //            plt.Plot();
        //        }
        //    }
        //}
        //}
    }
}
