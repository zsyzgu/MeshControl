using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HololensClient : UniveralClient
{
    private FaceControl faceControl = null;

    void Awake()
    {
        faceControl = GameObject.Find("Face").GetComponent<FaceControl>();
        startClient("192.168.1.135", 5003);
    }

    void OnApplicationQuit()
    {
        endClient();
    }

    protected override void run(StreamReader sr, StreamWriter sw)
    {
        while (mainThread != null)
        {
            int id, len;
            byte[] data;
            readPacket(sr, out id, out len, out data);
            faceControl.cmd(id, data);
        }
    }
}
