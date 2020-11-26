using System;
using System.Collections.Generic;
using Dependency_Injection_Library;
using Xunit;
using Xunit.Sdk;

namespace TestProject1
{
    interface IService {}

    class ServiceImpl1 : IService {}

    class ServiceImpl2 : IService {}
    
    abstract class AbstractService : IService {}
    
    class Service2 : AbstractService {}
    
    class ServiceImpl : IService
    {
        public ServiceImpl(IRepository repository) {}
    }

    interface IRepository{}
    
    class RepositoryImpl : IRepository
    {
        public RepositoryImpl(){}
    }
    
    interface IService<TRepository> where TRepository : IRepository {}

    class ServiceImpl<TRepository> : IService<TRepository> 
        where TRepository : IRepository
    {
        public TRepository Repository;
        public ServiceImpl(TRepository repository)
        {
            Repository = repository;
        }
    }

    class MySqlRepository : IRepository {}

    public class Tests
    {
        [Fact]
        public void Count()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl1>();
            dependencies.Register<AbstractService, Service2>();
            dependencies.Register<ServiceImpl1, ServiceImpl1>();
            Assert.Equal(3, dependencies.Count());
        }
        
        [Fact]
        public void ResolveBasic()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl1>();
            dependencies.Register<AbstractService, Service2>();
            
            var provider = new DependencyProvider(dependencies);
            var service1 = provider.Resolve<IService>();

            Assert.Equal( typeof(ServiceImpl1), service1.GetType());
        }
        
        [Fact]
        public void ResolveRecursive()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl>();
            dependencies.Register<IRepository, RepositoryImpl>();
            
            var provider = new DependencyProvider(dependencies);
            var service = provider.Resolve<IService>();

            Assert.Equal( typeof(ServiceImpl), service.GetType());
        }
        
        
        [Fact]
        public void ResolveSingleton()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl1>(isSingleton: true);
            
            var provider = new DependencyProvider(dependencies);
            var service1 = provider.Resolve<IService>();
            var service2 = provider.Resolve<IService>();

            Assert.Same( service1, service2);
        }
        
        [Fact]
        public void ResolveEnumerable()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IService, ServiceImpl1>();
            dependencies.Register<IService, ServiceImpl2>();
            
            var provider = new DependencyProvider(dependencies);
            var services = provider.Resolve<IEnumerable<IService>>();
        
            Assert.Equal(2, (services as List<object>)?.Count);    
        }
        
        [Fact]
        public void ResolveGenericDependency()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IRepository, MySqlRepository>();
            dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();
            
            var provider = new DependencyProvider(dependencies);
            var service = provider.Resolve<IService<IRepository>>();
        
            Assert.NotNull((service as ServiceImpl<IRepository>)?.Repository);    
        }
        
        [Fact]
        public void ResolveOpenGenericsDependency()
        {
            var dependencies = new DependenciesConfig();
            dependencies.Register<IRepository, MySqlRepository>();
            dependencies.Register(typeof(IService<>), typeof(ServiceImpl<>));
            
            var provider = new DependencyProvider(dependencies);
            var service = provider.Resolve<IService<IRepository>>();
        
            Assert.NotNull((service as ServiceImpl<IRepository>)?.Repository);    
        }
    }
}