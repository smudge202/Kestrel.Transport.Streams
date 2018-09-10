using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	public static class WebHostBuilderExtensions
	{
		public static IWebHostBuilder UsePipelineTransport(this IWebHostBuilder builder) => builder.ConfigureServices(services =>
		{
			services.AddSingleton<ITransportFactory, PipelineTransportFactory>();
			services.AddSingleton<ConnectionDispatcherContainer>();
			var transports = services
				.Where(x => x.ServiceType == typeof(ITransportFactory))
				.Select(x => x.AsSelfBound())
				.ToArray();
			foreach (var transport in transports)
				services.TryAdd(transport);
			services.AddSingleton<ITransportFactory, AggregatingTransportFactory>(provider => 
				new AggregatingTransportFactory(transports.Select(x => 
					provider.GetRequiredService(x.ServiceType)).OfType<ITransportFactory>()));
		});

		private static ServiceDescriptor AsSelfBound(this ServiceDescriptor descriptor)
		{
			if (descriptor.ImplementationInstance != null)
				return new ServiceDescriptor(descriptor.ImplementationInstance.GetType(), descriptor.ImplementationInstance);
			if (descriptor.ImplementationType != null)
				return new ServiceDescriptor(descriptor.ImplementationType, descriptor.ImplementationType, descriptor.Lifetime);
			if (descriptor.ImplementationFactory != null)
				return new ServiceDescriptor(typeof(TransportFactoryFacade), provider => new TransportFactoryFacade((ITransportFactory)descriptor.ImplementationFactory(provider)), descriptor.Lifetime);
			throw new NotSupportedException("Unexpected ServiceDescriptor found when binding Pipeline Transport");
		}
	}
}
