using System;
using System.Collections.Generic;
using System.Linq;
using Essentials;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MultiplayerProtocol
{
    public sealed class Protocol
    {
        public const uint DefaultTimeoutMs = 5000;

        private readonly Dictionary<Type, ushort> idMap = new();
        private readonly Dictionary<ushort, INetworkMessageListener> handlers = new();
        private readonly Dictionary<INetworkMessageListener, ushort> handlerIdMap = new();
        private readonly HashSet<ushort> threadSafeMessageIds = new();

        private readonly Dictionary<string, ushort> partnerProtocol = new();

        private readonly Dictionary<Guid, ResponseListener> responseListeners = new();

        internal Protocol(IEnumerable<INetworkMessageListener> handlers)
        {
            var list = handlers.ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var handler = list[i];
                var id = (ushort)(i + 1);
                idMap[handler.messageType] = id;
                this.handlers[id] = handler;
                handlerIdMap[handler] = id;
                if (handler is IThreadSafeListener) threadSafeMessageIds.Add(id);
            }
        }

        internal void Reset()
        {
            responseListeners.Clear();
        }

        public uint GetDefaultTimeoutMs(ushort messageId)
        {
            return handlers.TryGetValue(messageId, out var handler) &&
                   handler is IAsyncNetworkRequestHandler requestHandler
                ? requestHandler.maxTimeoutMs
                : DefaultTimeoutMs;
        }

        public bool IsThreadSafeMessage(ushort messageId) => threadSafeMessageIds.Contains(messageId);

        internal bool TryGetMessageId(Type type, out ushort messageId) => idMap.TryGetValue(type, out messageId);

        internal bool TryGetPartnerMessageId([NotNull] Type type, out ushort messageId)
        {
            if (partnerProtocol.TryGetValue(type.FullName ?? "", out messageId)) return true;
            return TryGetMessageId(type, out messageId);
        }

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
                partnerProtocol[messageId] = id;
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
            else if (handler is INetworkRequestHandler or IAsyncNetworkRequestHandler)
            {
                Debug.LogError(new InvalidOperationException("Handler " + handler.GetType().Name +
                                                             " expects a request but received a flat message instead!"));
            }
            else
            {
                Debug.LogWarning("Listener " + handler.GetType().Name +
                                 " does not implement any message handler interface");
            }
        }

        public SerializedMessage Serialize(INetworkMessage message)
        {
            if (!TryGetPartnerMessageId(message.GetType(), out var messageId))
            {
                throw new InvalidOperationException("Partner does not understand message of type " +
                                                    message.GetType().FullName);
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