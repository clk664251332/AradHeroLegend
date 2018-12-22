using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CustomEditor(typeof(QuestDataBase))]
public class QuestDataBaseInspector : Editor
{
    private ReorderableList m_list = null;
    private SerializedProperty m_selectedQuest = null;
    //private Quest m_selectedQuest = null;
    //private Editor QuestEditor = null;

    void OnEnable()
    {
        //绘制元素
        var props = serializedObject.FindProperty("m_allQuest");
        m_list = new ReorderableList(serializedObject, props);
        m_selectedQuest = null;

        m_list.drawHeaderCallback = (rect) =>
        {
            EditorGUI.LabelField(rect, "任务列表", EditorStyles.boldLabel);
        };
            //绘制文本框
            m_list.drawElementCallback = (rect, index, isActive, isFocused) => {
            if (!(0 <= index && index < m_list.serializedProperty.arraySize)) return;
            var element = props.GetArrayElementAtIndex(index);
            if (element == null) return;
            rect.height -= 4;
            rect.y += 2;
            string titleText = "任务" + element.FindPropertyRelative("id").intValue.ToString() +
            " :"+element.FindPropertyRelative("title").stringValue;
            EditorGUI.LabelField(rect, titleText);
        };

        m_list.onSelectCallback = (list) =>
        {
            var isIndexValid = (0 <= list.index && list.index < list.count);
            m_selectedQuest = isIndexValid ? list.serializedProperty.GetArrayElementAtIndex(list.index) : null;
        };

        //增加元素回调
        m_list.onAddCallback += (list) => {

            //添加元素
            props.arraySize++;

            //选择状态设置为最后一个元素
            list.index = props.arraySize - 1;
        };

        //删除元素回调
        m_list.onRemoveCallback += (list) => {

            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            var isIndexValid = (0 <= list.index && list.index < list.count);
            m_selectedQuest = isIndexValid ? list.serializedProperty.GetArrayElementAtIndex(list.index) : null;
        };

        //重新排序回调
        m_list.onReorderCallback = (list) => {
            //元素更新
            Debug.Log("onReorderCallback");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        m_list.DoLayoutList();
        DrawSelectedUIContent();
        serializedObject.ApplyModifiedProperties();
    }


    private void DrawSelectedUIContent()
    {
        if (m_selectedQuest == null) return;
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(m_selectedQuest, true);
        //if(QuestEditor==null)
            //QuestEditor = Editor.CreateEditor(m_selectedQuest);
        //QuestEditor.OnInspectorGUI();
        EditorGUI.indentLevel--;
    }
}
