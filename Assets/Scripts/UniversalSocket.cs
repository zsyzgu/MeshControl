using UnityEngine;
using System.IO;
using System;
#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
#else
using System.Threading;
using System.Net.Sockets;
using System.Net;
#endif

abstract public class UniversalSocket : MonoBehaviour
{
#if WINDOWS_UWP
    protected Task mainThread = null;
#else
    protected Thread mainThread = null;
#endif
    protected abstract void run(StreamReader sr, StreamWriter sw);

    protected byte[] readBytes(StreamReader sr, int len)
    {
        byte[] buffer = new byte[len];
        int offset = 0;
        int left = len;
        while (mainThread != null && left > 0)
        {
            int ret = sr.BaseStream.Read(buffer, offset, left);
            if (ret > 0)
            {
                left -= ret;
                offset += ret;
            }
        }
        return buffer;
    }

    protected void readPacket(StreamReader sr, out int id, out int len, out byte[] data)
    {
        byte[] info = readBytes(sr, 5);
        id = info[0];
        len = BitConverter.ToInt32(info, 1);
        data = readBytes(sr, len);
    }

    protected byte[] generatePacket(int id, int len, byte[] data)
    {
        byte[] buffer = new byte[data.Length + 5];
        buffer[0] = (byte)id;
        BitConverter.GetBytes(len).CopyTo(buffer, 1);
        data.CopyTo(buffer, 5);
        return buffer;
    }
}
