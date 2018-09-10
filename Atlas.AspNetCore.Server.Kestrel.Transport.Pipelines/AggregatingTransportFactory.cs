using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	internal sealed class AggregatingTransportFactory : ITransportFactory
	{
		private readonly IEnumerable<ITransportFactory> _transports;

		public AggregatingTransportFactory(IEnumerable<ITransportFactory> transports) 
			=> _transports = transports;

		public ITransport Create(IEndPointInformation endPointInformation, IConnectionDispatcher dispatcher)
			=> new AggregatingTransport(_transports.Select(x => x.Create(endPointInformation, dispatcher)));
	}
}
