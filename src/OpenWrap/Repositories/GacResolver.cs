﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenWrap.Collections;
using OpenWrap.PackageManagement.Exporters;
using OpenWrap.PackageModel;
using OpenWrap.Runtime;

namespace OpenWrap.Repositories
{
    public static class GacResolver
    {
        public static ILookup<IPackageInfo, AssemblyName> InGac(IEnumerable<IPackageInfo> packages, ExecutionEnvironment environment = null)
        {
            var domain = TempDomain();
            try
            {
                var loader = ((Loader)domain.CreateInstanceFromAndUnwrap(
                        typeof(Loader).Assembly.Location,
                        typeof(Loader).FullName));
                return (from package in packages.NotNull().Select(x => x.Load()).NotNull()
                        let export = package.GetExport("bin", environment)
                        where export != null
                        from assembly in export.Items.OfType<IAssemblyReferenceExportItem>()
                        let inGac = loader.InGAC(assembly.AssemblyName)
                        where inGac
                        select new { package, assembly.AssemblyName })
                        .ToLookup(x => (IPackageInfo)x.package, x => x.AssemblyName);
            }
            catch
            {
                return (new AssemblyName[0]).ToLookup(x => (IPackageInfo)null);
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        static AppDomain TempDomain()
        {
            return AppDomain.CreateDomain("GAC Resolve");
        }

        public class Loader : MarshalByRefObject
        {
            readonly IEnumerable<AssemblyName> _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName()).ToList();

            public bool InGAC(AssemblyName assemblyName)
            {
                if (!assemblyName.IsStronglyNamed() || _loadedAssemblies.Contains(assemblyName))
                    return false;
                try
                {
                    return Assembly.ReflectionOnlyLoad(assemblyName.FullName) != null;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}