//https://forum.unity.com/threads/display-a-list-class-with-a-custom-editor-script.227847/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABMU.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AgentSpawnList : MonoBehaviour {
    [System.Serializable]
    public class AgentSpawn {
        public GameObject agentPf;
        public LayerMask layerMask;
        [Range(1, 100)]
        public int  iterations;

        public SpawnTypes spawnType;

        public Vector3Int point;

        public enum SpawnTypes {
            Random,
            Last,
            Point
        }
    }

    public List<AgentSpawn> agentSpawnList = new List<AgentSpawn>(1);
    
    public void AddSpawn() {
        agentSpawnList.Add(new AgentSpawn());
    }

    public void RemoveSpawn(int index) {
        agentSpawnList.RemoveAt(index);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AgentSpawnList))]
public class AgentSpawnListEditor : Editor {
    AgentSpawnList spawnList;
    SerializedObject objTarget;
    SerializedProperty thisList;
    int listSize;

    void OnEnable() {
        spawnList = (AgentSpawnList)target;
        objTarget = new SerializedObject(spawnList);
        thisList = objTarget.FindProperty("agentSpawnList");
    }

    public override void OnInspectorGUI() {
        objTarget.Update();
        
        listSize = thisList.arraySize;
        listSize = EditorGUILayout.IntField ("List Size", listSize);

        if (listSize != thisList.arraySize) {
            while (listSize > thisList.arraySize) {
                thisList.InsertArrayElementAtIndex(thisList.arraySize);
            }
            while (listSize < thisList.arraySize) {
                thisList.DeleteArrayElementAtIndex(thisList.arraySize - 1);
            }
        }

        if (GUILayout.Button("Add Agent")) {
            spawnList.agentSpawnList.Add(new AgentSpawnList.AgentSpawn());
        }

        for (int i = 0; i < thisList.arraySize; i++) {
            SerializedProperty listRef = thisList.GetArrayElementAtIndex(i);
            SerializedProperty agentPf = listRef.FindPropertyRelative("agentPf");
            SerializedProperty layerMask = listRef.FindPropertyRelative("layerMask");
            SerializedProperty iterations = listRef.FindPropertyRelative("iterations");
            SerializedProperty spawnType = listRef.FindPropertyRelative("spawnType");
            SerializedProperty point = listRef.FindPropertyRelative("point");

            EditorGUILayout.PropertyField(agentPf);
            EditorGUILayout.PropertyField(layerMask);
            EditorGUILayout.PropertyField(iterations);
            EditorGUILayout.PropertyField(spawnType);
            EditorGUILayout.PropertyField(point);

            if (GUILayout.Button("Remove Agent " + i.ToString())) thisList.DeleteArrayElementAtIndex(i);
        }
        
        objTarget.ApplyModifiedProperties();
    }
}
#endif