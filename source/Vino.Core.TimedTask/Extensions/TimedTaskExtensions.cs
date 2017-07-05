﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vino.Core.TimedTask;
using Vino.Core.TimedTask.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TimedTaskExtensions
    {
        public static IServiceCollection AddTimedTask(this IServiceCollection self)
        {
            self.TryAddSingleton<IAssemblyLocator, VinoAssemblyLocator>();
            return self.AddSingleton<TimedTaskService>();
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    public static class TimedTaskExtensions
    {
        public static IApplicationBuilder UseTimedTask(this IApplicationBuilder self)
        {
            self.ApplicationServices.GetRequiredService<TimedTaskService>();
            return self;
        }

        public static IServiceProvider UseTimedTask(this IServiceProvider self)
        {
            self.GetRequiredService<TimedTaskService>();
            return self;
        }
    }
}
