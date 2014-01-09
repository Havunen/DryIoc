﻿using System;
using System.Reflection;
using NUnit.Framework;

namespace DryIoc.Samples
{
    [TestFixture]
    public class LazyImplementationTypeLoading
    {
        [Test]
        public void Register_implementation_on_demand_from_dynamically_loaded_assembly()
        {
            var container = new Container();

            var addinAssemblyPath = "DryIoc.Sample.CUT.dll";
            var addinAssembly = new Lazy<Assembly>(() => Assembly.LoadFrom(addinAssemblyPath));

            container.Register<IAddin>(new FactoryProvider(
                (_, __) => new ReflectionFactory(addinAssembly.Value.GetType("DryIoc.Sample.CUT.SomeAddin"), Reuse.Singleton)));
            
            container.Register<AddinUser>();

            var userOne = container.Resolve<AddinUser>();
            var userTwo = container.Resolve<AddinUser>();

            Assert.That(userOne.Addin, Is.SameAs(userTwo.Addin));
        }
    }

    public interface IAddin {}

    public class AddinUser
    {
        public IAddin Addin { get; set; }

        public AddinUser(IAddin addin)
        {
            Addin = addin;
        }
    }
}
