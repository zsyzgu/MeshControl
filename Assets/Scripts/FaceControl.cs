using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System;

public class FaceControl : MonoBehaviour
{
    private GameObject leftEye;
    private GameObject rightEye;
    private GameObject mouth;

    private Mesh faceMesh;
    private Mesh leftEyeMesh;
    private Mesh rightEyeMesh;
    private Mesh mouthMesh;

    private Vector3[] vertices = null;
    private int[] triangles = null;
    private byte[] modelTexture = null;
    private byte[] leftEyeTexture = null;
    private byte[] rightEyeTexture = null;
    private byte[] mouthTexture = null;
    private Vector2[] faceUV = null;
    private Vector2[] leftEyeUV = null;
    private Vector2[] rightEyeUV = null;
    private Vector2[] mouthUV = null;
    private bool faceMoved = false;
    private Vector3 facePosition;
    private Vector3 faceEulerAngles;
    
    void Start()
    {
        foreach (Transform trans in transform)
        {
            if (trans.name == "LeftEye")
            {
                leftEye = trans.gameObject;
            }
            if (trans.name == "RightEye")
            {
                rightEye = trans.gameObject;
            }
            if (trans.name == "Mouth")
            {
                mouth = trans.gameObject;
            }
        }

        faceMesh = GetComponent<MeshFilter>().mesh = new Mesh();
        leftEyeMesh = leftEye.GetComponent<MeshFilter>().mesh = new Mesh();
        rightEyeMesh = rightEye.GetComponent<MeshFilter>().mesh = new Mesh();
        mouthMesh = mouth.GetComponent<MeshFilter>().mesh = new Mesh();

        localStart();
    }

	void Update () {
        if (vertices != null)
        {
            faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouthMesh.vertices = vertices;
            updateNormals();
            vertices = null;
        }

        if (triangles != null)
        {
            int n = triangles.Length / 3;
            faceMesh.triangles = subTris(triangles, 0, (n - 14) * 3, faceMesh.vertices);
            leftEyeMesh.triangles = subTris(triangles, (n - 14) * 3, 4 * 3, leftEyeMesh.vertices);
            rightEyeMesh.triangles = subTris(triangles, (n - 10) * 3, 4 * 3, rightEyeMesh.vertices);
            mouthMesh.triangles = subTris(triangles, (n - 6) * 3, 6 * 3, mouthMesh.vertices);
            updateNormals();
            triangles = null;
        }

        if (modelTexture != null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(modelTexture);
            GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            modelTexture = null;
        }
        if (leftEyeTexture != null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(leftEyeTexture);
            leftEye.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            leftEyeTexture = null;
        }
        if (rightEyeTexture != null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(rightEyeTexture);
            rightEye.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            rightEyeTexture = null;
        }
        if (mouthTexture != null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(mouthTexture);
            mouth.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            mouthTexture = null;
        }

        if (faceUV != null)
        {
            faceMesh.uv = faceUV;
            faceUV = null;
        }
        if (leftEyeUV != null)
        {
            leftEyeMesh.uv = leftEyeUV;
            leftEyeUV = null;
        }
        if (rightEyeUV != null)
        {
            rightEyeMesh.uv = rightEyeUV;
            rightEyeUV = null;
        }
        if (mouthUV != null)
        {
            mouthMesh.uv = mouthUV;
            mouthUV = null;
        }
        if (faceMoved)
        {
            transform.position = facePosition;
            transform.eulerAngles = faceEulerAngles;
            faceMoved = false;
        }
    }

    private void localStart()
    {
        faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouthMesh.vertices = FileReader.getVertices("face.ver");
        triangles = FileReader.getTris("face.tri");
        updateNormals();

        faceMesh.uv = FileReader.getUV("model.uv");
        leftEyeMesh.uv = FileReader.getUV("lefteye.uv");
        rightEyeMesh.uv = FileReader.getUV("righteye.uv");
        mouthMesh.uv = FileReader.getUV("mouth.uv");

        GetComponent<Renderer>().material.SetTexture("_MainTex", FileReader.getTexture("model.jpg"));
        leftEye.GetComponent<Renderer>().material.SetTexture("_MainTex", FileReader.getTexture("lefteye.jpg"));
        rightEye.GetComponent<Renderer>().material.SetTexture("_MainTex", FileReader.getTexture("righteye.jpg"));
        mouth.GetComponent<Renderer>().material.SetTexture("_MainTex", FileReader.getTexture("mouth.jpg"));
    }

    private int[] subTris(int[] tris, int s, int len, Vector3[] vertices)
    {
        int[] ret = new int[len];
        for (int i = 0; i < len; i++)
        {
            ret[i] = tris[s + i];
            if (i % 3 == 2 && Vector3.Cross(vertices[ret[i - 1]] - vertices[ret[i - 2]], vertices[ret[i]] - vertices[ret[i - 2]]).z > 0)
            {
                int tmp = ret[i - 1];
                ret[i - 1] = ret[i];
                ret[i] = tmp;
            }
        }
        return ret;
    }

    private void updateNormals()
    {
        faceMesh.RecalculateNormals();
        leftEyeMesh.normals = rightEyeMesh.normals = mouthMesh.normals = faceMesh.normals;
    }

    public void setTransform(Vector3 position, Vector3 eulerAngles)
    {
        faceMoved = true;
        facePosition = position;
        faceEulerAngles = eulerAngles;
    }

    public void cmd(int id, byte[] data)
    {
        switch (id)
        {
            case 0: //model.jpg
                modelTexture = data;
                break;
            case 1: //model.uv
                faceUV = parseToVector2Array(data);
                break;
            case 2: //face.ver
                vertices = parseToVertices(data);
                break;
            case 3: //face.tri
                triangles = parseToIntArray(data);
                break;
            case 4: //lefteye.jpg
                leftEyeTexture = data;
                break;
            case 5: //lefteye.uv
                leftEyeUV = parseToVector2Array(data);
                break;
            case 6: //righteye.jpg
                rightEyeTexture = data;
                break;
            case 7: //righteye.uv
                rightEyeUV = parseToVector2Array(data);
                break;
            case 8: //mouth.jpg
                mouthTexture = data;
                break;
            case 9: //mouth.uv
                mouthUV = parseToVector2Array(data);
                break;
            case 10: //transform
                float[] array = parseToFloatArray(data);
                setTransform(new Vector3(array[0], array[1], array[2]), new Vector3(array[3], array[4], array[5]));
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
