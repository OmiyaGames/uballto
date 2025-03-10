﻿using UnityEditor;
using OmiyaGames.Builds;

namespace OmiyaGames.UI.Builds
{
    ///-----------------------------------------------------------------------
    /// <copyright file="WindowsBuildSettingEditor.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2018 Omiya Games
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
    /// <date>11/21/2015</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Editor script for <code>WindowsBuildSetting</code>
    /// </summary>
    /// <seealso cref="WindowsBuildSetting"/>
    [CustomEditor(typeof(WindowsBuildSetting))]
    public class WindowsBuildSettingEditor : IStandaloneBuildSettingEditor
    {
        private SerializedProperty includePdbFles;
        // FIXME: do more research on the Facebook builds
        //private SerializedProperty forFacebook;

        public override void OnEnable()
        {
            base.OnEnable();
            includePdbFles = serializedObject.FindProperty("includePdbFles");
            //forFacebook = serializedObject.FindProperty("forFacebook");
        }

        protected override void DrawPlatformSpecificSettings()
        {
            base.DrawPlatformSpecificSettings();
            EditorGUILayout.PropertyField(includePdbFles);
            //EditorGUILayout.PropertyField(forFacebook);
        }
    }
}
