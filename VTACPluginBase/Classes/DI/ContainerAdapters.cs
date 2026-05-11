using System;
using System.Collections.Generic;
using System.Linq;

namespace VTACPluginBase.Classes.DI.Adapters
{
    /// <summary>
    /// Abstract base class for DI Container adapters
    /// </summary>
    public abstract class ContainerAdapterBase : IServiceLocator, IDisposable
    {
        protected bool _disposed = false;

        public abstract T GetService<T>();
        public abstract object GetService(Type serviceType);
        public abstract IEnumerable<T> GetServices<T>();
        public abstract IEnumerable<object> GetServices(Type serviceType);
        public abstract bool IsRegistered<T>();
        public abstract bool IsRegistered(Type serviceType);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Simple built-in container adapter - no external dependencies
    /// </summary>
    public class SimpleContainerAdapter : ContainerAdapterBase
    {
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Func<object>> _transients = new Dictionary<Type, Func<object>>();
        private readonly object _lock = new object();

        public override T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public override object GetService(Type serviceType)
        {
            lock (_lock)
            {
                // Check singletons first
                if (_singletons.TryGetValue(serviceType, out object singleton))
                {
                    return singleton;
                }

                // Check transients
                if (_transients.TryGetValue(serviceType, out Func<object> factory))
                {
                    return factory();
                }

                // Try to create instance
                return TryCreateInstance(serviceType);
            }
        }

        public override IEnumerable<T> GetServices<T>()
        {
            var service = GetService<T>();
            if (service != null)
            {
                yield return service;
            }
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var service = GetService(serviceType);
            if (service != null)
            {
                yield return service;
            }
        }

        public override bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        public override bool IsRegistered(Type serviceType)
        {
            lock (_lock)
            {
                return _singletons.ContainsKey(serviceType) || _transients.ContainsKey(serviceType);
            }
        }

        /// <summary>
        /// Register a singleton service
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            RegisterSingleton<TInterface>(new TImplementation());
        }

        /// <summary>
        /// Register a singleton service instance
        /// </summary>
        public void RegisterSingleton<T>(T instance)
        {
            lock (_lock)
            {
                _singletons[typeof(T)] = instance;
            }
        }

        /// <summary>
        /// Register a transient service
        /// </summary>
        public void RegisterTransient<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            RegisterTransient<TInterface>(() => new TImplementation());
        }

        /// <summary>
        /// Register a transient service with factory
        /// </summary>
        public void RegisterTransient<T>(Func<T> factory)
        {
            lock (_lock)
            {
                _transients[typeof(T)] = () => factory();
            }
        }

        private object TryCreateInstance(Type serviceType)
        {
            try
            {
                if (serviceType.IsClass && !serviceType.IsAbstract)
                {
                    var constructor = serviceType.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        return Activator.CreateInstance(serviceType);
                    }
                }
            }
            catch
            {
                // Ignore creation errors
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                lock (_lock)
                {
                    // Dispose singletons that implement IDisposable
                    foreach (var singleton in _singletons.Values.OfType<IDisposable>())
                    {
                        try
                        {
                            singleton.Dispose();
                        }
                        catch
                        {
                            // Ignore disposal errors
                        }
                    }
                    _singletons.Clear();
                    _transients.Clear();
                }
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Unity Container Adapter - requires Unity package
    /// Uncomment and install Unity NuGet package to use
    /// </summary>
    /*
    public class UnityContainerAdapter : ContainerAdapterBase
    {
        private readonly IUnityContainer _container;

        public UnityContainerAdapter(IUnityContainer container = null)
        {
            _container = container ?? new UnityContainer();
        }

        public override T GetService<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch
            {
                return default(T);
            }
        }

        public override object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<T> GetServices<T>()
        {
            try
            {
                return _container.ResolveAll<T>();
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch
            {
                return Enumerable.Empty<object>();
            }
        }

        public override bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
        }

        public override bool IsRegistered(Type serviceType)
        {
            return _container.IsRegistered(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _container?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    */

    /// <summary>
    /// Autofac Container Adapter - requires Autofac package
    /// Uncomment and install Autofac NuGet package to use
    /// </summary>
    /*
    public class AutofacContainerAdapter : ContainerAdapterBase
    {
        private readonly IContainer _container;

        public AutofacContainerAdapter(IContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public override T GetService<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch
            {
                return default(T);
            }
        }

        public override object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<T> GetServices<T>()
        {
            try
            {
                return _container.Resolve<IEnumerable<T>>();
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                return (IEnumerable<object>)_container.Resolve(enumerableType);
            }
            catch
            {
                return Enumerable.Empty<object>();
            }
        }

        public override bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
        }

        public override bool IsRegistered(Type serviceType)
        {
            return _container.IsRegistered(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _container?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    */
}
