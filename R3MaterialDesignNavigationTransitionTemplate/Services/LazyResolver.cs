﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MaterialDesignNavigationTransitionTemplate.Services
{
    public class LazyResolver<T> : Lazy<T>
        where T : class
    {
        public LazyResolver(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        { }
    }
}
