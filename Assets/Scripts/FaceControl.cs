using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void localStart()
    {
        faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouthMesh.vertices = FileReader.getVertices("face.ver");
        setTris(FileReader.getTris("face.tri"));
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

    public void setMouthTexture(byte[] mouthTexture)
    {
        this.mouthTexture = mouthTexture;
    }

    public void setMouthUV(Vector2[] uv)
    {
        this.mouthUV = uv;
    }

    public void setTransform(Vector3 position, Vector3 rotation)
    {
        transform.position = position;
        transform.eulerAngles = rotation;
    }
}
