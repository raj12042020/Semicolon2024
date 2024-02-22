using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CandidatesHiring.Model
{
    public class Profile
    {
        public Profile() { }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId _id { get; set; } = ObjectId.Empty;


        [BsonElement("CandidateID")]
        [JsonPropertyName("CandidateID")]
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [BsonElement("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [BsonElement("ContactNumber")]
        [JsonPropertyName("ContactNumber")]
        public Int32 ContactNo { get; set; }

        [BsonElement("Current Role")]
        [JsonPropertyName("Current Role")]
        public string Designation { get; set; }

        [BsonElement("Education")]
        [JsonPropertyName("Education")]
        public string Education { get; set; }

        [BsonElement("DOB")]
        [JsonPropertyName("DOB")]
        public string DateOfBirth { get; set; }

        [BsonElement("Current Location")]
        [JsonPropertyName("Current Location")]
        public string CurrentLocation { get; set; }

        [BsonElement("Prefered Location")]
        [JsonPropertyName("Prefered Location")]
        public string PreferedLocation { get; set; }

        [BsonElement("Exp Years")]
        [JsonPropertyName("Exp Years")]
        public Int32 ExpInYears { get; set; }

        [BsonElement("Exp Months")]
        [JsonPropertyName("Exp Months")]
        public Int32 ExpInMonths { get; set; }

        [BsonElement("Official Notice Period")]
        [JsonPropertyName("Official Notice Period")]
        public int NoticePeriod { get; set; }

        [BsonElement("Remaining Days")]
        [JsonPropertyName("Remaining Days")]
        public int RemainingDays { get; set; }

        [BsonElement("Currently Serving Notice")]
        [JsonPropertyName("Currently Serving Notice")]
        public string ServingNoticePeriod { get; set; }

        [BsonElement("Current Company")]
        [JsonPropertyName("Current Company")]
        public string CurrentCompany { get; set; }

        [BsonElement("Technical Skills")]
        [JsonPropertyName("Technical Skills")]
        public string Skills { get; set; }
    }
}

