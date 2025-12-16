using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Dk.Odense.SSP.Web.Helpers
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));

            var res = DateTime.Parse(reader.GetString());
            return res;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToLocalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
        }
    }
}
