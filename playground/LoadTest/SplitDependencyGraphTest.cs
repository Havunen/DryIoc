using System;
using System.Web.Http;
using DryIoc;
using DryIoc.WebApi;

namespace LoadTest
{
    static class SplitDependencyGraphTest
    {

        public static IContainer GetContainerForTestNoFec(int depth)
        {
            var container = new Container((rules) =>
                rules
                    .WithoutFastExpressionCompiler()
                    .WithDependencyDepthToSplitObjectGraph(depth)
                    .WithoutInterpretationForTheFirstResolution()
                    .WithoutUseInterpretation()
                    .With(FactoryMethod.ConstructorWithResolvableArguments)
            ).WithWebApi(new HttpConfiguration());

            Registrations.RegisterTypes(container, true);

            return container;
        }

        public static IContainer GetContainerForTestFec(int depth)
        {
            var container = new Container((rules) =>
                rules
                    .WithDependencyDepthToSplitObjectGraph(depth)
                    .WithoutInterpretationForTheFirstResolution()
                    .WithoutUseInterpretation()
                    .With(FactoryMethod.ConstructorWithResolvableArguments)
            ).WithWebApi(new HttpConfiguration());

            Registrations.RegisterTypes(container, true);

            return container;
        }

        private static void ResolveAllControllers(IContainer container, Type[] controllerTypes)
        {
            foreach (var controllerType in controllerTypes)
            {
                using (var scope = container.OpenScope(Reuse.WebRequestScopeName))
                {
                    var controller = scope.Resolve(controllerType);

                    if (controller == null)
                    {
                        throw new Exception("Invalid result!");
                    }
                }
            }
        }

        public static void Start()
        {
            Console.WriteLine("Starting WithDependencyDepthToSplitObjectGraph test");

            var controllerTypes = TestHelper.GetAllControllers();

            for (var depth = 1; depth < 50; depth++)
            {
                Console.WriteLine("Depth " + depth);

                var container = GetContainerForTestNoFec(depth);

                ResolveAllControllers(container, controllerTypes);
                ResolveAllControllers(container, controllerTypes);
            }
        }

        public static void Start2()
        {
            Console.WriteLine("Starting WithDependencyDepthToSplitObjectGraph test");

            var controllerTypes = TestHelper.GetAllControllers();

            for (var depth = 1; depth < 50; depth++)
            {
                Console.WriteLine("Depth " + depth);

                var container = GetContainerForTestFec(depth);

                ResolveAllControllers(container, controllerTypes);
                ResolveAllControllers(container, controllerTypes);
            }
        }
    }
}
