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

    protected override bool loop(StreamReader sr, StreamWriter sw)
    {
        byte[] info = readBytes(sr, 5);
        int id = info[0];
        int len = BitConverter.ToInt32(info, 1);
        byte[] data = readBytes(sr, len);
        cmd(id, data);
        return true;
    }

    private void cmd(int id, byte[] data)
    {
        switch (id)
        {
            case 0: //model.jpg
                faceControl.setModelTexture(data);
                break;
            case 1: //model.uv
                faceControl.setModelUV(parseToVector2Array(data));
                break;
            case 2: //face.ver
                faceControl.setVertices(parseToVertices(data));
                break;
            case 3: //face.tri
                faceControl.setTris(parseToIntArray(data));
                break;
            case 4: //lefteye.jpg
                faceControl.setLeftEyeTexture(data);
                break;
            case 5: //lefteye.uv
                faceControl.setLeftEyeUV(parseToVector2Array(data));
                break;
            case 6: //righteye.jpg
                faceControl.setRightEyeTexture(data);
                break;
            case 7: //righteye.uv
                faceControl.setRightEyeUV(parseToVector2Array(data));
                break;
            case 8: //mouth.jpg
                faceControl.setMouthTexture(data);
                break;
            case 9: //mouth.uv
                faceControl.setMouthUV(parseToVector2Array(data));
                break;
            default:
                Debug.Log("Unknown cmd.");
                break;
        }
    }

    private Texture parseToTexture(byte[] data)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(data);
        return texture;
    }

    private int[] parseToIntArray(byte[] data)
    {
        int n = data.Length / 4;
        int[] intArray = new int[n];
        for (int i = 0; i < n; i++)
        {
            intArray[i] = BitConverter.ToInt32(data, i * 4);
        }
        return intArray;
    }

    private float[] parseToFloatArray(byte[] data)
    {
        int n = data.Length / 4;
        float[] floatArray = new float[n];
        for (int i = 0; i < n; i++)
        {
            floatArray[i] = BitConverter.ToSingle(data, i * 4);
        }
        return floatArray;
    }

    private Vector2[] parseToVector2Array(byte[] data)
    {
        float[] floatArray = parseToFloatArray(data);
        int n = floatArray.Length / 2;
        Vector2[] vector2Array = new Vector2[n];
        for (int i = 0; i < n; i++)
        {
            vector2Array[i] = new Vector2(floatArray[i * 2], floatArray[i * 2 + 1]);
        }
        return vector2Array;
    }

    private Vector3[] parseToVertices(byte[] data)
    {
        float[] floatArray = parseToFloatArray(data);
        int n = floatArray.Length / 3;
        Vector3[] vector3Array = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            vector3Array[i] = new Vector3(floatArray[i * 3], -floatArray[i * 3 + 1], floatArray[i * 3 + 2] * 0.5f);
        }
        return vector3Array;
    }
}
