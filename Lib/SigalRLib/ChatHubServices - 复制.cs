using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SigalRLib
{
    public class ChatHubServices<T> : Hub<T> where T : class
    {

    }
}
