using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading;

public class CenterServer : UniversalServer {
    BlockingQueue<byte[]> queue;

#if WINDOWS_UWP

#else
    void Awake()
    {
        queue = new BlockingQueue<byte[]>();
        startServer(5003);
    }

    void OnApplicationQuit()
    {
        queue.Stop();
        endServer();
    }
#endif

    protected override void run(StreamReader sr, StreamWriter sw)
    {
        Debug.Log("Hololens connected");

        byte[] data;
        while (mainThread != null && (data = queue.Dequeue()) != null)
        {
            sw.BaseStream.Write(data, 0, data.Length);
        }

        Debug.Log("Hololens disconnected");
    }

    public void addPacket(int id, int len, byte[] data)
    {
        queue.Enqueue(generatePacket(id, len, data));
    }
}
