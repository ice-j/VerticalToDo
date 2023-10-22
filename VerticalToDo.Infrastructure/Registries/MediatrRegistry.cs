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

                scanner.ConnectImplementationsToTypesClosing(typeof(BaseValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            });

            // This is the default but let's be explicit. At most we should be container scoped.
            For<IMediator>().Add<Mediator>().Scoped();
            //For<ServiceFactory>().Add(ctx => ctx.GetInstance);

            For(typeof(IRequestHandler<,>)).DecorateAllWith(typeof(MediatorPipelineHandler<,>));
        }
    }
}
