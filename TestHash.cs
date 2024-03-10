using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CodeTesting
{
    internal class TestHash
    {
        public static void Start()
        {
            var entryOne = new EnrtyOne
            {
                Id = 1,
                Name = "John",
                Departemnt = "IT",
                Store = new SubEntry
                {
                    StorRef = "Store1"
                }
                
            };

            var entryOne2 = new EnrtyOne
            {
                Id = 1,
                Name = "John",
                Departemnt = "IT",
                Store = new SubEntry
                {
                    StorRef = "Store1"
                }
            };

            var enrtyOneHash = GetHash(entryOne);
            var enrtyTwoHash = GetHash(entryOne2, true);

            Console.WriteLine(enrtyOneHash);
            Console.WriteLine(enrtyTwoHash);
            Console.WriteLine(enrtyOneHash == enrtyTwoHash);

        }

        public static string GetHash(string input)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(input);
            return GetHash(data);
        }

        public static string GetHash<T>(T obj, bool useJsonSer = false)
        {
            if (useJsonSer)
            {
                var jAsBytes = SerializeJson(obj);
                return GetHash(jAsBytes);
            }
            var tAsBytes = Serialize(obj);
            return GetHash(tAsBytes);
        }

        private static string GetHash(byte[] tAsBytes)
        {
            var shaM = SHA512.Create();
            
            byte[] result = shaM.ComputeHash(tAsBytes);
            return BitConverter.ToString(result).Replace("-", "");
        }

        // serialize object to byte array

        public static byte[] Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            using MemoryStream ms = new MemoryStream();

            var serlalizer = new DataContractSerializer(typeof(T));
            serlalizer.WriteObject(ms, obj);
            var asStr = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine(asStr);
            ms.Position = 0;
            return ms.ToArray();

        }

        // serialize object to byte array using json
        public static byte[] SerializeJson<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            // write to console
            Console.WriteLine(Encoding.UTF8.GetString(bytes));
            return bytes;
        }

    }

    
    public class EnrtyOne
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Departemnt { get; set; }
        public SubEntry Store { get; set; }
    }

    public class EnrtyTwo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Departemnt { get; set; }

        public SubEntry Store { get; set; }
    }

    public class SubEntry
    {
        public string StorRef { get; set; }
    }

}
