using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	internal sealed class NoopTransport : ITransport
	{
		public Task BindAsync() => Task.CompletedTask;

		public Task UnbindAsync() => Task.CompletedTask;

		public Task StopAsync() => Task.CompletedTask;
	}
}
