using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkeletalInst : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        int index = GPUSkeletalManager.Instance.Register(this);
        if (index >= 0)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Vector2[] uv2 = new Vector2[meshFilter.mesh.vertexCount];

            //Save uber mesh indices on uv5.x
            for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
            {
                uv2[i] = new Vector2(i + index, 0f);
            }
            meshFilter.mesh.uv2 = uv2;

            //Set compute buffer on material
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            ComputeBuffer buffer = GPUSkeletalManager.Instance._skinnedVerticesBuffer;
            meshRenderer.material.SetBuffer("_skinnedVertices", buffer);
        } else
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
