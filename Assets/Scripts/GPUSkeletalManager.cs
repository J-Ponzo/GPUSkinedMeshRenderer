using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkeletalManager : Singleton<GPUSkeletalManager>
{
    public List<GPUSkeletalInst> registeredInstances;

    //Uber mesh info
    public Mesh meshDef;
    public int maxVertex = 4194304;    //1024 * 1024 * 2 > 2065408 = 1024 * 2017 = nbMeshes * nbVertInMesh
    public int nbInstances = 0;

    public ComputeShader computeShaderDef;
    private int kernelMainCS;
    private ComputeShader computeShader;
    private ComputeBuffer _baseVerticesBuffer;
    private Vector3[] _baseVerticesData;
    public ComputeBuffer _boneWeightsBuffer;
    private BoneWeightsStruct[] _boneWeightsData;
    public ComputeBuffer _skinnedVerticesBuffer;
    private Vector3[] _skinnedVerticesData;
    public ComputeBuffer _animDataBuffer;
    private Matrix4x4[] _animDataData;

    //Anim info
    public GPUSkeletalAnim_Data animData;

    public float clipTime;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        registeredInstances = new List<GPUSkeletalInst>();

        //// Init data from mesh
        _baseVerticesData = new Vector3[meshDef.vertexCount];
        for (int i = 0; i < meshDef.vertexCount; i++)
        {
            _baseVerticesData[i] = meshDef.vertices[i];
        }
        _boneWeightsData = new BoneWeightsStruct[meshDef.boneWeights.Length];
        for (int i = 0; i < meshDef.boneWeights.Length; i++)
        {
            BoneWeightsStruct boneWeights = new BoneWeightsStruct();
            boneWeights.boneIdx_0 = meshDef.boneWeights[i].boneIndex0;
            boneWeights.boneIdx_1 = meshDef.boneWeights[i].boneIndex1;
            boneWeights.boneIdx_2 = meshDef.boneWeights[i].boneIndex2;
            boneWeights.boneIdx_3 = meshDef.boneWeights[i].boneIndex3;
            boneWeights.boneWeight_0 = meshDef.boneWeights[i].weight0;
            boneWeights.boneWeight_1 = meshDef.boneWeights[i].weight1;
            boneWeights.boneWeight_2 = meshDef.boneWeights[i].weight2;
            boneWeights.boneWeight_3 = meshDef.boneWeights[i].weight3;
            _boneWeightsData[i] = boneWeights;
        }

        _animDataData = new Matrix4x4[animData.bake_data.Length];
        for (int i = 0; i < animData.bake_data.Length; i++)
        {
            _animDataData[i] = animData.bake_data[i];
        }

        _skinnedVerticesData = new Vector3[maxVertex];

        // Init compute shader
        computeShader = (ComputeShader)Instantiate(computeShaderDef);
        kernelMainCS = computeShader.FindKernel("CSMain");

        _baseVerticesBuffer = new ComputeBuffer(_baseVerticesData.Length, 3 * 4);
        _baseVerticesBuffer.SetData(_baseVerticesData);
        computeShader.SetBuffer(kernelMainCS, "_baseVertices", _baseVerticesBuffer);
        _boneWeightsBuffer = new ComputeBuffer(_boneWeightsData.Length, 8 * 4);
        _boneWeightsBuffer.SetData(_boneWeightsData);
        computeShader.SetBuffer(kernelMainCS, "_boneWeights", _boneWeightsBuffer);
        _animDataBuffer = new ComputeBuffer(_animDataData.Length, 16 * 4);
        _animDataBuffer.SetData(_animDataData);
        computeShader.SetBuffer(kernelMainCS, "_animData", _animDataBuffer);

        _skinnedVerticesBuffer = new ComputeBuffer(_skinnedVerticesData.Length, 3 * 4);
        computeShader.SetBuffer(kernelMainCS, "_skinnedVertices", _skinnedVerticesBuffer);

        computeShader.SetFloat("_t", t);
        computeShader.SetInt("_instVCount", meshDef.vertexCount);
        computeShader.SetInt("_nbPoses", animData.bake_data.Length / animData.bake_nbBones);
        computeShader.SetInt("_nbBones", animData.bake_nbBones);
    }

    // Update is called once per frame
    void Update()
    {
        clipTime += Time.deltaTime;
        if (clipTime > animData.bake_duration)
        {
            clipTime = 0f;
        }
        t = clipTime / animData.bake_duration;

        ExecComputeShader();
    }

    void OnDrawGizmos()
    {
        int idx = 0;
        float nbPoses = animData.bake_data.Length / animData.bake_nbBones;
        float curPose = 0;
        while (idx < animData.bake_data.Length)
        {
            Gizmos.color = Color.Lerp(Color.red, Color.green, curPose / nbPoses);
            for (int i = 0; i < animData.bake_nbBones; i++)
            {
                Vector3 position = animData.bake_data[idx++].GetColumn(3);
                Gizmos.DrawSphere(position, 0.03f);
            }
            curPose++;
        }
    }

    private void ExecComputeShader()
    {
        int nbVert = nbInstances * meshDef.vertexCount;
        if (nbVert < 1) return;

        computeShader.SetFloat("_t", t);
        computeShader.Dispatch(kernelMainCS, nbVert / 1024, 1, 1);
        //_skinnedVerticesBuffer.GetData(_skinnedVerticesData);
    }

    private void OnDestroy()
    {
        if (_baseVerticesBuffer != null) _baseVerticesBuffer.Dispose();
        if (_skinnedVerticesBuffer != null) _skinnedVerticesBuffer.Dispose();
        if (_animDataBuffer != null) _animDataBuffer.Dispose();
        if (_boneWeightsBuffer != null) _boneWeightsBuffer.Dispose();
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
