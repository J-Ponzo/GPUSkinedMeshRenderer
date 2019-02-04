using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkinningManager : Singleton<GPUSkinningManager>
{
    public List<GPUSkeletalInst> registeredInstances;

    //Uber mesh info
    public Mesh meshDef;
    public int maxVertex = 2097152;    //1024 * 1024 * 2 > 2065408 = 1024 * 2017 = nbMeshes * nbVertInMesh
    public int nbInstances = 0;

    public ComputeShader computeShaderDef;
    private int kernelMainCS;
    private ComputeShader computeShader;
    private ComputeBuffer _baseVerticesBuffer;
    private Vector3[] _baseVerticesData;
    public ComputeBuffer _skinnedVerticesBuffer;
    private Vector3[] _skinnedVerticesData;

    public float clipLength;
    public float clipTime;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        registeredInstances = new List<GPUSkeletalInst>();

        //// Init data from mesh
        _baseVerticesData = new Vector3[meshDef.vertexCount];
        _skinnedVerticesData = new Vector3[maxVertex];
        for (int i = 0; i < meshDef.vertexCount; i++)
        {
            _baseVerticesData[i] = meshDef.vertices[i];
        }

        // Init compute shader
        computeShader = (ComputeShader)Instantiate(computeShaderDef);
        kernelMainCS = computeShader.FindKernel("CSMain");

        _baseVerticesBuffer = new ComputeBuffer(_baseVerticesData.Length, 3 * 4);
        _baseVerticesBuffer.SetData(_baseVerticesData);
        computeShader.SetBuffer(kernelMainCS, "_baseVertices", _baseVerticesBuffer);
        _skinnedVerticesBuffer = new ComputeBuffer(_skinnedVerticesData.Length, 3 * 4);
        computeShader.SetBuffer(kernelMainCS, "_skinnedVertices", _skinnedVerticesBuffer);

        computeShader.SetFloat("_t", t);
        computeShader.SetInt("_instVCount", meshDef.vertexCount);
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
        int nbVert = nbInstances * meshDef.vertexCount;
        if (nbVert < 1) return;

        computeShader.SetFloat("_t", t);
        computeShader.Dispatch(kernelMainCS, nbVert / 8, 1, 1);
        _skinnedVerticesBuffer.GetData(_skinnedVerticesData);
    }

    private void OnDestroy()
    {
        if (_baseVerticesBuffer != null) _baseVerticesBuffer.Dispose();
        if (_skinnedVerticesBuffer != null) _skinnedVerticesBuffer.Dispose();
    }

    public int Register(GPUSkeletalInst meshInstance)
    {
        int vertexStartOffset = nbInstances * meshDef.vertexCount;
        if (maxVertex > vertexStartOffset + meshDef.vertexCount)
        {
            registeredInstances.Add(meshInstance);
            nbInstances++;
            return vertexStartOffset;
        }
        return -1;
    }
}
