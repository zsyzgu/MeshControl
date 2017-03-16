using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReader {
    static string[] getTags(string pathName)
    {
        string str = File.ReadAllText("Assets\\Datas\\" + pathName);
        List<string> list = new List<string>();
        int st = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != '.' && str[i] != '-' && !('0' <= str[i] && str[i] <= '9'))
            {
                if (st < i)
                {
                    list.Add(str.Substring(st, i - st));
                }
                st = i + 1;
            }
        }
        return list.ToArray();
    }

    static public Texture getTexture(string pathName)
    {
        byte[] data = File.ReadAllBytes("Assets\\Pictures\\" + pathName);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(data);
        return texture;
    }

    static public int[] getTris(string pathName)
    {
        string[] tags = getTags(pathName);
        if (tags.Length % 3 != 0)
        {
            Debug.Log("Get Tris wrong from " + pathName);
        }
        int n = tags.Length;
        int[] tris = new int[n];
        for (int i = 0; i < n; i++)
        {
            tris[i] = int.Parse(tags[i]);
        }
        return tris;
    }

    static public int[] subTris(int[] tris, int s, int len, Vector3[] vertices)
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

    static public Vector3[] getVertices(string pathName)
    {
        string[] tags = getTags(pathName);
        if (tags.Length % 3 != 0)
        {
            Debug.Log("Get Vertices wrong from " + pathName);
        }
        int n = tags.Length / 3;
        Vector3[] vertices = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            vertices[i] = new Vector3(float.Parse(tags[i * 3]), -float.Parse(tags[i * 3 + 1]), float.Parse(tags[i * 3 + 2]) * 0.5f);
        }
        return vertices;
    }

    static public Vector2[] getUV(string pathName)
    {
        string[] tags = getTags(pathName);
        if (tags.Length % 2 != 0)
        {
            Debug.Log("Get UV wrong from " + pathName);
        }
        int n = tags.Length / 2;
        Vector2[] uv = new Vector2[n];
        for (int i = 0; i < n; i++)
        {
            uv[i] = new Vector2(float.Parse(tags[i * 2]), 1 - float.Parse(tags[i * 2 + 1]));
        }
        return uv;
    }
}
