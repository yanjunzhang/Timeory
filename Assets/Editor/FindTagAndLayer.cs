using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindTagAndLayer : EditorWindow {


    private string m_Tag = "Tag";
    private string m_Layer = "Layer";

    [MenuItem("Quibos/Component/Find Tag And Layer",false,23)]
    public static void OpenWindows()
    {
        EditorWindow window = GetWindow(typeof(FindTagAndLayer));
        window.titleContent.text = "Find Tag And Layer";
        window.minSize = new Vector2(300, 120);
        window.maxSize = new Vector2(300, 120);
        window.Show();
    }

    void OnGUI()
    {      
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal("Box");
        GUILayout.Label("Tag Value : ",GUILayout.Width(100));
        m_Tag = EditorGUILayout.TextArea(m_Tag, GUILayout.Height(20),GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal("Box");
        GUILayout.Label("Layer Value : ", GUILayout.Width(100));
        m_Layer = EditorGUILayout.TextArea(m_Layer, GUILayout.Height(20),GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        if (GUILayout.Button("Search",GUILayout.Height(70),GUILayout.Width(100))) {
            Search();
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
    }


    void Search()
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();

        if (string.IsNullOrEmpty(m_Tag)) Debug.LogWarning("Tag Value is null !");
        else {
            Selection.objects = GameObject.FindGameObjectsWithTag(m_Tag);
            foreach (var item in objs) {
                if (item.CompareTag(m_Tag)) {
                    GameObject obj = item;
                    string path = "/" + obj.name;
                    while (obj.transform.parent != null) {
                        obj = obj.transform.parent.gameObject;
                        path ="/" + obj.name + path;
                    }
                    Debug.Log("Tag Value is *** " + m_Tag + " *** Path : " + path);
                }              
                    
            }
        }

        if (string.IsNullOrEmpty(m_Layer)) Debug.LogWarning("Layer Value is null !");
        else {         
            foreach (var item in objs) {
                if (item.layer == LayerMask.NameToLayer(m_Layer)) {
                    GameObject obj = item;
                    string path = "/" + obj.name;
                    while (obj.transform.parent != null) {
                        obj = obj.transform.parent.gameObject;
                        path = "/" + obj.name + path;
                    }
                    Debug.Log("Layer Value is &&& " + m_Tag + " &&& Path : " + path);
                }                    
            }
        }
       
    }

}
