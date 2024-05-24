using BackendNotas.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendNotas.Models
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string NotesCollectionName { get; set; }

        public MongoDatabaseSettings()
        {
            ConnectionString = string.Empty; // o cualquier valor predeterminado que desees
            DatabaseName = string.Empty;
            NotesCollectionName = string.Empty;
        }
    }

    [BsonIgnoreExtraElements]
    public class Note
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime createdAt { get; set; }
        public string userId { get; set; } // Agrega este campo para asociar cada nota con un usuario
    }
}
