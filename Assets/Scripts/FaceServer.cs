using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;
using System.Text;

public class FaceServer : UniversalServer
{
    private CenterServer centerServer = null;

    void Awake()
    {
        centerServer = GameObject.Find("Center").GetComponent<CenterServer>();
        startServer(5001);
    }

    void OnApplicationQuit()
    {
        endServer();
    }

    protected override void run(StreamReader sr, StreamWriter sw)
    {
        while (mainThread != null)
        {
            int id, len;
            byte[] data;
            readPacket(sr, out id, out len, out data);
            centerServer.addPacket(id, len, data);
        }
    }
}
