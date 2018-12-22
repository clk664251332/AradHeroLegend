using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CustomPropertyDrawer(typeof(Quest))]
public class QuestGUI : PropertyDrawer
{
    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    //if (property == null)
    //        return base.GetPropertyHeight(property, label);
    //}
    private int select = 0;
    private string[] names = { "领取对话", "进行对话", "结束对话"};

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, label, property);
        try
        {
            var nameProperty = property.FindPropertyRelative("title");
            if (nameProperty == null) return;
            var y = rect.y;
            EditorGUI.PropertyField(rect,nameProperty, new GUIContent("标题", "任务的名称"));

            y += EditorGUIUtility.singleLineHeight;
            var idProperty = property.FindPropertyRelative("id");
            if (idProperty == null) return;
            EditorGUI.PropertyField(new Rect(rect.x,y,rect.width,rect.height), idProperty, new GUIContent("ID", "任务的ID"));

            y += EditorGUIUtility.singleLineHeight;
            var descriptionProperty = property.FindPropertyRelative("description");
            if (descriptionProperty == null) return;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight*3), descriptionProperty, new GUIContent("描述", "任务的描述"));

            y += EditorGUIUtility.singleLineHeight*3;
            var stateProperty = property.FindPropertyRelative("state");
            if (stateProperty == null) return;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), stateProperty, new GUIContent("状态", "任务的状态"));

            y += EditorGUIUtility.singleLineHeight;
            var typeProperty = property.FindPropertyRelative("Type");
            if (typeProperty == null) return;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight), typeProperty, new GUIContent("条件", "任务的完成条件"));
            //任务类型
            if (typeProperty.enumValueIndex == (int)QuestType.对话)
            {
                y += EditorGUIUtility.singleLineHeight;
                var npcIdProperty = property.FindPropertyRelative("NpcId");
                if (npcIdProperty == null) return;
                //EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), npcIdProperty, new GUIContent("NpcID", "与之对话的NPC的ID"));
            }else if (typeProperty.enumValueIndex == (int)QuestType.打怪)
            {
                y += EditorGUIUtility.singleLineHeight;
                var monsterIdProperty = property.FindPropertyRelative("MonsterId");
                if (monsterIdProperty == null) return;
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), monsterIdProperty, new GUIContent("怪物ID", "需要击杀怪物的ID"));

                y += EditorGUIUtility.singleLineHeight;
                var monsterValueProperty = property.FindPropertyRelative("MonsterValue");
                if (monsterValueProperty == null) return;
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), monsterValueProperty, new GUIContent("怪物数量", "至少需要击杀怪物的数量"));
            }
            else if (typeProperty.enumValueIndex == (int)QuestType.收集物品)
            {
                y += EditorGUIUtility.singleLineHeight;
                var itemIdProperty = property.FindPropertyRelative("ItemId");
                if (itemIdProperty == null) return;
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), itemIdProperty, new GUIContent("物品ID", "需要收集物品的ID"));

                y += EditorGUIUtility.singleLineHeight;
                var itemValueProperty = property.FindPropertyRelative("ItemValue");
                if (itemValueProperty == null) return;
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), itemValueProperty, new GUIContent("物品数量", "至少需要物品的数量"));
            }else if (typeProperty.enumValueIndex == (int)QuestType.通关)
            {
                y += EditorGUIUtility.singleLineHeight;
                var levelIdProperty = property.FindPropertyRelative("LevelId");
                if (levelIdProperty == null) return;
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), levelIdProperty, new GUIContent("关卡ID", "关卡的ID"));
            }
            else if (typeProperty.enumValueIndex == (int)QuestType.到达目的地)
            {
                y += EditorGUIUtility.singleLineHeight;
                var triggerIdProperty = property.FindPropertyRelative("TriggerId");
                if (triggerIdProperty == null) return;
                EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), triggerIdProperty, new GUIContent("目的地ID", "所要到达地点的ID"));
            }
            //任务NPC
            y += EditorGUIUtility.singleLineHeight*2;
            var startNpcIdProperty = property.FindPropertyRelative("StartNpcId");
            if (startNpcIdProperty == null) return;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), startNpcIdProperty, new GUIContent("领取任务NpcID", "领取任务NpcID"));

            y += EditorGUIUtility.singleLineHeight;
            var endNpcIdProperty = property.FindPropertyRelative("EndNpcId");
            if (endNpcIdProperty == null) return;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), endNpcIdProperty, new GUIContent("提交任务NpcID", "提交任务NpcID"));
            //导航栏
            //EditorGUILayout.BeginVertical();
            //创建 Toolbar  ， 返回值为选中的项， 参数1 为 选中的项， 参数 2 每项上的文字  
            //select = GUILayout.Toolbar(select, names,GUILayout.);
            //switch (select)
            //{
            //    case 0:
            //        GUILayout.Label("1111111111111111111");
            //        break;
            //    case 1:
            //        GUILayout.Label("2222222222222222222");
            //        break;
            //    case 2:
            //        GUILayout.Label("3333333333333333333");
            //        break;
            //}
            EditorGUILayout.Space();
           // EditorGUILayout.EndVertical();


            //对话内容
            y += EditorGUIUtility.singleLineHeight;
           var start_npc_dialogProperty = property.FindPropertyRelative("start_npc_dialog");
           if (endNpcIdProperty == null) return;
           EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), start_npc_dialogProperty, new GUIContent("任务领取对话", "任务领取对话"),true);

           y += EditorGUIUtility.singleLineHeight*3;
           var ing_npc_dialogProperty = property.FindPropertyRelative("ing_npc_dialog");
           if (endNpcIdProperty == null) return;
           EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), ing_npc_dialogProperty, new GUIContent("任务进行对话", "任务进行对话"), true);

            y += EditorGUIUtility.singleLineHeight*3;
           var end_npc_dialogProperty = property.FindPropertyRelative("end_npc_dialog");
           if (endNpcIdProperty == null) return;
           EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, rect.height), end_npc_dialogProperty, new GUIContent("任务提交对话", "任务提交对话"), true);


        }
        finally
        {
            EditorGUI.EndProperty();
        }
    }


}
