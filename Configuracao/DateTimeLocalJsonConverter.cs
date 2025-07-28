using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeLocalJsonConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-ddTHH:mm:ss"; 

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        return DateTime.SpecifyKind(DateTime.Parse(str), DateTimeKind.Local);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (value.Kind == DateTimeKind.Utc)
        {
            value = value.ToLocalTime();
        }
        writer.WriteStringValue(value.ToString(Format));
    }
}
