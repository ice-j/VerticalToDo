using Lamar;
using MediatR;
using VerticalToDo.Infrastructure.Decorators;
using VerticalToDo.Services;

namespace VerticalToDo.Infrastructure.Registries
{
    public class MediatrRegistry : ServiceRegistry
    {
        public MediatrRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.AssemblyContainingType(typeof(BaseHandler<,>));
                scanner.AssemblyContainingType(typeof(IRequestHandler<,>));

                scanner.ConnectImplementationsToTypesClosing(typeof(BaseValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(BasePaginationValidator<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });

            // This is the default but let's be explicit. At most we should be container scoped.
            For<IMediator>().Use<Mediator>().Setter<IMediator>("Mediator");
            For<ServiceFactory>().Use(ctx => ctx.GetInstance);

            Policies.SetAllProperties(c =>
            {
                c.NameMatches(x => x == "Mediator");
            });

            // Configure decorators over feature handlers
            For(typeof(IRequestHandler<,>)).DecorateAllWith(typeof(MediatorPipelineHandler<,>));
        }
    }
}
