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

    private byte[] modelTextureData = null;
    private byte[] leftEyeTextureData = null;
    private byte[] rightEyeTextureData = null;
    private byte[] mouseTextureData = null;

    private Vector2[] faceUVData = null;
    private Vector2[] leftEyeUVData = null;
    private Vector2[] rightEyeUVData = null;
    private Vector2[] mouseUVData = null;

    private Vector3[] vertices = null;

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
        
        faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouseMesh.vertices = FileReader.getVertices("face.ver");
        setTris(FileReader.getTris("face.tri"));
        faceMesh.RecalculateNormals();
        leftEyeMesh.normals = rightEyeMesh.normals = mouseMesh.normals = faceMesh.normals;

        faceMesh.uv = FileReader.getUV("model.uv");
        leftEyeMesh.uv = FileReader.getUV("lefteye.uv");
        rightEyeMesh.uv = FileReader.getUV("righteye.uv");
        mouseMesh.uv = FileReader.getUV("mouse.uv");

        setModelTexture(FileReader.getTexture("model.jpg"));
        setLeftEyeTexture(FileReader.getTexture("lefteye.jpg"));
        setRightEyeTexture(FileReader.getTexture("righteye.jpg"));
        setMouseTexture(FileReader.getTexture("mouse.jpg"));
    }

	void Update () {
        /*Vector3[] vertices = getFace();
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i] + new Vector3(0, 0, 0.1f * Mathf.Sin(i * Time.time));
        }
        GetComponent<MeshFilter>().mesh.vertices = vertices;*/
        updateVertices();
        updateModelTexture();
        updateLeftEyeTexture();
        updateRightEyeTexture();
        updateMouseTexture();
        updateModelUV();
        updateLeftEyeUV();
        updateRightEyeUV();
        updateMouseUV();
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

    public void setModelTexture(Texture faceTexture)
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", faceTexture);
    }

    private void updateModelTexture()
    {
        if (modelTextureData != null)
        {
            Texture2D faceTexture = new Texture2D(1, 1);
            faceTexture.LoadImage(modelTextureData);
            setModelTexture(faceTexture);
            modelTextureData = null;
        }
    }

    public void setModelTextureData(byte[] modelTextureData)
    {
        this.modelTextureData = modelTextureData;
    }

    private void updateModelUV()
    {
        if (faceUVData != null)
        {
            faceMesh.uv = faceUVData;
            faceUVData = null;
        }
    }

    public void setModelUVData(Vector2[] uv)
    {
        faceUVData = uv;
    }

    public void setTris(int[] tris)
    {
        int n = tris.Length / 3;
        faceMesh.triangles = subTris(tris, 0, (n - 14) * 3, faceMesh.vertices);
        leftEyeMesh.triangles = subTris(tris, (n - 14) * 3, 4 * 3, leftEyeMesh.vertices);
        rightEyeMesh.triangles = subTris(tris, (n - 10) * 3, 4 * 3, rightEyeMesh.vertices);
        mouseMesh.triangles = subTris(tris, (n - 6) * 3, 6 * 3, mouseMesh.vertices);
    }

    private void updateVertices()
    {
        if (vertices != null)
        {
            faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouseMesh.vertices = vertices;
            faceMesh.RecalculateNormals();
            leftEyeMesh.normals = rightEyeMesh.normals = mouseMesh.normals = faceMesh.normals;
            vertices = null;
        }
    }

    public void setVerticesData(Vector3[] vertices)
    {
        this.vertices = vertices;
    }

    public void setLeftEyeTexture(Texture leftEyeTexture)
    {
        leftEye.GetComponent<Renderer>().material.SetTexture("_MainTex", leftEyeTexture);
    }

    private void updateLeftEyeTexture()
    {
        if (leftEyeTextureData != null)
        {
            Texture2D leftEyeTexture = new Texture2D(1, 1);
            leftEyeTexture.LoadImage(leftEyeTextureData);
            setLeftEyeTexture(leftEyeTexture);
            leftEyeTextureData = null;
        }
    }

    public void setLeftEyeTextureData(byte[] leftEyeTextureData)
    {
        this.leftEyeTextureData = leftEyeTextureData;
    }

    private void updateLeftEyeUV()
    {
        if (leftEyeUVData != null)
        {
            leftEyeMesh.uv = leftEyeUVData;
            leftEyeUVData = null;
        }
    }

    public void setLeftEyeUVData(Vector2[] uv)
    {
        leftEyeUVData = uv;
    }

    public void setRightEyeTexture(Texture rightEyeTexture)
    {
        rightEye.GetComponent<Renderer>().material.SetTexture("_MainTex", rightEyeTexture);
    }

    private void updateRightEyeTexture()
    {
        if (rightEyeTextureData != null)
        {
            Texture2D rightEyeTexture = new Texture2D(1, 1);
            rightEyeTexture.LoadImage(rightEyeTextureData);
            setRightEyeTexture(rightEyeTexture);
            rightEyeTextureData = null;
        }
    }

    public void setRightEyeTextureData(byte[] rightEyeTextureData)
    {
        this.rightEyeTextureData = rightEyeTextureData;
    }

    private void updateRightEyeUV()
    {
        if (rightEyeUVData != null)
        {
            rightEyeMesh.uv = rightEyeUVData;
            rightEyeUVData = null;
        }
    }

    public void setRightEyeUVData(Vector2[] uv)
    {
        rightEyeUVData = uv;
    }

    public void setMouseTexture(Texture mouseTexture)
    {
        mouse.GetComponent<Renderer>().material.SetTexture("_MainTex", mouseTexture);
    }

    private void updateMouseTexture()
    {
        if (mouseTextureData != null)
        {
            Texture2D mouseTexture = new Texture2D(1, 1);
            mouseTexture.LoadImage(mouseTextureData);
            setMouseTexture(mouseTexture);
            mouseTextureData = null;
        }
    }

    public void setMouseTextureData(byte[] mouseTextureData)
    {
        this.mouseTextureData = mouseTextureData;
    }

    private void updateMouseUV()
    {
        if (mouseUVData != null)
        {
            mouseMesh.uv = mouseUVData;
            mouseUVData = null;
        }
    }

    public void setMouseUVData(Vector2[] uv)
    {
        mouseUVData = uv;
    }
}
