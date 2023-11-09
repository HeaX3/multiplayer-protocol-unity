using System;
using System.Collections.Generic;
using System.Linq;
using Essentials;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MultiplayerProtocol
{
    public sealed class Protocol
    {
        private readonly Dictionary<Type, ushort> idMap = new();
        private readonly Dictionary<ushort, INetworkMessageListener> handlers = new();
        private readonly Dictionary<INetworkMessageListener, ushort> handlerIdMap = new();

        private readonly Dictionary<Guid, ResponseListener> responseListeners = new();

        public Protocol(params INetworkMessageListener[] handlers) : this(
            (IEnumerable<INetworkMessageListener>)handlers)
        {
        }

        public Protocol(IEnumerable<INetworkMessageListener> handlers)
        {
            var list = handlers.ToList();
            list.Insert(0, new RequestMessageHandler(this));
            list.Insert(0, new ResponseMessageHandler(this));
            list.Insert(0, new ProtocolMessageHandler(this));
            for (var i = 0; i < list.Count; i++)
            {
                var handler = list[i];
                var id = (ushort)(i + 1);
                idMap[handler.messageType] = id;
                this.handlers[id] = handler;
                handlerIdMap[handler] = id;
            }
        }

        internal void Reset()
        {
            responseListeners.Clear();
        }

        internal bool TryGetMessageId(Type type, out ushort messageId) => idMap.TryGetValue(type, out messageId);

        public ProtocolMessage CreateProtocolMessage() => new(this);

        internal void AddResponseListener(Guid requestId, uint timeoutMs, Action<IRequestResponse> callback)
        {
            responseListeners[requestId] = new ResponseListener(callback, DateTime.UtcNow.AddMilliseconds(timeoutMs));
        }

        internal void RemoveResponseListener(Guid requestId)
        {
            responseListeners.Remove(requestId);
        }

        internal bool TryGetAndRemoveResponseListener(Guid requestId, out ResponseListener listener)
        {
            if (!responseListeners.TryGetValue(requestId, out listener))
            {
                return false;
            }

            responseListeners.Remove(requestId);
            return true;
        }

        internal JObject ToJson()
        {
            return new JObject().Set("ids", new JArray(handlerIdMap
                .Select(e => new JObject().Set("m", e.Key.messageId).Set("id", e.Value))
                .Cast<object>()
                .ToArray()
            ));
        }

        internal void LoadData(JObject json)
        {
            foreach (var entry in (json.GetArray("ids") ?? new JArray()).OfType<JObject>())
            {
                var messageId = entry.GetString("m");
                var id = entry.GetUShort("id");
                var handler = handlerIdMap.Keys.FirstOrDefault(h => h.messageId == messageId);
                if (handler == null) continue;
                idMap[handler.messageType] = id;
                handlers[id] = handler;
                handlerIdMap[handler] = id;
            }
        }

        public void Handle(SerializedMessage serializedMessage)
        {
            INetworkMessage message;
            INetworkMessageListener handler;
            try
            {
                message = Deserialize(serializedMessage, out handler);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed parsing serialized message:");
                Debug.LogError(e);
                return;
            }

            if (handler is INetworkMessageHandler simpleHandler)
            {
                simpleHandler.Handle(message, serializedMessage);
            }
            else
            {
                throw new InvalidOperationException("Handler " + handler.GetType().Name +
                                                    " expects a request but received a flat message instead!");
            }
        }

        public SerializedMessage Serialize(INetworkMessage message)
        {
            if (!idMap.TryGetValue(message.GetType(), out var messageId))
            {
                throw new InvalidOperationException("Unknown message type " + message.GetType().Name);
            }

            var result = new SerializedMessage(messageId);
            foreach (var value in message.values)
            {
                value.SerializeInto(result);
            }

            return result;
        }

        internal INetworkMessage Deserialize(SerializedMessage message, out INetworkMessageListener handler)
        {
            var typeId = message.ReadUShort();
            return Deserialize(message, typeId, out handler);
        }

        internal INetworkMessage Deserialize(SerializedMessage message, ushort typeId,
            out INetworkMessageListener handler)
        {
            if (!handlers.TryGetValue(typeId, out handler))
            {
                throw new InvalidOperationException("Unknown message type " + typeId);
            }

            var result = handler.CreateMessageInstance();
            foreach (var value in result.values) value.DeserializeFrom(message);
            return result;
        }

        internal class ResponseListener
        {
            private Action<IRequestResponse> callback { get; }
            public DateTime timeout { get; }

            private bool received;

            public ResponseListener(Action<IRequestResponse> callback, DateTime timeout)
            {
                this.callback = callback;
                this.timeout = timeout;
            }

            public void Receive(IRequestResponse response)
            {
                if (received) throw new InvalidOperationException("Response already received");
                received = true;

                if (DateTime.UtcNow > timeout)
                {
                    callback(RequestResponse.RequestTimeout());
                    return;
                }

                callback(response);
            }
        }
    }
}