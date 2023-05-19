using System;
using System.Collections.Generic;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Core.Logic.Services
{
    internal class CrossDomainService : ICrossDomainServiceRegistrar, ICrossDomainServiceResolver
    {
        private readonly Dictionary<Type, LifetimeScope> _serviceMap = new();

        public void RegisterService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope) where TType : TInterface
        {
            var type = typeof(TInterface);
            ValidateRegisteringKey(type);
            
            _serviceMap.Add(type, scope);
            builder.Register<TInterface, TType>(Lifetime.Singleton);
        }

        public void RegisterComponentService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope,
            TType monoBehaviour) where TType : MonoBehaviour, TInterface
        {
            var type = typeof(TInterface);
            ValidateRegisteringKey(type);
            
            _serviceMap.Add(type, scope);
            builder.RegisterComponent<TInterface>(monoBehaviour);
        }

        public void UnRegisterService<TInterface>()
        {
            var type = typeof(TInterface);
            _serviceMap.Remove(type);
        }

        public TInterface ResolveService<TInterface>()
        {
            var type = typeof(TInterface);
            var hasResolver = _serviceMap.TryGetValue(type, out var scope);

            if (!hasResolver)
            {
                throw new Exception($"Service {type} not found");
            }

            if (!scope)
            {
                _serviceMap.Remove(type);
                throw new Exception($"Can't resolve {type}. It's container doesn't exist or already destroyed");
            }

            return scope.Container.Resolve<TInterface>();
        }

        private void ValidateRegisteringKey(Type type)
        {
            if (_serviceMap.ContainsKey(type))
            {
                throw new InvalidKeyException($"Service with '{type}' type already registered");
            }
        }
    }
}