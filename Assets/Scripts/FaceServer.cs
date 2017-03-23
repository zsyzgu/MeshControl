using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;
using System.Text;

public class FaceServer : UniversalServer
{
    private FaceControl faceControl = null;

    void Awake()
    {
        faceControl = GameObject.Find("Face").GetComponent<FaceControl>();
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
            byte[] info = readBytes(sr, 5);
            int id = info[0];
            int len = BitConverter.ToInt32(info, 1);
            byte[] data = readBytes(sr, len);
            faceControl.cmd(id, data);
        }
    }
}
