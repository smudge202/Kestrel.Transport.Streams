using System;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	public class PipelineTransportOptions
	{
		/// <summary>
		/// The number of I/O queues used to process requests. Set to 0 to directly schedule I/O to the ThreadPool.
		/// </summary>
		/// <remarks>
		/// Defaults to <see cref="Environment.ProcessorCount" /> rounded down and clamped between 1 and 16.
		/// </remarks>
		public int IOQueueCount { get; set; } = Math.Min(Environment.ProcessorCount, 16);
	}
}
