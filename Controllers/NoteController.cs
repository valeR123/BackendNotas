using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using BackendNotas.Services;
using BackendNotas.Models;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;

namespace BackendNotas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _noteService;
        private readonly FirebaseAuth _firebaseAuth;

        public NoteController(NoteService noteService, FirebaseAuth firebaseAuth)
        {
            _noteService = noteService;
            _firebaseAuth = firebaseAuth;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetNotes([FromQuery] string idToken)
        {
            if (idToken == null)
            {
                return BadRequest("El parámetro idToken es obligatorio.");
            }

            // Verifica el nuevo token de Firebase del usuario
            string uid = await _firebaseAuth.VerifyIdTokenAsync(idToken);

            if (uid == null)
            {
                return Unauthorized("El token de Firebase es inválido.");
            }

            // Si el token es válido, obtiene las notas del usuario
            return await _noteService.GetNotesAsync(uid);
        }



        [HttpPost]
        public async Task<ActionResult<Note>> CreateNoteAsync(Note note, string idToken)
        {
            var userId = await _firebaseAuth.VerifyIdTokenAsync(idToken);

            if (userId == null)
            {
                return Unauthorized("El token de Firebase es inválido.");
            }

            note.userId = userId; // Asigna el userId a la nota
            note.createdAt =DateTime.UtcNow;
            await _noteService.CreateNoteAsync(note, userId); // Pasa el userId al método CreateNoteAsync
            return CreatedAtAction(nameof(GetNotes), new { _id = note.id }, note);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoteAsync(string id, [FromQuery] string idToken)
        {
            if (idToken == null)
            {
                return BadRequest("El parámetro idToken es obligatorio.");
            }

            // Verifica el nuevo token de Firebase del usuario
            string userId = await _firebaseAuth.VerifyIdTokenAsync(idToken);

            if (userId == null)
            {
                return Unauthorized("El token de Firebase es inválido.");
            }

            // Elimina la nota con el ID proporcionado si pertenece al usuario autenticado
            await _noteService.DeleteNoteAsync(id, userId);
            return NoContent();
        }

    }
}
