using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Essentials;
using GZipCompress;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MultiplayerProtocol
{
    public sealed class SerializedData : IDisposable
    {
        private List<byte> _buffer;
        private byte[] _readableBuffer;
        private int _readPos;

        private byte[] readableBuffer => _readableBuffer ?? ToArray();

        /// <summary>Creates a new empty packet (without an ID).</summary>
        public SerializedData()
        {
            _buffer = new List<byte>(); // Initialize buffer
            _readPos = 0; // Set readPos to 0
        }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="id">The packet ID.</param>
        public SerializedData(ushort id)
        {
            _buffer = new List<byte>(); // Initialize buffer
            _readPos = 0; // Set readPos to 0

            Write(id); // Write packet id to the buffer
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="data">The bytes to add to the packet.</param>
        public SerializedData(byte[] data)
        {
            _buffer = new List<byte>(); // Initialize buffer
            _readPos = 0; // Set readPos to 0

            SetBytes(data);
        }

        #region Functions

        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        /// <param name="data">The bytes to add to the packet.</param>
        public void SetBytes(byte[] data)
        {
            Write(data);
            _readableBuffer = _buffer.ToArray();
        }

        /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
        public void WriteLength()
        {
            _buffer.InsertRange(0,
                BitConverter.GetBytes(_buffer.Count)); // Insert the byte length of the packet at the very beginning
        }

        /// <summary>Inserts the given int at the start of the buffer.</summary>
        /// <param name="value">The int to insert.</param>
        public void InsertInt(int value)
        {
            _buffer.InsertRange(0, BitConverter.GetBytes(value)); // Insert the int at the start of the buffer
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            _readableBuffer = _buffer.ToArray();
            return _readableBuffer;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return _buffer.Count; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - _readPos; // Return the remaining length (unread)
        }

        /// <summary>Resets the packet instance to allow it to be reused.</summary>
        /// <param name="shouldReset">Whether or not to reset the packet.</param>
        public void Reset(bool shouldReset = true)
        {
            if (shouldReset)
            {
                _buffer.Clear(); // Clear buffer
                _readableBuffer = null;
                _readPos = 0; // Reset readPos
            }
            else
            {
                _readPos -= 4; // "Unread" the last read int
            }
        }

        #endregion

        #region Write Data

        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="value">The byte to add.</param>
        public void Write(byte value)
        {
            _buffer.Add(value);
        }

        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="value">The byte array to add.</param>
        public void Write(byte[] value)
        {
            _buffer.AddRange(value);
        }

        /// <summary>Adds a sbyte to the packet.</summary>
        /// <param name="value">The sbyte to add.</param>
        public void Write(sbyte value)
        {
            _buffer.Add((byte)(value + 128));
        }

        /// <summary>Adds a short to the packet.</summary>
        /// <param name="value">The short to add.</param>
        public void Write(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<short> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<short> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a ushort to the packet.</summary>
        /// <param name="value">The ushort to add.</param>
        public void Write(ushort value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<ushort> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<ushort> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds an ushort to the packet.</summary>
        /// <param name="value">The ushort to add.</param>
        public void Write(uint value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<uint> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<uint> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds an int to the packet.</summary>
        /// <param name="value">The int to add.</param>
        public void Write(int value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<int> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<int> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a long to the packet.</summary>
        /// <param name="value">The long to add.</param>
        public void Write(long value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<long> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<long> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a ulong to the packet.</summary>
        /// <param name="value">The ulong to add.</param>
        public void Write(ulong value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<ulong> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<ulong> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a float to the packet.</summary>
        /// <param name="value">The float to add.</param>
        public void Write(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<float> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<float> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="value">The bool to add.</param>
        public void Write(bool value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(IEnumerable<bool> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<bool> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a string to the packet.</summary>
        /// <param name="value">The string to add.</param>
        public void Write([CanBeNull] string value)
        {
            if (value == null)
            {
                Write(-1);
                return;
            }

            if (value.Length == 0)
            {
                Write(0);
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(value);
            Write(bytes.Length); // Add the length of the string to the packet
            _buffer.AddRange(bytes); // Add the string itself
        }

        public void Write(IEnumerable<string> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<string> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a guid to the packet.</summary>
        /// <param name="value">The guid to add.</param>
        public void Write(Guid value)
        {
            Write(value.ToString());
        }

        public void Write(IEnumerable<Guid> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Guid> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a date time to the packet.</summary>
        /// <param name="value">The date time to add.</param>
        public void Write(DateTime value)
        {
            Write(value != default);
            if (value == default) return;
            Write((value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime()).ToBinary());
        }

        public void Write(IEnumerable<DateTime> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<DateTime> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a NamespacedKey to the packet.</summary>
        /// <param name="value">The NamespacedKey to add.</param>
        public void Write(NamespacedKey value)
        {
            Write(value.nameSpace);
            Write(value.key);
        }

        public void Write(IEnumerable<NamespacedKey> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<NamespacedKey> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a Quaternion to the packet.</summary>
        /// <param name="value">The Quaternion to add.</param>
        public void Write(Quaternion value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        public void Write(IEnumerable<Quaternion> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Quaternion> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a Vector4 to the packet.</summary>
        /// <param name="value">The Vector4 to add.</param>
        public void Write(Vector4 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        public void Write(IEnumerable<Vector4> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Vector4> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a Vector3 to the packet.</summary>
        /// <param name="value">The Vector3 to add.</param>
        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        public void Write(IEnumerable<Vector3> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Vector3> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a Vector2 to the packet.</summary>
        /// <param name="value">The Vector2 to add.</param>
        public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }

        public void Write(IEnumerable<Vector2> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Vector2> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a Vector3Int to the packet.</summary>
        /// <param name="value">The Vector3Int to add.</param>
        public void Write(Vector3Int value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        public void Write(IEnumerable<Vector3Int> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Vector3Int> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a Vector2Int to the packet.</summary>
        /// <param name="value">The Vector2Int to add.</param>
        public void Write(Vector2Int value)
        {
            Write(value.x);
            Write(value.y);
        }

        public void Write(IEnumerable<Vector2Int> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Vector2Int> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        public void Write(Keyframe value)
        {
            Write(value.value);
            Write(value.time);
            Write(value.inTangent);
            Write(value.outTangent);
            Write(value.inWeight);
            Write(value.outWeight);
        }

        public void Write(IEnumerable<Keyframe> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<Keyframe> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        public void Write(AnimationCurve curve)
        {
            var keyframes = curve.keys;
            Write(keyframes);
            WriteEnum(curve.preWrapMode);
            WriteEnum(curve.postWrapMode);
        }

        public void Write(IEnumerable<AnimationCurve> value)
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            if (value is not IReadOnlyCollection<AnimationCurve> valueArray) valueArray = value.ToArray();
            Write(valueArray.Count);
            foreach (var item in valueArray) Write(item);
        }

        /// <summary>Adds a JToken to the packet.</summary>
        /// <param name="value">The JToken key to add.</param>
        /// <param name="compress">Whether to compress the json before writing it.</param>
        public void Write(JToken value, bool compress = false)
        {
            Write(value != null && compress ? GZipCompressor.CompressString(value.ToString()) : value?.ToString());
        }

        /// <summary>Adds a serializable value to the packet.</summary>
        /// <param name="value">The serializable value to add.</param>
        public void Write<T>([CanBeNull] T value) where T : ISerializableValue
        {
            Write(value != null);
            if (value == null) return;
            value.SerializeInto(this);
        }

        /// <summary>Adds a collection of serializable values to the packet.</summary>
        /// <param name="value">The serializable values to add.</param>
        public void Write<T>([CanBeNull] IEnumerable<T> value) where T : ISerializableValue, new()
        {
            if (value == null)
            {
                Write(0);
                return;
            }

            var list = value.ToArray();
            var length = list.Length;
            Write(length);
            if (length == 0) return;

            var raw = new SerializedData();
            foreach (var item in list)
            {
                raw.Write(item != null);
                if (item == null) continue;
                item.SerializeInto(raw);
            }

            var compressed = GZipCompressor.Compress(raw.ToArray());
            Write(compressed.Length);
            Write(compressed);
        }

        public void WriteEnum<T>(T value) where T : struct, Enum
        {
            Write((int)(object)value);
        }

        #endregion

        #region Read Data

        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                byte value = readableBuffer[_readPos]; // Get the byte at readPos' position
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 1; // Increase readPos by 1
                }

                return value; // Return the byte
            }

            throw new Exception("Could not read value of type 'byte'!");
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="length">The length of the byte array.</param>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int length, bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                byte[] value = _buffer.GetRange(_readPos, length)
                    .ToArray(); // Get the bytes at readPos' position with a range of _length
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += length; // Increase readPos by _length
                }

                return value; // Return the bytes
            }

            throw new Exception("Could not read value of type 'byte[]'!");
        }

        /// <summary>Reads a sbyte from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public sbyte ReadSByte(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                sbyte value = (sbyte)(readableBuffer[_readPos] - 128); // Get the byte at readPos' position
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 1; // Increase readPos by 1
                }

                return value; // Return the sbyte
            }

            throw new Exception("Could not read value of type 'sbyte'!");
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                short value = BitConverter.ToInt16(readableBuffer, _readPos); // Convert the bytes to a short
                if (moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    _readPos += 2; // Increase readPos by 2
                }

                return value; // Return the short
            }

            throw new Exception("Could not read value of type 'short'!");
        }

        public short[] ReadShortArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<short>();
            }

            var result = new short[length];
            for (var i = 0; i < length; i++) result[i] = ReadShort();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a ushort from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public ushort ReadUShort(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                ushort value = BitConverter.ToUInt16(readableBuffer, _readPos); // Convert the bytes to a short
                if (moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    _readPos += 2; // Increase readPos by 2
                }

                return value; // Return the short
            }

            throw new Exception("Could not read value of type 'ushort'!");
        }

        public ushort[] ReadUShortArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<ushort>();
            }

            var result = new ushort[length];
            for (var i = 0; i < length; i++) result[i] = ReadUShort();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                int value = BitConverter.ToInt32(readableBuffer, _readPos); // Convert the bytes to an int
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 4; // Increase readPos by 4
                }

                return value; // Return the int
            }

            throw new Exception("Could not read value of type 'int'!");
        }

        public int[] ReadIntArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<int>();
            }

            Debug.Log("Length: " + length);
            var result = new int[length];
            for (var i = 0; i < length; i++) result[i] = ReadInt();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads an uint from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public uint ReadUInt(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                uint value = BitConverter.ToUInt32(readableBuffer, _readPos); // Convert the bytes to an uint
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 4; // Increase readPos by 4
                }

                return value; // Return the uint
            }

            throw new Exception("Could not read value of type 'uint'!");
        }

        public uint[] ReadUIntArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<uint>();
            }

            var result = new uint[length];
            for (var i = 0; i < length; i++) result[i] = ReadUInt();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                long value = BitConverter.ToInt64(readableBuffer, _readPos); // Convert the bytes to a long
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 8; // Increase readPos by 8
                }

                return value; // Return the long
            }

            throw new Exception("Could not read value of type 'long'!");
        }

        public long[] ReadLongArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<long>();
            }

            var result = new long[length];
            for (var i = 0; i < length; i++) result[i] = ReadLong();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a ulong from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public ulong ReadULong(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                ulong value = BitConverter.ToUInt64(readableBuffer, _readPos); // Convert the bytes to au long
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 8; // Increase readPos by 8
                }

                return value; // Return the ulong
            }

            throw new Exception("Could not read value of type 'ulong'!");
        }

        public ulong[] ReadULongArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<ulong>();
            }

            var result = new ulong[length];
            for (var i = 0; i < length; i++) result[i] = ReadULong();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                float value = BitConverter.ToSingle(readableBuffer, _readPos); // Convert the bytes to a float
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 4; // Increase readPos by 4
                }

                return value; // Return the float
            }

            throw new Exception("Could not read value of type 'float'!");
        }

        public float[] ReadFloatArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<float>();
            }

            var result = new float[length];
            for (var i = 0; i < length; i++) result[i] = ReadFloat();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                bool value = BitConverter.ToBoolean(readableBuffer, _readPos); // Convert the bytes to a bool
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 1; // Increase readPos by 1
                }

                return value; // Return the bool
            }

            throw new Exception("Could not read value of type 'bool'!");
        }

        public bool[] ReadBoolArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<bool>();
            }

            var result = new bool[length];
            for (var i = 0; i < length; i++) result[i] = ReadBool();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool moveReadPos = true)
        {
            var readPos = _readPos;
            var length = ReadInt(); // Get the length of the string
            try
            {
                if (length < 0)
                {
                    if (!moveReadPos) _readPos = readPos;
                    return null;
                }

                if (length == 0)
                {
                    if (!moveReadPos) _readPos = readPos;
                    return "";
                }

                var value =
                    Encoding.UTF8.GetString(readableBuffer, _readPos, length); // Convert the bytes to a string
                if (moveReadPos)
                {
                    // If _moveReadPos is true string is not empty
                    _readPos += length; // Increase readPos by the length of the string
                }

                return value; // Return the string
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                var decodedValue = Encoding.UTF8.GetString(readableBuffer, _readPos, readableBuffer.Length - _readPos);
                Debug.LogError("Decoded length: " + decodedValue.Length + " Attempted display: " + decodedValue);
                throw new Exception("Could not read value of type 'string' (Expected length: " + length +
                                    ", Available length: " + (readableBuffer.Length - _readPos) + ")!");
            }
        }

        public string[] ReadStrings(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<string>();
            }

            var result = new string[length];
            for (var i = 0; i < length; i++) result[i] = ReadString();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a guid from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Guid ReadGuid(bool moveReadPos = true)
        {
            var stringValue = ReadString(moveReadPos);
            return stringValue != null && Guid.TryParse(stringValue, out var result) ? result : default;
        }

        public Guid[] ReadGuids(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Guid>();
            }

            var result = new Guid[length];
            for (var i = 0; i < length; i++) result[i] = ReadGuid();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a date time from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public DateTime ReadDateTime(bool moveReadPos = true)
        {
            var readPos = _readPos;
            if (!ReadBool())
            {
                if (!moveReadPos) _readPos = readPos;
                return default;
            }

            var dateData = ReadLong();
            if (!moveReadPos) _readPos = readPos;
            if (dateData <= DateTime.MinValue.Ticks || dateData >= DateTime.MaxValue.Ticks)
            {
                Debug.LogWarning("DateTime value was unexpectedly below the minimum value: " + dateData);
                return default;
            }

            var result = DateTime.FromBinary(dateData);
            return result;
        }

        public DateTime[] ReadDateTimes(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<DateTime>();
            }

            var result = new DateTime[length];
            for (var i = 0; i < length; i++) result[i] = ReadDateTime();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a namespaced key from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public NamespacedKey ReadNamespacedKey(bool moveReadPos = true)
        {
            var position = _readPos;
            var nameSpace = ReadString();
            var key = ReadString();
            if (!moveReadPos) _readPos = position;
            return new NamespacedKey(nameSpace, key);
        }

        public NamespacedKey[] ReadNamespacedKeys(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<NamespacedKey>();
            }

            var result = new NamespacedKey[length];
            for (var i = 0; i < length; i++) result[i] = ReadNamespacedKey();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a quaternion from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Quaternion ReadQuaternion(bool moveReadPos = true)
        {
            var position = _readPos;
            var x = ReadFloat();
            var y = ReadFloat();
            var z = ReadFloat();
            var w = ReadFloat();
            if (!moveReadPos) _readPos = position;
            return new Quaternion(x, y, z, w);
        }

        public Quaternion[] ReadQuaternions(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Quaternion>();
            }

            var result = new Quaternion[length];
            for (var i = 0; i < length; i++) result[i] = ReadQuaternion();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a Vector2Int from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Vector2Int ReadVector2Int(bool moveReadPos = true)
        {
            var position = _readPos;
            var x = ReadInt();
            var y = ReadInt();
            if (!moveReadPos) _readPos = position;
            return new Vector2Int(x, y);
        }

        public Vector2Int[] ReadVector2IntArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Vector2Int>();
            }

            var result = new Vector2Int[length];
            for (var i = 0; i < length; i++) result[i] = ReadVector2Int();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a Vector2 from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Vector2 ReadVector2(bool moveReadPos = true)
        {
            var position = _readPos;
            var x = ReadFloat();
            var y = ReadFloat();
            if (!moveReadPos) _readPos = position;
            return new Vector2(x, y);
        }

        public Vector2[] ReadVector2Array(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Vector2>();
            }

            var result = new Vector2[length];
            for (var i = 0; i < length; i++) result[i] = ReadVector2();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a Vector3Int from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Vector3Int ReadVector3Int(bool moveReadPos = true)
        {
            var position = _readPos;
            var x = ReadInt();
            var y = ReadInt();
            var z = ReadInt();
            if (!moveReadPos) _readPos = position;
            return new Vector3Int(x, y, z);
        }

        public Vector3Int[] ReadVector3IntArray(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Vector3Int>();
            }

            var result = new Vector3Int[length];
            for (var i = 0; i < length; i++) result[i] = ReadVector3Int();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a Vector3 from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Vector3 ReadVector3(bool moveReadPos = true)
        {
            var position = _readPos;
            var x = ReadFloat();
            var y = ReadFloat();
            var z = ReadFloat();
            if (!moveReadPos) _readPos = position;
            return new Vector3(x, y, z);
        }

        public Vector3[] ReadVector3Array(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Vector3>();
            }

            var result = new Vector3[length];
            for (var i = 0; i < length; i++) result[i] = ReadVector3();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a Vector4 from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Vector4 ReadVector4(bool moveReadPos = true)
        {
            var position = _readPos;
            var x = ReadFloat();
            var y = ReadFloat();
            var z = ReadFloat();
            var w = ReadFloat();
            if (!moveReadPos) _readPos = position;
            return new Vector4(x, y, z, w);
        }

        public Vector4[] ReadVector4Array(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Vector4>();
            }

            var result = new Vector4[length];
            for (var i = 0; i < length; i++) result[i] = ReadVector4();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        public Keyframe ReadKeyframe(bool moveReadPos = true)
        {
            var readPos = _readPos;
            var time = ReadFloat();
            var value = ReadFloat();
            var inTangent = ReadFloat();
            var outTangent = ReadFloat();
            var inWeight = ReadFloat();
            var outWeight = ReadFloat();
            if (!moveReadPos) _readPos = readPos;
            return new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
        }

        public Keyframe[] ReadKeyframes(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<Keyframe>();
            }

            var result = new Keyframe[length];
            for (var i = 0; i < length; i++) result[i] = ReadKeyframe();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        public AnimationCurve ReadAnimationCurve(bool moveReadPos = true)
        {
            var readPos = _readPos;
            var keyframes = ReadKeyframes(moveReadPos);
            var preWrapMode = ReadEnum<WrapMode>();
            var postWrapMode = ReadEnum<WrapMode>();

            if (!moveReadPos) _readPos = readPos;

            return new AnimationCurve(keyframes)
            {
                preWrapMode = preWrapMode,
                postWrapMode = postWrapMode
            };
        }

        public AnimationCurve[] ReadAnimationCurves(bool moveReadPos = true)
        {
            var position = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = position;
                return Array.Empty<AnimationCurve>();
            }

            var result = new AnimationCurve[length];
            for (var i = 0; i < length; i++) result[i] = ReadAnimationCurve();
            if (!moveReadPos) _readPos = position;
            return result;
        }

        /// <summary>Reads a JObject from the packet.</summary>
        /// <param name="decompress">Whether to decompress the data before parsing it.</param>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public JObject ReadJson(bool decompress = false, bool moveReadPos = true)
        {
            var stringValue = ReadString(moveReadPos);
            if (stringValue == null) return null;
            if (decompress) stringValue = GZipCompressor.DecompressString(stringValue);
            try
            {
                return JObject.Parse(stringValue);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Reads a JArray from the packet.</summary>
        /// <param name="decompress">Whether to decompress the data before parsing it.</param>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public JArray ReadJsonArray(bool decompress = false, bool moveReadPos = true)
        {
            var stringValue = ReadString(moveReadPos);
            if (stringValue == null) return null;
            if (decompress) stringValue = GZipCompressor.DecompressString(stringValue);
            try
            {
                return JArray.Parse(stringValue);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Reads a serialized value from the packet.</summary>
        [CanBeNull]
        public T Read<T>(bool moveReadPos = true) where T : ISerializableValue, new()
        {
            var readPos = _readPos;
            if (!ReadBool())
            {
                if (!moveReadPos) _readPos = readPos;
                return default;
            }

            var result = new T();
            result.DeserializeFrom(this);
            if (!moveReadPos) _readPos = readPos;
            return result;
        }

        public T ReadEnum<T>(bool moveReadPos = true) where T : struct, Enum
        {
            return (T)(object)ReadInt(moveReadPos);
        }

        /// <summary>Reads a serialized array from the packet.</summary>
        [NotNull]
        [ItemCanBeNull]
        public T[] ReadArray<T>(bool moveReadPos = true) where T : ISerializableValue, new()
        {
            var readPos = _readPos;
            var length = ReadInt();
            if (length == 0)
            {
                if (!moveReadPos) _readPos = readPos;
                return Array.Empty<T>();
            }

            var rawLength = ReadInt();
            var compressed = ReadBytes(rawLength);
            var raw = new SerializedData(GZipCompressor.Decompress(compressed));
            var value = new T[length];
            for (var i = 0; i < length; i++)
            {
                if (!raw.ReadBool()) continue;
                var entry = new T();
                entry.DeserializeFrom(raw);
                value[i] = entry;
            }

            if (!moveReadPos) _readPos = readPos;
            return value;
        }

        #endregion

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _buffer = null;
                    _readableBuffer = null;
                    _readPos = 0;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}