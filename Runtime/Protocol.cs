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
        private readonly Dictionary<ushort, INetworkMessageHandler> handlers = new();
        private readonly Dictionary<INetworkMessageHandler, ushort> handlerIdMap = new();

        public Protocol(params INetworkMessageHandler[] handlers) : this((IEnumerable<INetworkMessageHandler>)handlers)
        {
        }

        public Protocol(IEnumerable<INetworkMessageHandler> handlers)
        {
            var list = handlers.ToList();
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

        public ProtocolMessage CreateProtocolMessage() => new(this);

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

        public void Handle(SerializedMessage serializedMessage)
        {
            INetworkMessage message;
            INetworkMessageHandler handler;
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

            handler.Handle(message);
        }

        private INetworkMessage Deserialize(SerializedMessage message, out INetworkMessageHandler handler)
        {
            var typeId = message.ReadUShort();
            if (!handlers.TryGetValue(typeId, out handler))
            {
                throw new InvalidOperationException("Unknown message type " + typeId);
            }

            var result = handler.CreateMessageInstance();
            foreach (var value in result.values) value.DeserializeFrom(message);
            return result;
        }
    }
}