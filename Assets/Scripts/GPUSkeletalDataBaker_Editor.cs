using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GPUSkeletalAnim_Data))]
public class GPUSkeletalDataBaker_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Bake Clip"))
        {
            ((GPUSkeletalAnim_Data)target).Bake();
        }

        //SetupManager setupManager = (SetupManager)target;
        //if (GUILayout.Button("Bake Clip"))
        //{
        //    //Display curves
        //    foreach (var binding in AnimationUtility.GetCurveBindings(setupManager.clip))
        //    {
        //        AnimationCurve curve = AnimationUtility.GetEditorCurve(setupManager.clip, binding);
        //        Debug.Log(/*binding.path + "/" + */ binding.propertyName + "    nbKeys" + curve.keys.Length);
        //    }

        //    //Display rig
        //    Mesh mesh = setupManager.prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        //    //Debug.Log(mesh.bindposes.Length);
        //    AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(setupManager.model));
        //    ModelImporter modelImporter = importer as ModelImporter;
        //    HumanBone[] humanBones = modelImporter.humanDescription.human;
        //    foreach (HumanBone humanBone in humanBones)
        //    {
        //        //Debug.Log("bonename = " + humanBone.boneName + " humanName = " + humanBone.humanName);
        //    }

        //    for (int i = 0; i < HumanTrait.BoneName.Length; i++)
        //    {
        //        Debug.Log(HumanTrait.BoneName[i] + " => "
        //            + (HumanTrait.MuscleFromBone(i, 0) == -1 ? "\n\tnull" : ("\n\t" + HumanTrait.MuscleName[HumanTrait.MuscleFromBone(i, 0)])) + ", "
        //            + (HumanTrait.MuscleFromBone(i, 1) == -1 ? "\n\tnull" : ("\n\t" + HumanTrait.MuscleName[HumanTrait.MuscleFromBone(i, 1)])) + ", "
        //            + (HumanTrait.MuscleFromBone(i, 2) == -1 ? "\n\tnull" : ("\n\t" + HumanTrait.MuscleName[HumanTrait.MuscleFromBone(i, 2)])) + ", "
        //            + (HumanTrait.MuscleFromBone(i, 3) == -1 ? "\n\tnull" : ("\n\t" + HumanTrait.MuscleName[HumanTrait.MuscleFromBone(i, 3)])) + ", "
        //            + (HumanTrait.MuscleFromBone(i, 4) == -1 ? "\n\tnull" : ("\n\t" + HumanTrait.MuscleName[HumanTrait.MuscleFromBone(i, 4)])) + ", "
        //            + (HumanTrait.MuscleFromBone(i, 5) == -1 ? "\n\tnull" : ("\n\t" + HumanTrait.MuscleName[HumanTrait.MuscleFromBone(i, 5)])));
        //    }
        //    //Debug.Log("##########################################");
        //    //for (int i = 0; i < HumanTrait.MuscleName.Length; i++)
        //    //{
        //    //    Debug.Log(HumanTrait.MuscleName[i]);
        //    //}

        //    //Transform root = FindRecursive(setupManager.model.transform, modelImporter.humanDescription.human[0].boneName);
        //    //HumanPoseHandler handler = new HumanPoseHandler(setupManager.avatar, root);
        //    //HumanPose humanPose = new HumanPose();
        //    //handler.GetHumanPose(ref humanPose);
        //}

        //Transform FindRecursive(Transform root, string name)
        //{
        //    Transform node = root.Find(name);
        //    if (node) return node;
        //    for (int i = 0; i < node.childCount; i++)
        //    {
        //        node = FindRecursive(node.GetChild(i), name);
        //        if (node) return node;
        //    }
        //    return null;
        //}
    }
}
