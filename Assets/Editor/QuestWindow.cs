using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class QuestWindow : EditorWindow
{
    /// <summary>
    /// 该窗口有两个列表，任务列表与对话列表
    /// 在任务列表中点击任务可以在右侧显示任务检视窗口，并且包含一个对话的列表
    /// </summary>
    private const float DESCRIPTION_HEIGHT = 54f;
    private const float PROPERTY_HEIGHT = 40f;
    private const float RAW_LIST_HEIGHT = 38f;

    private SerializedObject m_QuestDataBase;
    private ReorderableList m_QuestList;
    private ReorderableList m_StartDialogs;//对话列表在点击事件中创建并显示
    private ReorderableList m_IngDialogs;
    private ReorderableList m_EndDialogs;
    private ReorderableList m_TriggerDialogs;
    private ReorderableList m_RewardItemIds;

    private Vector2 m_QuestsScrollPos;
    private Vector2 m_DialogsScrollPos;

    [MenuItem("Tools/我的项目/任务编辑器")]
    public static void Init()
    {
        //获得窗口实例，打开窗口
        EditorWindow.GetWindow<QuestWindow>(true, "任务编辑器");
    }
    //---------------------------------------------------------
    private void OnEnable()
    {
        InitializeWindow();

        Undo.undoRedoPerformed += Repaint;
    }
    /// <summary>
    /// 准备好相关的数据以及需要显示组件的创建
    /// </summary>
    private void InitializeWindow()
    {
        var database = Resources.LoadAll<QuestDataBase>("")[0];
        m_QuestDataBase = new SerializedObject(database);
        //创建任务列表
        m_QuestList = new ReorderableList(m_QuestDataBase, m_QuestDataBase.FindProperty("m_allQuest"), true, true, true, true);
        m_QuestList.drawElementCallback += DrawQuestListElement;
        m_QuestList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, ""); };
        m_QuestList.onSelectCallback += On_SelectedQuest;
        m_QuestList.onRemoveCallback = (ReorderableList list) => { m_QuestList.serializedProperty.DeleteArrayElementAtIndex(m_QuestList.index); };
    }
    //---------------------------------------------------------
    public void OnGUI()
    {
        if(m_QuestDataBase==null)
            EditorGUILayout.HelpBox("找不到数据库！", MessageType.Error);

        //绘制数据库的路径
        EditorGUILayout.LabelField(string.Format("Database path: '{0}'", AssetDatabase.GetAssetPath(m_QuestDataBase.targetObject)));

        //绘制分割线
        float topPadding = 25f;
        GUI.backgroundColor = Color.white;
        GUI.Box(new Rect(0f, topPadding, position.width, 1f), "");
        m_QuestDataBase.Update();

        //绘制矩形区域背景
        float innerWindowPadding = 8f;
        Rect innerWindowRect = new Rect(innerWindowPadding, topPadding + innerWindowPadding, position.width - innerWindowPadding * 2f, position.height - (topPadding + innerWindowPadding * 4.5f));
        GUI.backgroundColor = Color.grey;
        GUI.Box(innerWindowRect, "");
        GUI.backgroundColor = Color.white;
        //绘制中间垂直的分割线
        GUI.Box(new Rect(innerWindowRect.x + innerWindowRect.width * 0.5f, innerWindowRect.y, 1f, innerWindowRect.height), "");
        //绘制任务列表标签
        Vector2 labelSize = new Vector2(192f, 20f);
        GUI.Box(new Rect(innerWindowRect.x + innerWindowRect.width * 0.25f - labelSize.x * 0.5f, innerWindowRect.y, labelSize.x, labelSize.y), "任务列表");
        //绘制任务列表
        DrawList(m_QuestList, new Rect(innerWindowRect.x, innerWindowRect.y + labelSize.y, innerWindowRect.width * 0.5f - 2f, innerWindowRect.height - labelSize.y - 3f), ref m_QuestsScrollPos);
        //绘制任务编辑标签
        GUI.Box(new Rect(innerWindowRect.x + innerWindowRect.width * 0.75f - labelSize.x * 0.5f, innerWindowRect.y, labelSize.x, labelSize.y), "任务编辑");
        //绘制任务编辑界面
        bool itemIsSelected = m_QuestList != null && m_QuestList.count != 0 && m_QuestList.index != -1 && m_QuestList.index < m_QuestList.count&& m_StartDialogs != null;
        Rect inspectorRect = new Rect(innerWindowRect.x + innerWindowRect.width * 0.5f + 4f, innerWindowRect.y + labelSize.y, innerWindowRect.width * 0.5f - 5f, innerWindowRect.height - labelSize.y - 1f);
        if (itemIsSelected)
            DrawQuestInspector(inspectorRect);
        else
        {
            inspectorRect.x += 4f;
            inspectorRect.y += 4f;
            GUI.Label(inspectorRect, "请选择一个任务进行编辑", new GUIStyle() { fontSize = 15});
        }

        //应用属性更改
        m_QuestDataBase.ApplyModifiedProperties();
    }
    //---------------------------------------------------------
    //绘制整个列表的显示
    private void DrawList(ReorderableList list, Rect totalRect, ref Vector2 scrollPosition)
    {
        float scrollbarWidth = 16f;

        Rect onlySeenRect = new Rect(totalRect.x, totalRect.y, totalRect.width, totalRect.height);
        Rect allContentRect = new Rect(totalRect.x, totalRect.y, totalRect.width - scrollbarWidth, (list.count + 4) * list.elementHeight);

        scrollPosition = GUI.BeginScrollView(onlySeenRect, scrollPosition, allContentRect, false, true);

        // Draw the clear button.
        Vector2 buttonSize = new Vector2(56f, 16f);

        if (list.count > 0 && GUI.Button(new Rect(allContentRect.x + 2f, allContentRect.yMax - 60f, buttonSize.x, buttonSize.y), "Clear"))
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want the list to be cleared? (All elements will be deleted)", "Yes", "Cancel"))
                list.serializedProperty.ClearArray();

        list.DoList(allContentRect);

        GUI.EndScrollView();
    }
    //绘制任务元素在列表中的显示
    private void DrawQuestListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (m_QuestList.serializedProperty.arraySize == index)
            return;

        rect.y += 2;
        var element = m_QuestList.serializedProperty.GetArrayElementAtIndex(index);
        var name = element.FindPropertyRelative("title");
        //显示元素名称
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 256f, 16f), name.stringValue);
    }
    //绘制任务元素的选择事件
    private void On_SelectedQuest(ReorderableList list)
    {
        if (m_QuestList == null || m_QuestList.count == 0 || m_QuestList.index == -1 || m_QuestList.index >= m_QuestList.count)
            return;
        m_StartDialogs = new ReorderableList(m_QuestDataBase, m_QuestList.serializedProperty.GetArrayElementAtIndex(m_QuestList.index).FindPropertyRelative("start_npc_dialog"));
        m_StartDialogs.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, ""); };
        m_StartDialogs.drawElementCallback += DrawStartQuestDialogs;
        m_StartDialogs.elementHeight = DESCRIPTION_HEIGHT;

        m_IngDialogs = new ReorderableList(m_QuestDataBase, m_QuestList.serializedProperty.GetArrayElementAtIndex(m_QuestList.index).FindPropertyRelative("ing_npc_dialog"));
        m_IngDialogs.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, ""); };
        m_IngDialogs.drawElementCallback += DrawIngQuestDialogs;
        m_IngDialogs.elementHeight = DESCRIPTION_HEIGHT;

        m_EndDialogs = new ReorderableList(m_QuestDataBase, m_QuestList.serializedProperty.GetArrayElementAtIndex(m_QuestList.index).FindPropertyRelative("end_npc_dialog"));
        m_EndDialogs.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, ""); };
        m_EndDialogs.drawElementCallback += DrawEndQuestDialogs;
        m_EndDialogs.elementHeight = DESCRIPTION_HEIGHT;

        m_TriggerDialogs = new ReorderableList(m_QuestDataBase, m_QuestList.serializedProperty.GetArrayElementAtIndex(m_QuestList.index).FindPropertyRelative("tigger_dialog"));
        m_TriggerDialogs.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, ""); };
        m_TriggerDialogs.drawElementCallback += DrawTriggerQuestDialogs;
        m_TriggerDialogs.elementHeight = DESCRIPTION_HEIGHT;

        m_RewardItemIds = new ReorderableList(m_QuestDataBase, m_QuestList.serializedProperty.GetArrayElementAtIndex(m_QuestList.index).FindPropertyRelative("rewardItems"));
        m_RewardItemIds.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, ""); };
        m_RewardItemIds.drawElementCallback += DrawRewardItemIds;
        m_RewardItemIds.elementHeight = DESCRIPTION_HEIGHT;

    }
    //绘制开始对话列表的显示
    private void DrawStartQuestDialogs(Rect rect, int index, bool isActive, bool isFocused)
    {
        var list = m_StartDialogs;

        if (list.serializedProperty.arraySize == index)
            return;

        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2f;
        rect.height -= 2f;
        
        var name= element.FindPropertyRelative("name");
        var portrait = element.FindPropertyRelative("portrait");
        var saying = element.FindPropertyRelative("sentence");

        EditorStyles.textArea.wordWrap = true;
        saying.stringValue = EditorGUI.TextArea(new Rect(rect.x,rect.y,rect.width *0.65f, rect.height), saying.stringValue, EditorStyles.textArea);
        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y, 30f, 16f), "名字: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y, 66f, 20f), name, GUIContent.none);

        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y+25f, 30f, 16f), "头像: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y + 25f, 66f, 20f), portrait, GUIContent.none);
    }
    //绘制进行对话列表的显示
    private void DrawIngQuestDialogs(Rect rect, int index, bool isActive, bool isFocused)
    {
        var list = m_IngDialogs;

        if (list.serializedProperty.arraySize == index)
            return;

        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2f;
        rect.height -= 2f;

        var name = element.FindPropertyRelative("name");
        var portrait = element.FindPropertyRelative("portrait");
        var saying = element.FindPropertyRelative("sentence");

        saying.stringValue = EditorGUI.TextArea(new Rect(rect.x, rect.y, rect.width * 0.65f, rect.height), saying.stringValue, EditorStyles.textArea);
        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y, 30f, 16f), "名字: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y, 66f, 20f), name, GUIContent.none);

        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y + 25f, 30f, 16f), "头像: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y + 25f, 66f, 20f), portrait, GUIContent.none);
    }
    //绘制结束对话列表的显示
    private void DrawEndQuestDialogs(Rect rect, int index, bool isActive, bool isFocused)
    {
        var list = m_EndDialogs;

        if (list.serializedProperty.arraySize == index)
            return;

        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2f;
        rect.height -= 2f;

        var name = element.FindPropertyRelative("name");
        var portrait = element.FindPropertyRelative("portrait");
        var saying = element.FindPropertyRelative("sentence");

        saying.stringValue = EditorGUI.TextArea(new Rect(rect.x, rect.y, rect.width * 0.65f, rect.height), saying.stringValue, EditorStyles.textArea);
        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y, 30f, 16f), "名字: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y, 66f, 20f), name, GUIContent.none);

        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y + 25f, 30f, 16f), "头像: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y + 25f, 66f, 20f), portrait, GUIContent.none);
    }
    //绘制区域触发对话列表的显示
    private void DrawTriggerQuestDialogs(Rect rect, int index, bool isActive, bool isFocused)
    {
        var list = m_TriggerDialogs;

        if (list.serializedProperty.arraySize == index)
            return;

        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2f;
        rect.height -= 2f;

        var name = element.FindPropertyRelative("name");
        var portrait = element.FindPropertyRelative("portrait");
        var saying = element.FindPropertyRelative("sentence");

        saying.stringValue = EditorGUI.TextArea(new Rect(rect.x, rect.y, rect.width * 0.65f, rect.height), saying.stringValue, EditorStyles.textArea);
        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y, 30f, 16f), "名字: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y, 66f, 20f), name, GUIContent.none);

        GUI.Label(new Rect(rect.x + rect.width * 0.65f, rect.y + 25f, 30f, 16f), "头像: ");
        EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.65f + 35f, rect.y + 25f, 66f, 20f), portrait, GUIContent.none);
    }
    //绘制奖励物品列表的显示
    private void DrawRewardItemIds(Rect rect, int index, bool isActive, bool isFocused)
    {
        var list = m_RewardItemIds;

        if (list.serializedProperty.arraySize == index)
            return;

        rect.y += 2;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        var rewardItemId = element.FindPropertyRelative("itemId");
        var rewardItemAmount = element.FindPropertyRelative("amount");
        //显示奖励物品id
        GUI.Label(new Rect(rect.x , rect.y, 30f, 16f), "ID: ");
        EditorGUI.PropertyField(new Rect(rect.x  + 35f, rect.y, 66f, 20f), rewardItemId, GUIContent.none);

        //显示奖励物品数量
        GUI.Label(new Rect(rect.x , rect.y + 25f, 30f, 16f), "数量: ");
        EditorGUI.PropertyField(new Rect(rect.x  + 35f, rect.y + 25f, 66f, 20f), rewardItemAmount, GUIContent.none);
    }
    //绘制任务编辑界面的显示
    private void DrawQuestInspector(Rect totalRect)
    {
        SerializedProperty item = m_QuestList.serializedProperty.GetArrayElementAtIndex(m_QuestList.index);

        SerializedProperty title = item.FindPropertyRelative("title");
        SerializedProperty id = item.FindPropertyRelative("id");
        SerializedProperty description = item.FindPropertyRelative("description");
        SerializedProperty state = item.FindPropertyRelative("state");
        SerializedProperty type = item.FindPropertyRelative("Type");
        SerializedProperty NpcId = item.FindPropertyRelative("NpcId");
        SerializedProperty MonsterId = item.FindPropertyRelative("MonsterId");
        SerializedProperty MonsterValue = item.FindPropertyRelative("MonsterValue");
        SerializedProperty ItemId = item.FindPropertyRelative("ItemId");
        SerializedProperty ItemValue = item.FindPropertyRelative("ItemValue");
        SerializedProperty LevelId = item.FindPropertyRelative("LevelId");
        SerializedProperty TriggerId = item.FindPropertyRelative("TriggerId");
        SerializedProperty StartNpcId = item.FindPropertyRelative("StartNpcId");
        SerializedProperty EndNpcId = item.FindPropertyRelative("EndNpcId");
        SerializedProperty NextQuestId = item.FindPropertyRelative("nextQuestId");
        SerializedProperty RewardExp = item.FindPropertyRelative("rewardExp");
        SerializedProperty RewardGold = item.FindPropertyRelative("rewardGold");


        float scrollbarWidth = 16f;
        float spacing = 4f;
        float stdWidth = 154f;

        Rect onlySeenRect = new Rect(totalRect.x, totalRect.y, totalRect.width, totalRect.height);

        float totalContentHeight = 0f;

        totalContentHeight =
            10 * 18f + 17 * spacing + 136f +
            (RAW_LIST_HEIGHT + m_StartDialogs.elementHeight) +
            Mathf.Clamp(m_StartDialogs.count - 1, 0, Mathf.Infinity) * m_StartDialogs.elementHeight +
             (RAW_LIST_HEIGHT + m_IngDialogs.elementHeight) +
            Mathf.Clamp(m_IngDialogs.count - 1, 0, Mathf.Infinity) * m_IngDialogs.elementHeight +
             (RAW_LIST_HEIGHT + m_EndDialogs.elementHeight) +
            Mathf.Clamp(m_EndDialogs.count - 1, 0, Mathf.Infinity) * m_EndDialogs.elementHeight +
              (RAW_LIST_HEIGHT + m_TriggerDialogs.elementHeight) +
            Mathf.Clamp(m_TriggerDialogs.count - 1, 0, Mathf.Infinity) * m_TriggerDialogs.elementHeight +
             (RAW_LIST_HEIGHT + m_RewardItemIds.elementHeight) +
            Mathf.Clamp(m_RewardItemIds.count - 1, 0, Mathf.Infinity) * m_RewardItemIds.elementHeight;

        //有内容的部分
        Rect allContentRect = new Rect(totalRect.x, totalRect.y, totalRect.width - scrollbarWidth, totalContentHeight);
        //// SCROLL VIEW BEGIN.
        m_DialogsScrollPos = GUI.BeginScrollView(totalRect, m_DialogsScrollPos, allContentRect, false, true);
        if (totalContentHeight > 0f)
            GUI.Box(allContentRect, "");

        Rect rect = new Rect(allContentRect.x + spacing, allContentRect.y + spacing, stdWidth, 16f);

        GUIStyle richTextStyle = new GUIStyle() { richText = true, wordWrap = true };

        //标题
        rect.width = 48f;
        GUI.Label(rect, "<b>标题:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        EditorGUI.PropertyField(rect, title, GUIContent.none);

        //ID
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>ID:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        EditorGUI.PropertyField(rect, id, GUIContent.none);

        //描述
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>描述:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth * 2f + 48f;
        rect.height = 60f;
        description.stringValue = EditorGUI.TextArea(rect,description.stringValue);

        //任务状态
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>状态:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        rect.height = 16f;
        EditorGUI.PropertyField(rect, state, GUIContent.none);

        //任务种类
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>类型:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        EditorGUI.PropertyField(rect, type, GUIContent.none);
        if (type.enumValueIndex==(int)QuestType.对话)
        {
            //显示npcId
            rect.x = allContentRect.x + spacing+48f;
            rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>NpcID:</b> ", richTextStyle);

            rect.x = rect.xMax;
            rect.width = stdWidth-48f;
            EditorGUI.PropertyField(rect, NpcId, GUIContent.none);
        }else if (type.enumValueIndex == (int)QuestType.打怪)
        {
            //显示怪物ID
            rect.x = allContentRect.x + spacing + 48f;
            rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>怪物ID:</b> ", richTextStyle);

            rect.x = rect.xMax;
            rect.width = stdWidth - 48f;
            EditorGUI.PropertyField(rect, MonsterId, GUIContent.none);

            //显示怪物数量
            rect.x = allContentRect.x + spacing + 210f;
            //rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>怪物数量:</b> ", richTextStyle);

            rect.x = rect.xMax+6f;
            rect.width = stdWidth - 48f;
            EditorGUI.PropertyField(rect, MonsterValue, GUIContent.none);
        }else if (type.enumValueIndex == (int)QuestType.收集物品)
        {
            //显示物品ID
            rect.x = allContentRect.x + spacing + 48f;
            rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>物品ID:</b> ", richTextStyle);

            rect.x = rect.xMax;
            rect.width = stdWidth - 48f;
            EditorGUI.PropertyField(rect, ItemId, GUIContent.none);

            //显示物品数量
            rect.x = allContentRect.x + spacing + 210f;
            //rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>物品数量:</b> ", richTextStyle);

            rect.x = rect.xMax + 6f;
            rect.width = stdWidth - 48f;
            EditorGUI.PropertyField(rect, ItemValue, GUIContent.none);
        }else if (type.enumValueIndex == (int)QuestType.通关)
        {
            //显示关卡Id
            rect.x = allContentRect.x + spacing + 48f;
            rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>关卡ID:</b> ", richTextStyle);

            rect.x = rect.xMax;
            rect.width = stdWidth - 48f;
            EditorGUI.PropertyField(rect, LevelId, GUIContent.none);
        }else if (type.enumValueIndex == (int)QuestType.到达目的地)
        {
            //显示区域Id
            rect.x = allContentRect.x + spacing + 48f;
            rect.y = rect.yMax + spacing;
            rect.width = 48f;
            GUI.Label(rect, "<b>区域ID:</b> ", richTextStyle);

            rect.x = rect.xMax;
            rect.width = stdWidth - 48f;
            EditorGUI.PropertyField(rect, TriggerId, GUIContent.none);
        }
        //任务领取NPC和任务提交NPC标签
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>接受任务NPC:</b> ", richTextStyle);

        rect.x = allContentRect.x + spacing + 210f;
        rect.width = 48f;
        GUI.Label(rect, "<b>提交任务NPC:</b> ", richTextStyle);

        //任务领取NPC和任务提交NPC属性
        rect.x = allContentRect.x + spacing+48f;
        rect.y = rect.yMax + spacing;
        rect.width = (stdWidth - 48f)/2;
        EditorGUI.PropertyField(rect, StartNpcId, GUIContent.none);

        rect.x = allContentRect.x + spacing + 258f;
        //rect.width = stdWidth - 48f;
        EditorGUI.PropertyField(rect, EndNpcId, GUIContent.none);

        //显示任务接取对话
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>任务接取对话:</b> ", richTextStyle);

        rect.x = allContentRect.x + spacing+48f;
        rect.y = rect.yMax + spacing;
        rect.width = stdWidth * 2f + 48f;
        m_StartDialogs.DoList(rect);

        //显示任务进行对话
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + 26f + Mathf.Max(m_StartDialogs.count, 1) * DESCRIPTION_HEIGHT;
        rect.width = 48f;
        GUI.Label(rect, "<b>任务进行对话:</b> ", richTextStyle);

        rect.x = allContentRect.x + spacing + 48f;
        rect.y = rect.yMax + spacing;
        rect.width = stdWidth * 2f + 48f;
        m_IngDialogs.DoList(rect);

        //显示任务结束对话
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + 26f +  Mathf.Max(m_IngDialogs.count, 1) * DESCRIPTION_HEIGHT;
        rect.width = 48f;
        GUI.Label(rect, "<b>任务结束对话:</b> ", richTextStyle);

        rect.x = allContentRect.x + spacing + 48f;
        rect.y = rect.yMax + spacing;
        rect.width = stdWidth * 2f + 48f;
        m_EndDialogs.DoList(rect);

        //显示区域碰撞对话
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + 26f + Mathf.Max(m_EndDialogs.count, 1) * DESCRIPTION_HEIGHT;
        rect.width = 48f;
        GUI.Label(rect, "<b>区域碰撞对话:</b> ", richTextStyle);

        rect.x = allContentRect.x + spacing + 48f;
        rect.y = rect.yMax + spacing;
        rect.width = stdWidth * 2f + 48f;
        m_TriggerDialogs.DoList(rect);

        //显示奖励物品ID列表
        //Rect tempRect = new Rect(allContentRect.x + spacing, rect.yMax + 26f + Mathf.Max(m_TriggerDialogs.count, 1) * DESCRIPTION_HEIGHT, 48, 16);
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + 26f + Mathf.Max(m_TriggerDialogs.count, 1) * DESCRIPTION_HEIGHT;
        rect.width = 48f;
        GUI.Label(rect, "<b>奖励物品ID列表:</b> ", richTextStyle);

        rect.x = allContentRect.x + spacing + 48f;
        rect.y = rect.yMax + spacing;
        rect.width = stdWidth * 2f + 48f;
        m_RewardItemIds.DoList(rect);

        //奖励经验
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + 26f + Mathf.Max(m_RewardItemIds.count, 1) * DESCRIPTION_HEIGHT;
        rect.width = 48f;
        GUI.Label(rect, "<b>经验奖励:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        EditorGUI.PropertyField(rect, RewardExp, GUIContent.none);

        //奖励金钱
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>金钱奖励:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        EditorGUI.PropertyField(rect, RewardGold, GUIContent.none);

        //下一个关卡
        rect.x = allContentRect.x + spacing;
        rect.y = rect.yMax + spacing;
        rect.width = 48f;
        GUI.Label(rect, "<b>下个任务:</b> ", richTextStyle);

        rect.x = rect.xMax;
        rect.width = stdWidth;
        EditorGUI.PropertyField(rect, NextQuestId, GUIContent.none);

        GUI.EndScrollView();
    }
}
