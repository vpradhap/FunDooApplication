using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunDooApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL noteBL;
        public NotesController(INotesBL noteBL)
        {
            this.noteBL = noteBL;
        }

        [HttpPost("Create")]
        public IActionResult CreateNote(NotesCreateModel notesCreateModel)
        {
            long userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userId").Value); 
            var result = noteBL.Create(notesCreateModel,userid);
            if(result != null)
            {
                return this.Ok(new { Success = true, message = "Notes Created Successfully", result = result });
            }
            else
            {
                return this.BadRequest(new { Success = false, message = "Notes Creation failed" });
            }
        }

        [HttpGet("AllNotes")]
        public IEnumerable<NotesEntity> GetAllNotes()
        {
            try
            {
                return noteBL.GetAllNotes();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("NotesByNoteId")]
        public IEnumerable<NotesEntity> GetNotesbyNoteid(long noteId)
        {
            try
            {
                return noteBL.GetNotesbyNoteid(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("Update")]
        public IActionResult Update(NotesCreateModel notesCreateModel, long noteId)
        {
            try
            {
                var result = noteBL.Update(notesCreateModel, noteId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Note Updated Successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Update note" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("Remove")]
        public IActionResult Delete(NotesIdModel notesIdModel)
        {
            try
            {
                if (noteBL.Delete(notesIdModel.NoteID))
                {
                    return this.Ok(new { Success = true, message = "Note Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to delete note" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("Archive")]
        public IActionResult Archive(NotesIdModel notesIdModel)
        {
            try
            {
                var result = noteBL.Archive(notesIdModel);
                if (result != null)
                {
                    return this.Ok(new { message = "Note Unarchived ", Response = result });
                }
                else
                {
                    return this.Ok(new { message = "Note Archived Successfully" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("Pin")]
        public IActionResult Pin(NotesIdModel notesIdModel)
        {
            try
            {
                var result = noteBL.Pin(notesIdModel.NoteID);
                if (result != null)
                {
                    return this.Ok(new { message = "Note unPinned ", Response = result });
                }
                else
                {
                    return this.Ok(new { message = "Note Pinned Successfully" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("Trash")]
        public IActionResult Trash(NotesIdModel notesIdModel)
        {
            try
            {
                var result = noteBL.Trash(notesIdModel.NoteID);
                if (result != null)
                {
                    return this.Ok(new { message = "Note Restored ", Response = result });
                }
                else
                {
                    return this.Ok(new { message = "Note is in trash" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("Image")]
        public IActionResult Image(long noteid, IFormFile image)
        {
            try
            {
                var result = noteBL.Image(noteid, image);
                if (result != null)
                {
                    return this.Ok(new { message = "Image Uploaded success", Response = result });
                }
                else
                {
                    return this.BadRequest(new { message = "Image upload failed" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
