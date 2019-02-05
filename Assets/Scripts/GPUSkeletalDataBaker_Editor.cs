using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetupManager))]
public class GPUSkeletalDataBaker_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SetupManager setupManager = (SetupManager)target;
        if (GUILayout.Button("Bake Clip"))
        {
            //Display curves
            foreach (var binding in AnimationUtility.GetCurveBindings(setupManager.clip))
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(setupManager.clip, binding);
                Debug.Log((binding.path + "/" + binding.propertyName + ", Keys: " + curve.keys.Length));
                Keyframe key = curve.keys[0];
            }
            //Display rig
            Mesh mesh = setupManager.prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            Debug.Log(mesh.bindposes.Length);
            SkinnedMeshRenderer s;
            Animator a;
        }
    }
}
