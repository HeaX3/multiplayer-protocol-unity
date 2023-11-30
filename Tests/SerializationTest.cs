using System;
using System.Linq;
using UnityEngine;

namespace MultiplayerProtocol.Tests
{
    public class SerializationTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TestArray();
            TestDateTime();
        }

        private void TestArray()
        {
            Debug.Log("Test Array");
            var testValue = new[]
            {
                1, 2, 3, 4
            };
            var testValue2 = "https://test.com";
            var serialized = new SerializedData();
            serialized.Write(testValue);
            serialized.Write(testValue);
            serialized.Write(testValue2);
            serialized.Write(testValue2);
            var read = serialized.ReadIntArray();
            var read2 = serialized.ReadIntArray();
            var read3 = serialized.ReadString();
            var read4 = serialized.ReadString();
            Debug.Log(
                "Input: " + string.Join(", ", testValue) + "\n" +
                "Read 1: " + string.Join(", ", read) + "\n" +
                "Read 2: " + string.Join(", ", read2) + "\n" +
                "Read 3: " + string.Join(", ", read3) + "\n" +
                "Read 4: " + string.Join(", ", read4)
            );
            Debug.Log("Success: " + (testValue.Length == read.Length && testValue.Length == read2.Length &&
                                     testValue.Select((v, i) => read[i] == v && read2[i] == v).All(v => v) &&
                                     testValue2 == read3 && testValue2 == read4));
        }

        private void TestDateTime()
        {
            Debug.Log("Test DateTime");
            var testValue = DateTime.UtcNow;
            var serialized = new SerializedData();
            serialized.Write(testValue);
            serialized.Write((DateTime)default);
            var read = serialized.ReadDateTime();
            var read2 = serialized.ReadDateTime();
            Debug.Log(
                "Input: " + testValue + "\n" +
                "Read 1: " + read + "\n" +
                "Read 2: " + read2
            );
            Debug.Log("Success: " + (testValue == read && default == read2));
        }
    }
}