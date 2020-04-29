using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace FrameMediatR
{
    public interface Handler<T> where T : INotification, INotificationHandler<T>
    {

    }
}
