using UnityEngine;
using System;
using System.IO;
using System.Text;
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

abstract public class UniveralClient : UniversalSocket
{
    string serverIP;
    int port;

#if WINDOWS_UWP
    protected void startClient(string serverIP, int port)
    {
        this.serverIP = serverIP;
        this.port = port;
        mainThread = new Task(msgThread);
        mainThread.Start();
    }

    protected void endClient() {
        mainThread = null;
    }

    private async void msgThread() {
        StreamSocket socket = new StreamSocket();
        await socket.ConnectAsync(new HostName(serverIP), "" + port);
        StreamReader sr = new StreamReader(socket.InputStream.AsStreamForRead());
        StreamWriter sw = new StreamWriter(socket.OutputStream.AsStreamForWrite());
        
        run(sr, sw);
    }
#else
    protected void startClient(string serverIP, int port)
    {
        this.serverIP = serverIP;
        this.port = port;
        mainThread = new Thread(msgThread);
        mainThread.Start();
    }

    protected void endClient()
    {
        mainThread = null;
    }

    private void msgThread()
    {
        TcpClient client = new TcpClient();
        client.Connect(serverIP, port);

        StreamReader sr = new StreamReader(client.GetStream());
        StreamWriter sw = new StreamWriter(client.GetStream());

        run(sr, sw);

        client.Close();
    }
#endif
}
