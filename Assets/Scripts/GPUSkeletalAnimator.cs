using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkeletalAnimator : MonoBehaviour
{
    private Mesh mesh;

    public ComputeShader computeShaderDef;

    private int kernelMainCS;
    private ComputeShader computeShader;
    private ComputeBuffer _baseVerticesBuffer;
    private Vector3[] _baseVerticesData;
    private ComputeBuffer _skinnedVerticesBuffer;
    private Vector3[] _skinnedVerticesData;

    public float clipLength;
    public float clipTime;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        // Init data from mesh
        mesh = this.GetComponent<MeshFilter>().mesh;
        _baseVerticesData = new Vector3[mesh.vertexCount];
        _skinnedVerticesData = new Vector3[mesh.vertexCount];
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            _baseVerticesData[i] = mesh.vertices[i];
        }

        // Init compute shader
        computeShader = (ComputeShader)Instantiate(computeShaderDef);
        kernelMainCS = computeShader.FindKernel("CSMain");
        _baseVerticesBuffer = new ComputeBuffer(_baseVerticesData.Length, 3 * 4);
        _baseVerticesBuffer.SetData(_baseVerticesData);
        computeShader.SetBuffer(kernelMainCS, "_baseVertices", _baseVerticesBuffer);
        _skinnedVerticesBuffer = new ComputeBuffer(_skinnedVerticesData.Length, 3 * 4);
        computeShader.SetBuffer(kernelMainCS, "_skinnedVertices", _skinnedVerticesBuffer);
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

        ExecComputeShader();
    }

    private void ExecComputeShader()
    {
        computeShader.SetFloat("_t", t);
        computeShader.Dispatch(kernelMainCS, mesh.vertexCount / 8, 1, 1);
        _skinnedVerticesBuffer.GetData(_skinnedVerticesData);

    }
}
