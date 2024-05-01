using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OldOdin
{
    [CustomEditor(typeof(CreateCollidersScript))]
    public class CreateCollidersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CreateCollidersScript myScript = (CreateCollidersScript)target;
            if (GUILayout.Button("Create Colliders"))
            {
                myScript.Add();
            }
        }
    }
    
}