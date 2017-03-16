using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceControl : MonoBehaviour
{
    private Texture faceTexture;

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

        Mesh faceMesh = GetComponent<MeshFilter>().mesh = new Mesh();
        Mesh leftEyeMesh = leftEye.GetComponent<MeshFilter>().mesh = new Mesh();
        Mesh rightEyeMesh = rightEye.GetComponent<MeshFilter>().mesh = new Mesh();
        Mesh mouseMesh = mouse.GetComponent<MeshFilter>().mesh = new Mesh();

        faceMesh.vertices = leftEyeMesh.vertices = rightEyeMesh.vertices = mouseMesh.vertices = FileReader.getVertices("1.ver");

        faceMesh.uv = FileReader.getUV("face.uv");
        leftEyeMesh.uv = FileReader.getUV("face.uv");
        rightEyeMesh.uv = FileReader.getUV("face.uv");
        mouseMesh.uv = FileReader.getUV("face.uv");

        int[] tris = FileReader.getTris("face.tri");
        if (tris.Length >= 14 * 3)
        {
            int n = tris.Length / 3;
            faceMesh.triangles = FileReader.subTris(tris, 0, (n - 14) * 3, faceMesh.vertices);
            leftEyeMesh.triangles = FileReader.subTris(tris, (n - 14) * 3, 4 * 3, leftEyeMesh.vertices);
            rightEyeMesh.triangles = FileReader.subTris(tris, (n - 10) * 3, 4 * 3, rightEyeMesh.vertices);
            mouseMesh.triangles = FileReader.subTris(tris, (n - 6) * 3, 6 * 3, mouseMesh.vertices);
        }
        else
        {
            Debug.Log("Triangels number is not enough.");
        }

        faceMesh.RecalculateNormals();
        leftEyeMesh.normals = rightEyeMesh.normals = mouseMesh.normals = faceMesh.normals;

        faceTexture = FileReader.getTexture("zsyzgu.jpg");
        GetComponent<Renderer>().material.SetTexture("_MainTex", faceTexture);
        leftEye.GetComponent<Renderer>().material.SetTexture("_MainTex", faceTexture);
        rightEye.GetComponent<Renderer>().material.SetTexture("_MainTex", faceTexture);
        mouse.GetComponent<Renderer>().material.SetTexture("_MainTex", faceTexture);
    }

    /*void reduceMesh(ref Mesh mesh)
    {
        int n = mesh.vertices.Length;
        int[] mapping = new int[n];
        for (int i = 0; i < n; i++)
        {
            mapping[i] = -1;
        }
        int[] tris = mesh.triangles;
        for (int i = 0; i < tris.Length; i++)
        {
            mapping[tris[i]] = 0;
        }
        int id = 0;
        for (int i = 0; i < n; i++)
        {
            if (mapping[i] == 0)
            {
                mapping[i] = id++;
            }
        }
        Vector3[] vertices = new Vector3[id];
        Vector2[] uv = new Vector2[id];
        id = 0;
        for (int i = 0; i < n; i++)
        {
            if (mapping[i] == 0)
            {
                vertices[id] = mesh.vertices[i];
                uv[id] = mesh.uv[i];
                id++;
            }
        }
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = mapping[tris[i]];
        }
    }*/

	void Update () {
        /*Vector3[] vertices = getFace();
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i] + new Vector3(0, 0, 0.1f * Mathf.Sin(i * Time.time));
        }
        setFace(vertices);*/
    }

    public Vector3[] getFace()
    {
        return GetComponent<MeshFilter>().mesh.vertices;
    }

    public void setFace(Vector3[] vertices)
    {
        GetComponent<MeshFilter>().mesh.vertices = vertices;
    }
}
