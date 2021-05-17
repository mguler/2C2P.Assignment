using _2c2p.Assignment.Tools.Abstraction.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace _2c2p.Assignment.Model.Mapping
{
    public class XmlToTransactionDeserializerMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.RegisterByKey("text/xml", new Func<string, List<Transaction>>((input) =>
            {
                var result = new List<Transaction>();
                var xmlDeserializer = new XmlSerializer(typeof(List<Transaction>));

                var buffer = Encoding.UTF8.GetBytes(input);
                var memoryStream = new MemoryStream(buffer);
                result = (List<Transaction>)xmlDeserializer.Deserialize(memoryStream);

                return result;
            }));
        }
    }
}
