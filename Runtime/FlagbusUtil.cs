using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils.Flags
{
    public static class FlagBusUtil
    {
        public static IReadOnlyList<Type> FlagTypes { get; private set; }
        public static IReadOnlyList<Type> FlagBusTypes { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            FlagTypes = PredefinedAssemblyUtil.GetTypes(typeof(IFlag));
            FlagBusTypes = CreateAllFlagBusTypes();
        }

        private static List<Type> CreateAllFlagBusTypes()
        {
            var flagbBsTypes = new List<Type>();
            var typeDef = typeof(FlagBus<>);

            foreach (var flagType in FlagTypes)
            {
                var busType = typeDef.MakeGenericType(flagType);
                flagbBsTypes.Add(busType);
            }

            return flagbBsTypes;
        }

        public static void ClearAllFlags()
        {
            foreach (var type in FlagBusTypes)
            {
                var clearMethod = type.GetMethod("Clear", BindingFlags.Public | BindingFlags.Static);
                clearMethod?.Invoke(null, null);
            }
        }


#if UNITY_EDITOR
        public static PlayModeStateChange PlayModeState { get; set; }

        [InitializeOnLoadMethod]
        public static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeState = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
                ClearAllFlags();
        }
#endif
    }
}
