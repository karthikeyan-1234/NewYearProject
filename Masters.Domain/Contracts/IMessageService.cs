using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masters.Domain.Contracts
{
    public interface IMessageService
    {
        Task PublishMessage<T>(T obj, string topic);
    }
}
