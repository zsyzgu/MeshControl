using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceControl : MonoBehaviour
{
    private GameObject leftEye;
    private GameObject rightEye;
    private GameObject mouse;

    private Mesh faceMesh;
    private Mesh leftEyeMesh;
    private Mesh rightEyeMesh;
    private Mesh mouseMesh;

    private Vector3[] vertices = null;
    private int[] triangles = null;
    private byte[] modelTexture = null;
    private byte[] leftEyeTexture = null;
    private byte[] rightEyeTexture = null;
    private byte[] mouseTexture = null;
    private Vector2[] faceUV = null;
    private Vector2[] leftEyeUV = null;
    private Vector2[] rightEyeUV = null;
    private Vector2[] mouseUV = null;


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
            if (trans.name == "Mouse")
            {
                mouse = trans.gameObject;
            }
        }

        faceMesh = GetComponent<MeshFilter>().mesh = new Mesh();
        leftEyeMesh = leftEye.GetComponent<MeshFilter>().mesh = new Mesh();
        rightEyeMesh = rightEye.GetComponent<MeshFilter>().mesh = new Mesh();
        mouseMesh = mouse.GetComponent<MeshFilter>().mesh = new Mesh();
    }

	void Update () {
        if (vertices != null)
        {
            faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouseMesh.vertices = vertices;
            updateNormals();
            vertices = null;
        }

        if (triangles != null)
        {
            int n = triangles.Length / 3;
            faceMesh.triangles = subTris(triangles, 0, (n - 14) * 3, faceMesh.vertices);
            leftEyeMesh.triangles = subTris(triangles, (n - 14) * 3, 4 * 3, leftEyeMesh.vertices);
            rightEyeMesh.triangles = subTris(triangles, (n - 10) * 3, 4 * 3, rightEyeMesh.vertices);
            mouseMesh.triangles = subTris(triangles, (n - 6) * 3, 6 * 3, mouseMesh.vertices);
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
        if (mouseTexture != null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(mouseTexture);
            mouse.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            mouseTexture = null;
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
        if (mouseUV != null)
        {
            mouseMesh.uv = mouseUV;
            mouseUV = null;
        }
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
        leftEyeMesh.normals = rightEyeMesh.normals = mouseMesh.normals = faceMesh.normals;
    }

    public void setModelTexture(byte[] modelTexture)
    {
        this.modelTexture = modelTexture;
    }

    public void setModelUV(Vector2[] uv)
    {
        this.faceUV = uv;
    }

    public void setTris(int[] tris)
    {
        this.triangles = tris;
    }

    public void setVertices(Vector3[] vertices)
    {
        this.vertices = vertices;
    }

    public void setLeftEyeTexture(byte[] leftEyeTexture)
    {
        this.leftEyeTexture = leftEyeTexture;
    }

    public void setLeftEyeUV(Vector2[] uv)
    {
        this.leftEyeUV = uv;
    }

    public void setRightEyeTexture(byte[] rightEyeTexture)
    {
        this.rightEyeTexture = rightEyeTexture;
    }

    public void setRightEyeUV(Vector2[] uv)
    {
        this.rightEyeUV = uv;
    }

    public void setMouseTexture(byte[] mouseTexture)
    {
        this.mouseTexture = mouseTexture;
    }

    public void setMouseUV(Vector2[] uv)
    {
        this.mouseUV = uv;
    }
}
