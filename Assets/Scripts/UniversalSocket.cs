using UnityEngine;
using System.IO;
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

    protected void copyStream(StreamReader sr, StreamWriter sw)
    {
        byte[] buffer = new byte[32768];
        int len = 0;
        while (mainThread != null && (len = sr.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            sw.BaseStream.Write(buffer, 0, len);
        }
    }
}
