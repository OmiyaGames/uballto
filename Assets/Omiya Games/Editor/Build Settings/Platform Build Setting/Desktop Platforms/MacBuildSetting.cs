﻿using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Builds
{
    ///-----------------------------------------------------------------------
    /// <copyright file="MacBuildSetting.cs" company="Omiya Games">
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
    /// <date>10/31/2018</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Build settings for Mac platform.
    /// </summary>
    public class MacBuildSetting : IStandaloneBuildSetting
    {
        private static readonly Architecture[] supportedArchitectures = new Architecture[]
        {
            Architecture.BuildUniversal
        };
        private static readonly ScriptingImplementation[] supportedScriptingBackends = new ScriptingImplementation[]
        {
            ScriptingImplementation.Mono2x,
            ScriptingImplementation.IL2CPP
        };

        public override Architecture[] SupportedArchitectures
        {
            get
            {
                return supportedArchitectures;
            }
        }

        public override ScriptingImplementation[] SupportedScriptingBackends
        {
            get
            {
                return supportedScriptingBackends;
            }
        }

        public override ScriptingImplementation ScriptingBackend
        {
            get
            {
                switch (base.ScriptingBackend)
                {
                    // TODO: Figure out if there's an actual way to check if the editor does support IL2CPP
#if UNITY_EDITOR_OSX
                    case ScriptingImplementation.IL2CPP:
                        return base.ScriptingBackend;
#endif
                    default:
                        return DefaultScriptingBackend;
                }
            }
        }

        protected override BuildTarget Target
        {
            get
            {
                return BuildTarget.StandaloneOSX;
            }
        }
    }
}
