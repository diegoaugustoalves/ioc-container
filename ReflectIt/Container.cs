﻿using System;
 using System.Collections.Generic;
 using System.Linq;

 namespace ReflectIt
{
    public class Container
    {
        private Dictionary<Type, Type> _map = new Dictionary<Type, Type>();
        public ContainerBuilder For<TSource>()
        {
            return For(typeof(TSource));
        }

        public ContainerBuilder For(Type sourceType)
        {
            return new ContainerBuilder(this, sourceType);
        }
        
        public TSource Resolve<TSource>()
        {
            return (TSource) Resolve(typeof(TSource));
        }
        
        public object Resolve(Type sourceType)
        {
            if (_map.ContainsKey(sourceType))
            {
                var targetType = _map[sourceType];
                return CreateInstance(targetType);
            }

            if (sourceType.IsGenericType && _map.ContainsKey(sourceType.GetGenericTypeDefinition())) 
            {
                var target = _map[sourceType.GetGenericTypeDefinition()]; // Definition of IRepository<Customer> == IRepository<>
                var closedTarget = target.MakeGenericType(sourceType.GenericTypeArguments); 

                return CreateInstance(closedTarget);
            }

            if (!sourceType.IsAbstract)
            {
                return CreateInstance(sourceType);
            }
            
            throw new InvalidOperationException($"Could not resolve the {sourceType} type");
        }

        private object CreateInstance(Type targetType)
        {
            var parameters = targetType.GetConstructors()
                .OrderByDescending(p => p.GetParameters().Count())
                .First()
                .GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();
                    
            return Activator.CreateInstance(targetType, parameters);
        }

        public class ContainerBuilder
        {
            private readonly Container _container;
            private readonly Type _sourceType;
            
            public ContainerBuilder(Container container, Type sourceType)
            {
                _container = container;
                _sourceType = sourceType;
            }
            
            public ContainerBuilder Use<TTarget>()
            {
                return Use(typeof(TTarget));
            }
            
            public ContainerBuilder Use(Type targetType)
            {
                _container._map.Add(_sourceType, targetType);
                return this;
            }
        }
    }
}