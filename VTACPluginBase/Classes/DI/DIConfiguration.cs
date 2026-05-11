using System;

using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.DI.Adapters;
using VTACPluginBase.Classes.TextLogger;

namespace VTACPluginBase.Classes.DI
{
    /// <summary>
    /// Dependency Injection Configuration Helper
    /// Provides easy setup for different DI containers
    /// </summary>
    public static class DIConfiguration
    {
        /// <summary>
        /// Configure Simple Container (built-in, no external dependencies)
        /// </summary>
        /// <param name="configureServices">Service configuration action</param>
        /// <returns>Configured Service Locator</returns>
        public static IServiceLocator ConfigureSimpleContainer(Action<SimpleContainerAdapter> configureServices = null)
        {
            var container = new SimpleContainerAdapter();

            // Register default services
            RegisterDefaultServices(container);

            // Allow custom service registration
            configureServices?.Invoke(container);

            // Set as current service locator
            ServiceLocatorProvider.SetCurrent(container);

            return container;
        }

        /// <summary>
        /// Configure Unity Container (requires Unity NuGet package)
        /// Uncomment when Unity is available
        /// </summary>
        /*
        public static IServiceLocator ConfigureUnityContainer(Action<IUnityContainer> configureServices = null)
        {
            var unityContainer = new UnityContainer();
            
            // Register default services
            RegisterDefaultServices(unityContainer);
            
            // Allow custom service registration
            configureServices?.Invoke(unityContainer);
            
            var adapter = new UnityContainerAdapter(unityContainer);
            ServiceLocatorProvider.SetCurrent(adapter);
            
            return adapter;
        }
        */

        /// <summary>
        /// Configure Autofac Container (requires Autofac NuGet package)
        /// Uncomment when Autofac is available
        /// </summary>
        /*
        public static IServiceLocator ConfigureAutofacContainer(Action<ContainerBuilder> configureServices = null)
        {
            var builder = new ContainerBuilder();
            
            // Register default services
            RegisterDefaultServices(builder);
            
            // Allow custom service registration
            configureServices?.Invoke(builder);
            
            var container = builder.Build();
            var adapter = new AutofacContainerAdapter(container);
            ServiceLocatorProvider.SetCurrent(adapter);
            
            return adapter;
        }
        */

        /// <summary>
        /// Register default services that are always available
        /// </summary>
        /// <param name="container">Simple container to configure</param>
        private static void RegisterDefaultServices(SimpleContainerAdapter container)
        {
            // Register error logger
            container.RegisterSingleton<BusinessBaseV2_0_0_Cls.IErrorLogger>(new DefaultErrorLogger());

            // Register other default services as needed
            // container.RegisterSingleton<IOtherService>(new OtherServiceImplementation());
        }

        /// <summary>
        /// Register default services for Unity (when available)
        /// </summary>
        /*
        private static void RegisterDefaultServices(IUnityContainer container)
        {
            container.RegisterSingleton<BusinessBaseV2_0_0_Cls.IErrorLogger, DefaultErrorLogger>();
            // Register other services...
        }
        */

        /// <summary>
        /// Register default services for Autofac (when available)
        /// </summary>
        /*
        private static void RegisterDefaultServices(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultErrorLogger>()
                   .As<BusinessBaseV2_0_0_Cls.IErrorLogger>()
                   .SingleInstance();
            // Register other services...
        }
        */

        /// <summary>
        /// Default Error Logger implementation
        /// </summary>
        private class DefaultErrorLogger : BusinessBaseV2_0_0_Cls.IErrorLogger
        {
            public void Write(string methodName, Exception ex)
            {
                ErrorLogger_Cls.Write(methodName, ex);
            }
        }

        /// <summary>
        /// Reset DI configuration to defaults
        /// </summary>
        public static void Reset()
        {
            ServiceLocatorProvider.Reset();
        }

        /// <summary>
        /// Check if DI container is properly configured
        /// </summary>
        /// <returns>True if configured</returns>
        public static bool IsConfigured()
        {
            try
            {
                var serviceLocator = ServiceLocatorProvider.Current;
                return serviceLocator != null &&
                       serviceLocator.IsRegistered<BusinessBaseV2_0_0_Cls.IErrorLogger>();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get current DI container type name
        /// </summary>
        /// <returns>Container type name</returns>
        public static string GetCurrentContainerType()
        {
            var current = ServiceLocatorProvider.Current;
            return current?.GetType().Name ?? "Not configured";
        }

        /// <summary>
        /// Initialize DI with default configuration if not already configured
        /// </summary>
        public static void EnsureInitialized()
        {
            if (!IsConfigured())
            {
                ConfigureSimpleContainer();
            }
        }
    }

    /// <summary>
    /// DI Container Factory for easy container switching
    /// </summary>
    public static class ContainerFactory
    {
        /// <summary>
        /// Available container types
        /// </summary>
        public enum ContainerType
        {
            Simple,
            Unity,
            Autofac
        }

        /// <summary>
        /// Create and configure a DI container
        /// </summary>
        /// <param name="containerType">Type of container to create</param>
        /// <param name="configureAction">Configuration action</param>
        /// <returns>Configured Service Locator</returns>
        public static IServiceLocator CreateContainer(ContainerType containerType, Action<object> configureAction = null)
        {
            switch (containerType)
            {
                case ContainerType.Simple:
                    return DIConfiguration.ConfigureSimpleContainer(container => configureAction?.Invoke(container));

                case ContainerType.Unity:
                    throw new NotSupportedException("Unity container requires Unity NuGet package. Please install and uncomment Unity support in ContainerAdapters.cs and DIConfiguration.cs");

                case ContainerType.Autofac:
                    throw new NotSupportedException("Autofac container requires Autofac NuGet package. Please install and uncomment Autofac support in ContainerAdapters.cs and DIConfiguration.cs");

                default:
                    throw new ArgumentException($"Unknown container type: {containerType}");
            }
        }

        /// <summary>
        /// Get recommended container type for .NET Framework 4.8
        /// </summary>
        /// <returns>Recommended container type</returns>
        public static ContainerType GetRecommendedContainer()
        {
            // For .NET Framework 4.8, Simple container is most compatible
            // Autofac would be second choice, Unity third
            return ContainerType.Simple;
        }
    }
}
