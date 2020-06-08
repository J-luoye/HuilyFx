using System;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Core.SignalR
{
    public class HubEven : Hub
    {
        // Is set via the constructor on each creation
        private Broadcaster _broadcaster;
        public HubEven()
        {
        }
        public HubEven(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }
        public void UpdateModel(MessageModel clientModel)
        {
            clientModel.ClientId = Context.ConnectionId;
            _broadcaster.Send(clientModel);
        }
    }

    public class Broadcaster
    {
        private readonly TimeSpan BroadcastInterval =
            TimeSpan.FromMilliseconds(40);
        private MessageModel _model;
        private readonly IHubContext<HubEven> _hubContext;
        private Timer _broadcastLoop;
        private bool _modelUpdated;
        public Broadcaster(IHubContext<HubEven> context)
        {

            _hubContext = context;//GlobalHost.ConnectionManager.GetHubContext<HubEven>();
            _modelUpdated = false;
            _model = new MessageModel();
            _broadcastLoop = new Timer(
                BroadcastShape,
                null,
                BroadcastInterval,
                BroadcastInterval);
        }
        public void BroadcastShape(object state)
        {
            if (_modelUpdated)
            {
                _hubContext.Clients.AllExcept(_model.ClientId).SendAsync(JsonConvert.SerializeObject(_model));
                _modelUpdated = false;
            }
        }
        public void Send(MessageModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }
    }

    public class MessageModel
    {
        public string ClientId { get; set; }

        public string MessageJsonString { get; set; }
    }
}
