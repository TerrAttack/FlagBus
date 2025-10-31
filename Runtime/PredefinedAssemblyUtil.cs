using System;
using System.Collections.Generic;

namespace Utils.Flags
{
    public class PredefinedAssemblyUtil
    {
        private static AssemblyType? getAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null
            };
        }

        public static List<Type> GetTypes(Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            var types = new List<Type>();
            for (var i = 0; i < assemblies.Length; i++)
            {
                var assemblyType = getAssemblyType(assemblies[i].GetName().Name);
                if (assemblyType != null) assemblyTypes.Add(assemblyType.Value, assemblies[i].GetTypes());
            }

            assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var assemblyCSharpTypes);
            AddTypesFromAssembly(assemblyCSharpTypes, types, interfaceType);

            assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharpFirstPass, out var assemblyCSharpFirstPassTypes);
            AddTypesFromAssembly(assemblyCSharpFirstPassTypes, types, interfaceType);

            return types;
        }

        private static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
        {
            if (assembly == null) return;
            for (var i = 0; i < assembly.Length; i++)
            {
                var type = assembly[i];
                if (type != interfaceType && interfaceType.IsAssignableFrom(type)) types.Add(type);
            }
        }

        private enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass
        }
    }
}