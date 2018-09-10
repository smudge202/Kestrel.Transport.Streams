using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	internal sealed class ConnectionDispatcherContainer
	{
		public IConnectionDispatcher Dispatcher { get; set; }
	}
}
