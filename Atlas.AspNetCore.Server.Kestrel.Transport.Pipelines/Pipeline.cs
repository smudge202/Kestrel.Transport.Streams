using System;
using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;

namespace Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines
{
	public class Pipeline
	{
		internal static IConnectionDispatcher Dispatcher { private get; set; }

		public static Task<string> GetAsync(string path)
			=> GetAsync(path, CancellationToken.None);

		public static async Task<string> GetAsync(string path, CancellationToken ct)
		{
			var connection = new PipelineConnection();
			Dispatcher.OnConnection(connection);
			//_ = connection.StartAsync();
			var request = $@"GET {path} HTTP/1.1
Connection: Keep-Alive
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
Accept-Language: en-US,en;q=0.8
Host: localhost
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36


";
			var requestBytes = Encoding.UTF8.GetBytes(request);
			await connection.Input.WriteAsync(requestBytes, ct);
			await connection.Input.FlushAsync(ct);
			connection.Input.Complete();
			var responseBuilder = new StringBuilder();

			while (true)
			{
				var result = await connection.Output.ReadAsync(ct);
				var buffer = result.Buffer;
				if (result.IsCanceled || buffer.IsEmpty) break;

				var resultBytes = buffer.ToArray();
				responseBuilder.Append(Encoding.UTF8.GetString(resultBytes));

				connection.Output.AdvanceTo(buffer.Start, buffer.End);
				if (result.IsCompleted) break;
			}

			var response = responseBuilder.ToString();
			var indexOfFireDoubleLineBreak = response.IndexOf("\r\n\r\n", StringComparison.Ordinal);
			var headers = response.Substring(0, indexOfFireDoubleLineBreak);

			if (headers.Contains("Transfer-Encoding: chunked"))
				return BuildChunkedResponse(response, indexOfFireDoubleLineBreak + 4);
			return response.Substring(indexOfFireDoubleLineBreak + 4);
		}

		private static string BuildChunkedResponse(string response, int index)
		{
			var bodyBuilder = new StringBuilder();
			while (index < response.Length)
			{
				var nextLineBreak = response.IndexOf("\r\n", index, StringComparison.Ordinal);
				var chunk = response.Substring(index, nextLineBreak - index);
				var chunkLength = Convert.ToInt32(chunk, 16);
				bodyBuilder.Append(response.Substring(nextLineBreak + 2, chunkLength));
				index = nextLineBreak + 2 + chunkLength + 2;
			}
			return bodyBuilder.ToString();
		}
	}
}
