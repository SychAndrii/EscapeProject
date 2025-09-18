using System.Text.Json.Serialization;

namespace EscapeProject.Task
{
    public class TaskEntity
    {
        public string name { get; set; } = string.Empty;

        [JsonConverter(typeof(StrictDateTimeConverter))]
        public DateTime? from
        {
            get; set;
        }

        [JsonConverter(typeof(StrictDateTimeConverter))]
        public DateTime? until
        {
            get; set;
        }
    }
}
