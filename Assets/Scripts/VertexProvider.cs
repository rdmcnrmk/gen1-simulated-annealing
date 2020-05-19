using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexProvider
{
    private Vector3[] vertices;
    private Transform trans;

    public VertexProvider(Mesh mesh, Transform trans)
    {
        vertices = mesh.vertices;
        this.trans = trans;
    }

    public List<Vector3> GetVertexSamples(int sampleCount)
    {
        List<Vector3> result = new List<Vector3>(sampleCount);
        vertices.Shuffle();

        for(int i = 0; i < sampleCount; i++)
        {
            result.Add(trans.TransformPoint(vertices[i]));
        }
        Debug.Log(result.Count);
        return result;
    }
}
