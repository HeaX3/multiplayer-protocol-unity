using System.Linq;
using UnityEngine;

namespace MultiplayerProtocol.Tests
{
    public class SerializationTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var testValue = new[]
            {
                1, 2, 3, 4
            };
            var serialized = new SerializedData();
            serialized.Write(testValue);
            serialized.Write(testValue);
            var read = serialized.ReadIntArray();
            var read2 = serialized.ReadIntArray();
            Debug.Log(
                "Input: " + string.Join(", ", testValue) + "\n" +
                "Read 1: " + string.Join(", ", read) + "\n" +
                "Read 2: " + string.Join(", ", read2)
            );
            Debug.Log("Success: " + (testValue.Length == read.Length && testValue.Length == read2.Length &&
                                     testValue.Select((v, i) => read[i] == v && read2[i] == v).All(v => v)));
        }
    }
}