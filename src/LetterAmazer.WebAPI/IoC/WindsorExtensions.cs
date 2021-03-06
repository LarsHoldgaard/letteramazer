﻿using Castle.Core;
using Castle.MicroKernel.ComponentActivator;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LetterAmazer.WebAPI.IoC
{
    public static class WindsorExtensions
    {
        public static void InjectProperties(this IWindsorContainer container, object target, bool overwrite = false)
        {
            var kernel = container.Kernel;
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && (overwrite || property.GetValue(target, null) == null) && kernel.HasComponent(property.PropertyType))
                {
                    var value = kernel.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                        throw new ComponentActivatorException(message, ex, (ComponentModel)target);
                    }
                }
            }
        }
    }
}