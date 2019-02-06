using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GPUSkeletalAnim_Data", menuName = "GPUSkeletal/GPUAnim", order = 1)]
public class GPUSkeletalAnim_Data : ScriptableObject
{
    //Input
    public float samplingDelta = 0.1f;
    public GameObject model;

    //Baked Data
    public float bake_samplingDelta;
    public float bake_duration;
    public int bake_nbBones; 
    public Matrix4x4[] bake_data;


    public void Bake()
    {
        //Get objects
        GameObject skelModel = Instantiate(model);
        Animator skelAnimator = skelModel.GetComponent<Animator>();
        SkinnedMeshRenderer skelMeshRenderer = skelModel.GetComponentInChildren<SkinnedMeshRenderer>();
        AnimationClip clip = skelAnimator.GetCurrentAnimatorClipInfo(0)[0].clip;

        //Bake data
        bake_samplingDelta = samplingDelta;
        bake_duration = clip.length;
        bake_nbBones = skelMeshRenderer.bones.Length;
        bake_data = new Matrix4x4[bake_nbBones * (int) ((bake_duration / bake_samplingDelta) + 1)];
        int idx = 0;
        for (float t = 0; t < clip.length; t += samplingDelta)
        {
            skelAnimator.Update(samplingDelta);
            for (int i = 0; i < skelMeshRenderer.bones.Length; i++)
            {
                Transform bone = skelMeshRenderer.bones[i];
                bake_data[idx++] = bone.localToWorldMatrix;
            }
        }

        DestroyImmediate(skelModel);

        //Save data
#if UNITY_EDITOR
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif

        Debug.Log("Baking Complete !");
    }
}
