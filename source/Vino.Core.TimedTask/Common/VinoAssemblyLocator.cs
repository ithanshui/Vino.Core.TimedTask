﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyModel;

namespace Vino.Core.TimedTask.Common
{
    public class VinoAssemblyLocator : IAssemblyLocator
    {
        private static readonly string AssemblyRoot = typeof(TimedTask).GetTypeInfo().Assembly.GetName().Name;
        private readonly Assembly _entryAssembly;
        private readonly DependencyContext _dependencyContext;

        public VinoAssemblyLocator()
        {
            _entryAssembly = Assembly.GetEntryAssembly();
            //_entryAssembly = Assembly.Load(new AssemblyName(environment.ApplicationName));
            _dependencyContext = DependencyContext.Load(_entryAssembly);
        }

        public virtual IList<Assembly> GetAssemblies()
        {
            if (_dependencyContext == null)
            {
                // Use the entry assembly as the sole candidate.
                return new[] { _entryAssembly };
            }

            return _dependencyContext
                .RuntimeLibraries
                .Where(IsCandidateLibrary)
                .SelectMany(l => l.GetDefaultAssemblyNames(_dependencyContext))
                .Select(assembly => Assembly.Load(new AssemblyName(assembly.Name)))
                .ToArray();
        }

        private bool IsCandidateLibrary(RuntimeLibrary library)
        {
            return library.Dependencies.Any(dependency => string.Equals(AssemblyRoot, dependency.Name, StringComparison.Ordinal));
        }
    }
}
