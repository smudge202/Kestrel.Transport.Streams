using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	internal sealed class PipelineTransportFactory : ITransportFactory
	{
		private readonly ConnectionDispatcherContainer _container;

		public PipelineTransportFactory(ConnectionDispatcherContainer container) 
			=> _container = container;

		public ITransport Create(IEndPointInformation endPointInformation, IConnectionDispatcher dispatcher)
		{
			Pipeline.Dispatcher = _container.Dispatcher = dispatcher;
			return new NoopTransport();
		}
	}
}
