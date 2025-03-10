﻿using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Builds
{
    ///-----------------------------------------------------------------------
    /// <copyright file="LinuxBuildSetting.cs" company="Omiya Games">
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
    /// Build settings for Linux platform.
    /// </summary>
    public class LinuxBuildSetting : IStandaloneBuildSetting
    {
        private static readonly Architecture[] supportedArchitectures = new Architecture[]
        {
            Architecture.BuildUniversal,
            Architecture.Build64Bit,
            Architecture.Build32Bit
        };
        private static readonly ScriptingImplementation[] supportedScriptingBackends = new ScriptingImplementation[]
        {
            ScriptingImplementation.Mono2x
        };

        [SerializeField]
        protected bool enableHeadlessMode = false;

        #region Overrides
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
                    // TODO: currently, Linux only supports Mono. Update this property when that's no longer true
#if false
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
                switch(ArchitectureToBuild)
                {
                    case Architecture.Build64Bit:
                        return BuildTarget.StandaloneLinux64;
                    case Architecture.Build32Bit:
                        return BuildTarget.StandaloneLinux;
                    default:
                        return BuildTarget.StandaloneLinuxUniversal;
                }
            }
        }

        protected override BuildOptions Options
        {
            get
            {
                BuildOptions options = base.Options;

                // Add Headless options
                if (enableHeadlessMode == true)
                {
                    options |= BuildOptions.EnableHeadlessMode;
                }
                return options;
            }
        }
        #endregion
    }
}
