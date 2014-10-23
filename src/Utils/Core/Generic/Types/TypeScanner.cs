using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using int32.Utils.Core.Generic.Singleton;

namespace int32.Utils.Core.Generic.Types
{
    public class TypeScanner : Singleton<TypeScanner>
    {
        private List<Assembly> _assemblies;

        public TypeScanner()
        {
            _assemblies = new List<Assembly>();
            UpdateAssemblies();
        }

        public IEnumerable<Type> Scan<T>()
        {
            _assemblies.AddRange(GetReferencedAssemblies());

            return (from asm in _assemblies
                    from t in asm.GetTypes()
                    where t.BaseType == typeof(T)
                    select t
                ).ToList();
        }

        private IEnumerable<Assembly> GetReferencedAssemblies()
        {
            var existingAssemblyPaths = _assemblies.Select(a => a.Location).ToArray();
            var assemblies = new List<Assembly>();

            foreach (var directory in GetAssemblyDirectories())
            {
                var unloadedAssemblies = Directory.GetFiles(directory, "*.dll")
                    .Where(f => !existingAssemblyPaths.Contains(f, StringComparer.InvariantCultureIgnoreCase)).ToArray();

                foreach (var unloadedAssembly in unloadedAssemblies)
                {
                    Assembly inspectedAssembly = null;
                    try
                    {
                        inspectedAssembly = Assembly.ReflectionOnlyLoadFrom(unloadedAssembly);
                    }
                    catch (BadImageFormatException biEx) { }

                    if (inspectedAssembly == null) continue;

                    try
                    {
                        var asm = Assembly.Load(inspectedAssembly.GetName());
                        assemblies.Add(asm);
                    }
                    catch { }
                }
            }

            return assemblies;
        }


        private void UpdateAssemblies()
        {
            _assemblies = (AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.ReflectionOnly)).ToList();
        }

        private IEnumerable<string> GetAssemblyDirectories()
        {
            var privateBinPathDirectories = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath == null
                                                ? new string[] { }
                                                : AppDomain.CurrentDomain.SetupInformation.PrivateBinPath.Split(';');

            foreach (var privateBinPathDirectory in privateBinPathDirectories)
            {
                if (!string.IsNullOrEmpty(privateBinPathDirectory))
                {
                    yield return privateBinPathDirectory;
                }
            }

            if (AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe == null)
            {
                yield return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }

    }
}
