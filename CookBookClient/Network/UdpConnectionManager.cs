using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace CookBookClient.Network;

public class UdpConnectionManager
{
    public async Task<Response> SendRequestAsync(Request request)
    {
        var client = new UdpClient();
        await SendMessage(request, client);
        var responseBytes = await client.ReceiveAsync();
        var bytes = responseBytes.Buffer;
        Response? response;
        if (request.Type == RequestType.GetImage)
        {
            response = new Response
            {
                Status = ResponseStatus.Success,
                Image = bytes
            };
        }
        else
        {
            var responseJson = Encoding.UTF8.GetString(bytes);
            response = JsonSerializer.Deserialize<Response>(responseJson);
        }

        return response ?? throw new Exception("Serialization failed");
    }
    
    public async Task SendImageAsync(byte[] image)
    {
        var client = new UdpClient();
        await client.SendAsync(image, image.Length, new IPEndPoint(IPAddress.Broadcast, Configuration.ServerPort));
    }

    private static async Task SendMessage(Request request, UdpClient client)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestBytes = Encoding.UTF8.GetBytes(requestJson);
        await client.SendAsync(requestBytes, requestBytes.Length, new IPEndPoint(IPAddress.Broadcast, Configuration.ServerPort));
    }
}