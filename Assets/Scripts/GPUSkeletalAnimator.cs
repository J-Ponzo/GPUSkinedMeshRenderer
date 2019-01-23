using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkeletalAnimator : MonoBehaviour
{
    private Mesh mesh;

    public float clipLength;
    public float clipTime;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        mesh = this.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        clipTime += Time.deltaTime;
        if (clipTime > clipLength)
        {
            clipTime = 0f;
        }
        t = clipTime / clipLength;

        UpdateVertices();
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            mesh.vertices[i].x = mesh.vertices[(i + 1) % mesh.vertexCount].x;
            mesh.vertices[i].y = mesh.vertices[(i + 1) % mesh.vertexCount].x;
            mesh.vertices[i].z = mesh.vertices[(i + 1) % mesh.vertexCount].x;
        }
    }
}
