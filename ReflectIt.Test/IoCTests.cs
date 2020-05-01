using System;
using System.ComponentModel;
using ReflectIt.Model;
using Xunit;

namespace ReflectIt.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Can_Resolve_Types()
        {
            var ioc = new Container();
            ioc.For<ILogger>().Use<SqlServerLogger>();

            var logger = ioc.Resolve<ILogger>();
            
            Assert.Equal(typeof(SqlServerLogger), logger.GetType());
        }
        
        [Fact]
        public void Can_Resolve_Types_Without_Default_Ctor()
        {
            var ioc = new Container();
            ioc.For<ILogger>().Use<SqlServerLogger>();
            ioc.For<IRepository<Employee>>().Use<SqlRepository<Employee>>();

            var repository = ioc.Resolve<IRepository<Employee>>();
            
            Assert.Equal(typeof(SqlRepository<Employee>), repository.GetType());
        }
        
        [Fact]
        public void Can_Resolve_Concrete_Type()
        {
            var ioc = new Container();
            ioc.For<ILogger>().Use<SqlServerLogger>();
            ioc.For(typeof(IRepository<>)).Use(typeof(SqlRepository<>));

            var service = ioc.Resolve<InvoiceService>();
            
            Assert.NotNull(service);
        }
    }
}