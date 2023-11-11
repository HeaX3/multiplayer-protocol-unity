using System;
using System.Collections.Generic;
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

        /// <summary>Adds a short to the packet.</summary>
        /// <param name="value">The short to add.</param>
        public void Write(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds a ushort to the packet.</summary>
        /// <param name="value">The ushort to add.</param>
        public void Write(ushort value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds an ushort to the packet.</summary>
        /// <param name="value">The ushort to add.</param>
        public void Write(uint value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds an int to the packet.</summary>
        /// <param name="value">The int to add.</param>
        public void Write(int value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds a long to the packet.</summary>
        /// <param name="value">The long to add.</param>
        public void Write(long value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds a ulong to the packet.</summary>
        /// <param name="value">The ulong to add.</param>
        public void Write(ulong value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds a float to the packet.</summary>
        /// <param name="value">The float to add.</param>
        public void Write(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="value">The bool to add.</param>
        public void Write(bool value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
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

            var bytes = Encoding.UTF8.GetBytes(value);
            Write(bytes.Length); // Add the length of the string to the packet
            _buffer.AddRange(bytes); // Add the string itself
        }

        /// <summary>Adds a guid to the packet.</summary>
        /// <param name="value">The guid to add.</param>
        public void Write(Guid value)
        {
            Write(value.ToString());
        }

        /// <summary>Adds a namespaced key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(NamespacedKey value)
        {
            Write(value.ToString());
        }

        /// <summary>Adds a quaternion key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(Quaternion value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        /// <summary>Adds a vector4 key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(Vector4 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        /// <summary>Adds a vector3 key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        /// <summary>Adds a vector2 key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }

        /// <summary>Adds a vector3int key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(Vector3Int value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        /// <summary>Adds a vector2int key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        public void Write(Vector2Int value)
        {
            Write(value.x);
            Write(value.y);
        }

        /// <summary>Adds a JToken key to the packet.</summary>
        /// <param name="value">The namespaced key to add.</param>
        /// <param name="compress">Whether to compress the json before writing it.</param>
        public void Write(JToken value, bool compress = false)
        {
            Write(value != null && compress ? GZipCompressor.CompressString(value.ToString()) : value?.ToString());
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
                byte value = _readableBuffer[_readPos]; // Get the byte at readPos' position
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

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                short value = BitConverter.ToInt16(_readableBuffer, _readPos); // Convert the bytes to a short
                if (moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    _readPos += 2; // Increase readPos by 2
                }

                return value; // Return the short
            }

            throw new Exception("Could not read value of type 'short'!");
        }

        /// <summary>Reads a ushort from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public ushort ReadUShort(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                ushort value = BitConverter.ToUInt16(_readableBuffer, _readPos); // Convert the bytes to a short
                if (moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    _readPos += 2; // Increase readPos by 2
                }

                return value; // Return the short
            }

            throw new Exception("Could not read value of type 'ushort'!");
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                int value = BitConverter.ToInt32(_readableBuffer, _readPos); // Convert the bytes to an int
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 4; // Increase readPos by 4
                }

                return value; // Return the int
            }

            throw new Exception("Could not read value of type 'int'!");
        }

        /// <summary>Reads an uint from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public uint ReadUInt(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                uint value = BitConverter.ToUInt32(_readableBuffer, _readPos); // Convert the bytes to an uint
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 4; // Increase readPos by 4
                }

                return value; // Return the uint
            }

            throw new Exception("Could not read value of type 'uint'!");
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                long value = BitConverter.ToInt64(_readableBuffer, _readPos); // Convert the bytes to a long
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 8; // Increase readPos by 8
                }

                return value; // Return the long
            }

            throw new Exception("Could not read value of type 'long'!");
        }

        /// <summary>Reads a ulong from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public ulong ReadULong(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                ulong value = BitConverter.ToUInt64(_readableBuffer, _readPos); // Convert the bytes to au long
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 8; // Increase readPos by 8
                }

                return value; // Return the ulong
            }

            throw new Exception("Could not read value of type 'ulong'!");
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                float value = BitConverter.ToSingle(_readableBuffer, _readPos); // Convert the bytes to a float
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 4; // Increase readPos by 4
                }

                return value; // Return the float
            }

            throw new Exception("Could not read value of type 'float'!");
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool moveReadPos = true)
        {
            if (_buffer.Count > _readPos)
            {
                // If there are unread bytes
                bool value = BitConverter.ToBoolean(_readableBuffer, _readPos); // Convert the bytes to a bool
                if (moveReadPos)
                {
                    // If _moveReadPos is true
                    _readPos += 1; // Increase readPos by 1
                }

                return value; // Return the bool
            }

            throw new Exception("Could not read value of type 'bool'!");
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool moveReadPos = true)
        {
            try
            {
                int length = ReadInt(); // Get the length of the string
                if (length < 0)
                {
                    return null;
                }

                if (length == 0) return "";

                string value =
                    Encoding.UTF8.GetString(_readableBuffer, _readPos, length); // Convert the bytes to a string
                if (moveReadPos)
                {
                    // If _moveReadPos is true string is not empty
                    _readPos += length; // Increase readPos by the length of the string
                }

                return value; // Return the string
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }

        /// <summary>Reads a guid from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Guid ReadGuid(bool moveReadPos = true)
        {
            var stringValue = ReadString(moveReadPos);
            return stringValue != null && Guid.TryParse(stringValue, out var result) ? result : default;
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