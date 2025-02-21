﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using OmiyaGames.Scenes;

namespace OmiyaGames.UI.Scenes
{
    ///-----------------------------------------------------------------------
    /// <copyright file="SceneTransitionManagerEditor.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2016 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <author>Taro Omiya</author>
    /// <date>4/15/2016</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Editor script for <code>SceneInfo</code>
    /// </summary>
    /// <seealso cref="SceneInfo"/>
    [CustomPropertyDrawer(typeof(SceneInfo))]
    public class SceneInfoDrawer : PropertyDrawer
    {
        const float RevertTimeWidth = 140;

        static GUIContent revertTimeScaleContent = null;
        public GUIContent RevertTimeScaleContent
        {
            get
            {
                if (revertTimeScaleContent == null)
                {
                    revertTimeScaleContent = new GUIContent("Reset TimeScale?", "See TimeManager script to set the scene's timescale.");
                }
                return revertTimeScaleContent;
            }
        }

        static GUIStyle rightAlignStyleCache = null;
        public static GUIStyle RightAlignStyle
        {
            get
            {
                if (rightAlignStyleCache == null)
                {
                    rightAlignStyleCache = new GUIStyle();
                    rightAlignStyleCache.alignment = TextAnchor.MiddleRight;
                }
                return rightAlignStyleCache;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetHeight(property.FindPropertyRelative("displayName"), label);
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Draw label
            Rect labelRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (string.IsNullOrEmpty(label.text) == false)
            {
                position.y += (EditorGUIUtility.singleLineHeight + EditorUiUtility.VerticalMargin);
                EditorGUI.indentLevel = 1;
            }

            // Draw the File Name label
            position.height = base.GetPropertyHeight(property, label);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("scenePath"));

            // Dock the rest of the fields down a bit
            position.y += (position.height + EditorUiUtility.VerticalMargin);
            Rect fieldRect = position;
            fieldRect.width -= RevertTimeWidth - EditorUiUtility.VerticalSpace;

            // Draw Cursor label
            EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("cursorMode"));

            fieldRect.width = RevertTimeWidth;
            fieldRect.x = position.xMax - fieldRect.width;

            // Draw Revert Time field
            SerializedProperty childProperty = property.FindPropertyRelative("revertTimeScale");
            childProperty.boolValue = EditorGUI.ToggleLeft(fieldRect, "Reset Time Scale", childProperty.boolValue);

            // Dock the rest of the fields down a bit
            position.y += (position.height + EditorUiUtility.VerticalMargin);

            // Draw Display Name label
            EditorGUI.PropertyField(position, property.FindPropertyRelative("displayName"));

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        internal static float GetHeight(SerializedProperty translatedDisplayNameProperty, GUIContent label = null)
        {
            float returnHeight = EditorUiUtility.GetHeight(label, 2, EditorUiUtility.VerticalMargin);
            if (translatedDisplayNameProperty != null)
            {
                returnHeight += EditorUiUtility.VerticalMargin;
                returnHeight += EditorGUI.GetPropertyHeight(translatedDisplayNameProperty);
            }
            return returnHeight;
        }
    }

    ///-----------------------------------------------------------------------
    /// <copyright file="SceneTransitionManagerEditor.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2016 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <author>Taro Omiya</author>
    /// <date>4/15/2016</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Editor script for <code>SceneTransitionManager</code>
    /// </summary>
    /// <seealso cref="SceneTransitionManager"/>
    [CustomEditor(typeof(SceneTransitionManager))]
    public class SceneTransitionManagerEditor : Editor
    {
        const float VerticalMargin = 2;

        SerializedProperty debugLockMode;
        SerializedProperty soundEffect;
        SerializedProperty mainMenu;
        SerializedProperty credits;
        SerializedProperty loading;
        SerializedProperty levels;
        ReorderableList levelList;

        static bool displayDefaults = false;
        static int defaultFillIn = 0;
        static string defaultDisplayName = "Level {0}";
        static bool defaultRevertsTimeScale = true;
        static CursorLockMode defaultLockMode = CursorLockMode.None;
        static readonly string[] DefaultFillInOptions = new string[]
        {
            "Ordinal",
            "Scene Name"
        };
        static readonly GUIContent DefaultDisplayNameLabel = new GUIContent("New Display Name");
        static readonly GUIContent DefaultFillInLabel = new GUIContent("Fill-in {0} with...");
        static readonly GUIContent DefaultRevertsTimeScaleLabel = new GUIContent("New Revert TimeScale");
        static readonly GUIContent DefaultCursorLockModeLabel = new GUIContent("New Cursor Lock Mode");

        public void OnEnable()
        {
            // Grab all serialized properties
            debugLockMode = serializedObject.FindProperty("debugLockMode");
            soundEffect = serializedObject.FindProperty("soundEffect");
            mainMenu = serializedObject.FindProperty("mainMenu");
            credits = serializedObject.FindProperty("credits");
            loading = serializedObject.FindProperty("loading");
            levels = serializedObject.FindProperty("levels");

            // Setup level list
            levelList = new ReorderableList(serializedObject, levels, true, true, true, true);
            levelList.drawHeaderCallback = DrawLevelListHeader;
            levelList.drawElementCallback = DrawLevelListElement;
            levelList.elementHeightCallback = GetLeverListElementHeight;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(debugLockMode, true);
            EditorGUILayout.PropertyField(soundEffect, true);
            EditorGUILayout.PropertyField(mainMenu, true);
            EditorGUILayout.PropertyField(credits, true);
            EditorGUILayout.PropertyField(loading, true);
            levelList.DoLayoutList();

            // Display the scene appending stuff
            EditorGUILayout.Separator();
            displayDefaults = EditorGUILayout.Foldout(displayDefaults, "Populate All Levels with scenes in Build Settings");
            if (displayDefaults == true)
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 1;
                DrawDefaultLevelFields();
                DrawLevelListButtons();
                EditorGUI.indentLevel = indent;
            }
            serializedObject.ApplyModifiedProperties();
        }

        void DrawLevelListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "All Levels");
        }

        void DrawLevelListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = levels.GetArrayElementAtIndex(index);
            rect.y += VerticalMargin;
            rect.height = SceneInfoDrawer.GetHeight(element.FindPropertyRelative("displayName"));
            EditorGUI.PropertyField(rect, element, GUIContent.none);
        }

        void DrawDefaultLevelFields()
        {
            // Default Display Name
            Rect controlRect = EditorGUILayout.GetControlRect();
            controlRect = EditorGUI.PrefixLabel(controlRect, GUIUtility.GetControlID(FocusType.Passive), DefaultDisplayNameLabel);
            defaultDisplayName = EditorGUI.TextField(controlRect, defaultDisplayName);


            // Default Fill-In field
            controlRect = EditorGUILayout.GetControlRect();
            controlRect = EditorGUI.PrefixLabel(controlRect, GUIUtility.GetControlID(FocusType.Passive), DefaultFillInLabel);
            defaultFillIn = EditorGUI.Popup(controlRect, defaultFillIn, DefaultFillInOptions);

            // Default Revert TimeScale field
            controlRect = EditorGUILayout.GetControlRect();
            controlRect = EditorGUI.PrefixLabel(controlRect, GUIUtility.GetControlID(FocusType.Passive), DefaultRevertsTimeScaleLabel);
            defaultRevertsTimeScale = EditorGUI.Toggle(controlRect, defaultRevertsTimeScale);


            // Default Cursor field
            controlRect = EditorGUILayout.GetControlRect();
            controlRect = EditorGUI.PrefixLabel(controlRect, GUIUtility.GetControlID(FocusType.Passive), DefaultCursorLockModeLabel);
            defaultLockMode = (CursorLockMode)EditorGUI.EnumPopup(controlRect, defaultLockMode);
        }

        void DrawLevelListButtons()
        {
            // Show Append button
            EditorGUILayout.Space();
            Rect controlRect = EditorGUILayout.GetControlRect();
            controlRect.height += (VerticalMargin * 2);
            if (GUI.Button(controlRect, "Append new scenes in Build Settings to All Levels list") == true)
            {
                // Actually append scenes to the list
                SceneTransitionManager manager = ((SceneTransitionManager)target);
                manager.SetupLevels(defaultDisplayName, (defaultFillIn != 0), defaultRevertsTimeScale, defaultLockMode, true);

                // Untoggle
                displayDefaults = false;
            }

            // Show Replace button
            EditorGUILayout.Space();
            controlRect = EditorGUILayout.GetControlRect();
            controlRect.height += (VerticalMargin * 2);
            if (GUI.Button(controlRect, "Replace All Levels list with scenes in Build Settings") == true)
            {
                // Actually append scenes to the list
                SceneTransitionManager manager = ((SceneTransitionManager)target);
                manager.SetupLevels(defaultDisplayName, (defaultFillIn != 0), defaultRevertsTimeScale, defaultLockMode, false);

                // Untoggle
                displayDefaults = false;
            }

            EditorGUILayout.Space();
        }

        float GetLeverListElementHeight(int index)
        {
            SerializedProperty element = levels.GetArrayElementAtIndex(index);
            return SceneInfoDrawer.GetHeight(element.FindPropertyRelative("displayName")) + VerticalMargin;
        }
    }
}