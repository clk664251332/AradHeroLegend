using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Quest))]
public class QuestInspector : Editor
{
    private SerializedObject obj;
    private QuestDataBase questDataBase;
    private Quest quest;
    private SerializedProperty type;
    public SerializedProperty NpcId;
    public SerializedProperty MonsterId;
    public SerializedProperty ItemId;
    public SerializedProperty LevelId;
    public SerializedProperty TriggerId;

    void OnEnable()
    {
        obj = new SerializedObject(target);
        NpcId = obj.FindProperty("NpcId");
        MonsterId = obj.FindProperty("MonsterId");
        ItemId = obj.FindProperty("ItemId");
        LevelId = obj.FindProperty("LevelId");
        TriggerId = obj.FindProperty("TriggerId");
    }

    public  void Draw()
    {
        base.OnInspectorGUI();
        //questDataBase = (QuestDataBase)target;

        //testA.type = (TestType)EditorGUILayout.EnumPopup("任务类型", testA.type);
        //if (testA.type == TestType.typeA)
        //{
        //    EditorGUILayout.PropertyField(width);
        //    EditorGUILayout.PropertyField(height);
        //}
    }
}
