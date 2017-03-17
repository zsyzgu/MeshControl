using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceControl : MonoBehaviour
{
    private Texture faceTexture;
    private Texture leftEyeTexture;
    private Texture rightEyeTexture;
    private Texture mouseTexture;
    private Vector3[] vertices;
    private Vector2[] faceUV;
    private Vector2[] leftEyeUV;
    private Vector2[] rightEyeUV;
    private Vector2[] mouseUV;
    private int[] faceTris;
    private int[] leftEyeTris;
    private int[] rightEyeTris;
    private int[] mouseTris;

    private GameObject leftEye;
    private GameObject rightEye;
    private GameObject mouse;

    private Mesh faceMesh;
    private Mesh leftEyeMesh;
    private Mesh rightEyeMesh;
    private Mesh mouseMesh;

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

        setVertices(FileReader.getVertices("face.ver"));
        setTris(FileReader.getTris("face.tri"));
        updateNormals();

        setModelUV(FileReader.getUV("model.uv"));
        setLeftEyeUV(FileReader.getUV("lefteye.uv"));
        setRightEyeUV(FileReader.getUV("righteye.uv"));
        setMouseUV(FileReader.getUV("mouse.uv"));

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

    public void setModelUV(Vector2[] uv)
    {
        faceMesh.uv = uv;
    }

    public void setTris(int[] tris)
    {
        int n = tris.Length / 3;
        faceMesh.triangles = subTris(tris, 0, (n - 14) * 3, faceMesh.vertices);
        leftEyeMesh.triangles = subTris(tris, (n - 14) * 3, 4 * 3, leftEyeMesh.vertices);
        rightEyeMesh.triangles = subTris(tris, (n - 10) * 3, 4 * 3, rightEyeMesh.vertices);
        mouseMesh.triangles = subTris(tris, (n - 6) * 3, 6 * 3, mouseMesh.vertices);
    }

    public void setVertices(Vector3[] vertices)
    {
        faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouseMesh.vertices = vertices;
    }
    
    public void updateNormals()
    {
        faceMesh.RecalculateNormals();
        leftEyeMesh.normals = rightEyeMesh.normals = mouseMesh.normals = faceMesh.normals;
    }

    public void setLeftEyeTexture(Texture leftEyeTexture)
    {
        leftEye.GetComponent<Renderer>().material.SetTexture("_MainTex", leftEyeTexture);
    }

    public void setLeftEyeUV(Vector2[] uv)
    {
        leftEyeMesh.uv = uv;
    }

    public void setRightEyeTexture(Texture rightEyeTexture)
    {
        rightEye.GetComponent<Renderer>().material.SetTexture("_MainTex", rightEyeTexture);
    }

    public void setRightEyeUV(Vector2[] uv)
    {
        rightEyeMesh.uv = uv;
    }

    public void setMouseTexture(Texture mouseTexture)
    {
        mouse.GetComponent<Renderer>().material.SetTexture("_MainTex", mouseTexture);
    }

    public void setMouseUV(Vector2[] uv)
    {
        mouseMesh.uv = uv;
    }
}
