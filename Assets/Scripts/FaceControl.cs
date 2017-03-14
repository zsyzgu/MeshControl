using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceControl : MonoBehaviour {
    public Vector3[] vertices;
    public Vector2[] uv;
    public int[] triangles;

    void Start () {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = FileReader.getVertices("1.ver");
        mesh.uv = FileReader.getUV("face.uv");
        mesh.triangles = FileReader.getTris("face.tri");
	}
	
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
