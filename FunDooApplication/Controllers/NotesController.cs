using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunDooApplication.Controllers
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL noteBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<NotesController> logger;

        public NotesController(INotesBL noteBL,IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
            this.noteBL = noteBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
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
            //try
            //{
                logger.LogInformation("Fetching all the notes from the database");
                var result = noteBL.GetAllNotes();
                //throw new Exception("Exception while fetching all the notes from the database");
                logger.LogInformation("Displaying "+ result.Count()+" Notes");
                return result;
            //}
            //catch (Exception e)
            //{
            //    logger.LogError($"Something went wrong : {e}");
            //    return Enumerable.Empty<NotesEntity>();
            //}
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

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string serializedNotesList;
            var NotesList = new List<NotesEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = (List<NotesEntity>)noteBL.GetAllNotes();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }
    }
}
