using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	internal sealed class TransportFactoryFacade : ITransportFactory
	{
		private readonly ITransportFactory _factory;

		public TransportFactoryFacade(ITransportFactory factory)
			=> _factory = factory;

		public ITransport Create(IEndPointInformation endPointInformation, IConnectionDispatcher dispatcher)
			=> _factory.Create(endPointInformation, dispatcher);
	}
}
