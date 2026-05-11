using System;
using System.Collections.Generic;

namespace VTACPluginBase.Classes.DI
{
    /// <summary>
    /// Service Locator interface for Dependency Injection abstraction
    /// Provides a unified interface for different DI containers
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Get service of specified type
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Service instance</returns>
        T GetService<T>();

        /// <summary>
        /// Get service of specified type
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>Service instance</returns>
        object GetService(Type serviceType);

        /// <summary>
        /// Get all services of specified type
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Collection of service instances</returns>
        IEnumerable<T> GetServices<T>();

        /// <summary>
        /// Get all services of specified type
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>Collection of service instances</returns>
        IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        /// Check if service is registered
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>True if registered</returns>
        bool IsRegistered<T>();

        /// <summary>
        /// Check if service is registered
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>True if registered</returns>
        bool IsRegistered(Type serviceType);
    }

    /// <summary>
    /// Service Locator Provider - Singleton pattern for global access
    /// </summary>
    public static class ServiceLocatorProvider
    {
        private static IServiceLocator _current;
        private static readonly object _lock = new object();

        /// <summary>
        /// Current Service Locator instance
        /// </summary>
        public static IServiceLocator Current
        {
            get
            {
                if (_current == null)
                {
                    lock (_lock)
                    {
                        if (_current == null)
                        {
                            _current = new DefaultServiceLocator();
                        }
                    }
                }
                return _current;
            }
        }

        /// <summary>
        /// Set the current Service Locator
        /// </summary>
        /// <param name="serviceLocator">Service Locator instance</param>
        public static void SetCurrent(IServiceLocator serviceLocator)
        {
            lock (_lock)
            {
                _current = serviceLocator;
            }
        }

        /// <summary>
        /// Reset to default Service Locator
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _current = new DefaultServiceLocator();
            }
        }
    }

    /// <summary>
    /// Default Service Locator implementation with basic service registration
    /// </summary>
    internal class DefaultServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();
        private readonly object _lock = new object();

        public DefaultServiceLocator()
        {
            // Register default services
            RegisterDefaults();
        }

        private void RegisterDefaults()
        {
            // Register basic services that are always available
            // Additional services can be registered by calling Register methods
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            lock (_lock)
            {
                // Check for singleton instance
                if (_services.TryGetValue(serviceType, out object instance))
                {
                    return instance;
                }

                // Check for factory
                if (_factories.TryGetValue(serviceType, out Func<object> factory))
                {
                    return factory();
                }

                // Try to create instance if it has parameterless constructor
                if (serviceType.IsClass && !serviceType.IsAbstract)
                {
                    var constructor = serviceType.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        return Activator.CreateInstance(serviceType);
                    }
                }

                return null;
            }
        }

        public IEnumerable<T> GetServices<T>()
        {
            return (IEnumerable<T>)GetServices(typeof(T));
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var service = GetService(serviceType);
            if (service != null)
            {
                yield return service;
            }
        }

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type serviceType)
        {
            lock (_lock)
            {
                return _services.ContainsKey(serviceType) || _factories.ContainsKey(serviceType);
            }
        }

        /// <summary>
        /// Register a singleton service instance
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <param name="instance">Service instance</param>
        public void RegisterSingleton<T>(T instance)
        {
            lock (_lock)
            {
                _services[typeof(T)] = instance;
            }
        }

        /// <summary>
        /// Register a factory for creating service instances
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <param name="factory">Factory function</param>
        public void RegisterFactory<T>(Func<T> factory)
        {
            lock (_lock)
            {
                _factories[typeof(T)] = () => factory();
            }
        }

        /// <summary>
        /// Register a factory for creating service instances
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <param name="factory">Factory function</param>
        public void RegisterFactory(Type serviceType, Func<object> factory)
        {
            lock (_lock)
            {
                _factories[serviceType] = factory;
            }
        }
    }
}
