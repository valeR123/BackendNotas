using System.Collections.Generic;
using System.Threading.Tasks;
using BackendNotas.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BackendNotas.Services
{
   public interface IMongoDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string NotesCollectionName { get; set; }
    }

    public class NoteService
    {
     private readonly IMongoCollection<Note> _notes;

        public NoteService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _notes = database.GetCollection<Note>("notes");
        }

        public async Task<List<Note>> GetNotesAsync(string userId)// Agrega el parámetro userId
        {
            var projection = Builders<Note>.Projection
                .Include(n => n.id)
                .Include(n => n.title)
                .Include(n => n.content)
                .Include(n => n.createdAt);

            return await _notes.Find(note => note.userId == userId)
                               .Project<Note>(projection)
                               .ToListAsync();
        }

        public async Task CreateNoteAsync(Note note, string userId)
        {
            // Asigna el userId a la nota
            note.userId = userId;

            // Genera un ObjectId único para el id de la nota
            note.id = ObjectId.GenerateNewId().ToString();

            // Inserta la nota en la colección
            await _notes.InsertOneAsync(note);
        }

        public async Task DeleteNoteAsync(string id, string userId)
        {
            var note = await _notes.Find(note => note.id == id).FirstOrDefaultAsync();
            if (note.userId != userId)
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta nota.");
            }
            await _notes.DeleteOneAsync(note => note.id == id);
        }
    }
}
