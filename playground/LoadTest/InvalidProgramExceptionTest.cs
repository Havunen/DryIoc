using System;
using System.Diagnostics;
using System.Web.Http;
using DryIoc;
using DryIoc.WebApi;

namespace LoadTest
{
    static class InvalidProgramExceptionTest
    {
        public static IContainer GetContainerForTestFec()
        {
            var container = new Container((rules) =>
                rules
                    .WithoutDependencyDepthToSplitObjectGraph()
                    .WithoutInterpretationForTheFirstResolution()
                    .WithoutUseInterpretation()
                    .With(FactoryMethod.ConstructorWithResolvableArguments)
            ).WithWebApi(new HttpConfiguration());
            Registrations.RegisterTypes(container, true);

            return container;
        }

        public static IContainer GetContainerForTestNoFec()
        {
            var container = new Container(rules => rules
                .WithoutFastExpressionCompiler()
                .WithoutDependencyDepthToSplitObjectGraph()
                .WithoutInterpretationForTheFirstResolution()
                .WithoutUseInterpretation()
                .With(FactoryMethod.ConstructorWithResolvableArguments))
                .WithWebApi(new HttpConfiguration());

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
            Console.WriteLine("Starting InvalidProgramException test");

            var controllerTypes = TestHelper.GetAllControllers();
            IContainer container = GetContainerForTestNoFec();

            var validateResult = container.Validate();

            if (validateResult.Length != 0)
            {
                throw new Exception(validateResult.ToString());
            }

            ResolveAllControllers(container, controllerTypes);
            ResolveAllControllers(container, controllerTypes);
        }

        public static void Start2()
        {
            Console.WriteLine("Starting InvalidProgramException test");

            var controllerTypes = TestHelper.GetAllControllers();
            IContainer container = GetContainerForTestFec();

            Console.WriteLine("Validating everything...");
            var sw = Stopwatch.StartNew();
            var validateResult = container.Validate();
            Console.WriteLine($"Validated in {sw.Elapsed.TotalMilliseconds} ms");


            if (validateResult.Length != 0)
            {
                throw new Exception(validateResult.ToString());
            }

            ResolveAllControllers(container, controllerTypes);
            ResolveAllControllers(container, controllerTypes);
        }
    }
}
