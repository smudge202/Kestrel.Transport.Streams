using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	internal sealed class AggregatingTransport : ITransport
	{
		private readonly IEnumerable<ITransport> _transports;

		public AggregatingTransport(IEnumerable<ITransport> transports) 
			=> _transports = transports;

		public Task BindAsync()
			=> Task.WhenAll(_transports.Select(x => x.BindAsync()));

		public Task UnbindAsync()
			=> Task.WhenAll(_transports.Select(x => x.UnbindAsync()));

		public Task StopAsync()
			=> Task.WhenAll(_transports.Select(x => x.StopAsync()));
	}
}
