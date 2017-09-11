using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelLoader))]
public class LevelLoadorEditor : Editor
{
    private string m_folderName;
    private string m_folderPath;
    private string m_prefabPath;
    private string m_prefabName;

    private LevelLoader m_levelLoader;

    private Transform m_levelToSave;

    private void OnEnable()
    {
        m_levelLoader = (LevelLoader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //m_folderName = EditorGUILayout.TextField(m_folderName);
        //m_folderPath = EditorGUILayout.TextField(m_folderPath);
        //m_prefabPath = EditorGUILayout.TextField(m_prefabPath);
        m_prefabName = EditorGUILayout.TextField("Prefab Name", m_prefabName);

        if (GUILayout.Button("Create Level Prefab"))
        {
            m_levelLoader.CreateLevel();
        }
        if (GUILayout.Button("SavePrefab"))
        {
            SaveLevel();
        }
    }

    private void SaveLevel()
    {
        if (m_levelLoader.GetLevelParent())
        {
            //AssetDatabase.CreateAsset(m_levelLoader.GetLevelParent(), "Assets/Prefabs/Levels" + m_prefabName + ".prefab");
            PrefabUtility.CreatePrefab("Assets/Prefabs/Levels/" + m_prefabName + ".prefab", m_levelLoader.GetLevelParent().gameObject);
        }
    }
}