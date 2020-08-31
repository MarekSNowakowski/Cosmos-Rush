// <copyright file="CloudIDs.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace CloudOnce
{
#if (UNITY_ANDROID || UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
    using Internal;
    using UnityEngine;
#endif
    /// <summary>
    /// Provides access to achievement- and leaderboard IDs registered via the CloudOnce Editor.
    /// This file was automatically generated by CloudOnce. Do not edit.
    /// </summary>
    public static class CloudIDs
    {
        /// <summary>
        /// Contains properties that retrieves achievement IDs for the current platform.
        /// </summary>
        public static class AchievementIDs
        {
            public static string scoreMaster
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQBw";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "scoreMaster";
#else
                    return string.Empty;
#endif
                }
            }

            public static string speedster
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQCA";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "speedster";
#else
                    return string.Empty;
#endif
                }
            }

            public static string comboGod
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQCQ";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "comboGod";
#else
                    return string.Empty;
#endif
                }
            }

            public static string mountEverest
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQCg";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "mountEverest";
#else
                    return string.Empty;
#endif
                }
            }

            public static string millionaire
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQCw";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "millionaire";
#else
                    return string.Empty;
#endif
                }
            }

            public static string destroyer
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQDA";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "destroyer";
#else
                    return string.Empty;
#endif
                }
            }

            public static string andromeda
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQDQ";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "andromeda";
#else
                    return string.Empty;
#endif
                }
            }

            public static string triangulum
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQDg";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "triangulum";
#else
                    return string.Empty;
#endif
                }
            }

            public static string prestidge
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQDw";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "prestidge";
#else
                    return string.Empty;
#endif
                }
            }
        }

        /// <summary>
        /// Contains properties that retrieves leaderboard IDs for the current platform.
        /// </summary>
        public static class LeaderboardIDs
        {
            public static string highScore
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQAA";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "highScore";
#else
                    return string.Empty;
#endif
                }
            }

            public static string maxCombo
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQAQ";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "maxCombo";
#else
                    return string.Empty;
#endif
                }
            }

            public static string moneyEarned
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQAg";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "moneyEarned";
#else
                    return string.Empty;
#endif
                }
            }

            public static string distance
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQAw";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "distance";
#else
                    return string.Empty;
#endif
                }
            }

            public static string ballsDistroyed
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQBA";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "ballsDistroyed";
#else
                    return string.Empty;
#endif
                }
            }

            public static string maxSpeed
            {
                get
                {
#if UNITY_ANDROID && !UNITY_EDITOR
#if CLOUDONCE_GOOGLE
                    return "CgkIuvz0_NcCEAIQBQ";
#else
                    return string.Empty;
#endif
#elif (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
                    return "";
#elif UNITY_EDITOR
                    return "maxSpeed";
#else
                    return string.Empty;
#endif
                }
            }
        }
    }
}
