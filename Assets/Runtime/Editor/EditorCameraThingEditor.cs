#if UNITY_EDITOR
using SoldByWizards.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorCameraThing))]
public class EditorCameraThingInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorCameraThing scriptReference = (EditorCameraThing)target;
        if (GUILayout.Button ("Preview"))
        {
            scriptReference.PreviewCamera();
        }
        if (GUILayout.Button ("Take Photo"))
        {
            scriptReference.DoCamera();
        }
    }
}
#endif
