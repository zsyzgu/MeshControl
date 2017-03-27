using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OptitrackClient : UniveralClient
{
    private CenterServer centerServer = null;
    private FaceControl faceControl = null;

#if WINDOWS_UWP

#else
    void Awake()
    {
        centerServer = GameObject.Find("Center").GetComponent<CenterServer>();
        faceControl = GameObject.Find("Face").GetComponent<FaceControl>();
        startClient("192.168.1.167", 5002);
    }

    void OnApplicationQuit()
    {
        endClient();
    }
#endif

    protected override void run(StreamReader sr, StreamWriter sw)
    {
        Debug.Log("Optitrack connected");

        while (mainThread != null)
        {
            string message = sr.ReadLine();
            if (message == null)
            {
                break;
            }
            string[] tags = message.Split(' ');
            if (tags.Length >= 8 && tags[0] == "rb")
            {
                //int id = int.Parse(tags[1]);
                float x = float.Parse(tags[2]);
                float y = float.Parse(tags[3]);
                float z = float.Parse(tags[4]);
                float rx = float.Parse(tags[5]);
                float ry = float.Parse(tags[6]);
                float rz = float.Parse(tags[7]);
                faceControl.setTransform(new Vector3(x, y, z), new Vector3(rx, ry, rz));
                int len = 24;
                byte[] buffer = new byte[len];
                BitConverter.GetBytes(x).CopyTo(buffer, 0);
                BitConverter.GetBytes(y).CopyTo(buffer, 4);
                BitConverter.GetBytes(z).CopyTo(buffer, 8);
                BitConverter.GetBytes(rx).CopyTo(buffer, 12);
                BitConverter.GetBytes(ry).CopyTo(buffer, 16);
                BitConverter.GetBytes(rz).CopyTo(buffer, 20);
                centerServer.addPacket(10, len, buffer);
            }
        }

        Debug.Log("Optitrack disconnected");
    }
}
