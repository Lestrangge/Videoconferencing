using System;
using Newtonsoft.Json;

namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class TrickleRequest : JanusBase
    {
        public override string Janus => "trickle";
        public TrickleBody Candidate { get; set; }
        public long? HandleId { get; set; }

    }

    public class TrickleBody
    {
    }

    [JsonConverter(typeof(TrickleConverter))]
    public class TrickleCandidate : TrickleBody
    {
        public string SdpMid { get; set; }
        public int SdpMLineIndex { get; set; }
        public string Candidate { get; set; }
    }

    public class TrickleCompleted : TrickleBody
    {
        public bool Completed { get; set; }
    }


    public class TrickleCandidateReceivedDto : TrickleBody
    {
        public string SdpMid { get; set; }
        public int SdpMLineIndex { get; set; }
        public string Candidate { get; set; }
        public bool Completed { get; set; }
    }


    public class TrickleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TrickleCandidate).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var candidate = (TrickleCandidate)value;
            writer.WriteStartObject();
            writer.WritePropertyName("sdpMid");
            serializer.Serialize(writer, candidate.SdpMid);
            writer.WritePropertyName("sdpMLineIndex");
            serializer.Serialize(writer, candidate.SdpMLineIndex);
            writer.WritePropertyName("candidate");
            serializer.Serialize(writer, candidate.Candidate);
            writer.WriteEndObject();
        }
    }
}
