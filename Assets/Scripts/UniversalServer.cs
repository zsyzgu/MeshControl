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

abstract public class UniversalServer : UniversalSocket
{
    private int port;

#if WINDOWS_UWP

#else
    protected void startServer(int port)
    {
        string ipAddress = Network.player.ipAddress;
        this.port = port;
        mainThread = new Thread(() => hostServer(ipAddress));
        mainThread.Start();
    }

    protected void hostServer(string ipAddress)
    {
        IPAddress serverIP = IPAddress.Parse(ipAddress);
        TcpListener listener = new TcpListener(serverIP, port);

        listener.Start();
        while (mainThread != null)
        {
            if (listener.Pending())
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread thread = new Thread(() => msgThread(client));
                thread.Start();
            }
            Thread.Sleep(10);
        }
        listener.Stop();
    }


    private void msgThread(TcpClient client)
    {
        StreamReader sr = new StreamReader(client.GetStream());
        StreamWriter sw = new StreamWriter(client.GetStream());

        while (mainThread != null)
        {
            if (loop(sr, sw) == false)
            {
                break;
            }
        }

        client.Close();
    }

    protected void endServer()
    {
        mainThread = null;
    }
#endif
}
