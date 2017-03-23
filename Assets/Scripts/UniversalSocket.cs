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

    protected abstract bool loop(StreamReader sr, StreamWriter sw);

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
}
