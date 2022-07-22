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
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        public CollabController(ICollabBL collabBL)
        {
            this.collabBL = collabBL;
        }

        [HttpPost("Create")]
        public IActionResult Create(CollabModel collabModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userId").Value);
                var result = collabBL.Create(collabModel.noteid, userid, collabModel.email);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Collaborator Added Successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to add" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(CollabIdModel collabId )
        {
            try
            {
                if (collabBL.Delete(collabId.collabid))
                {
                    return this.Ok(new { Success = true, message = "Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Delete" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("NoteId")]
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteId)
        {
            try
            {
                return collabBL.GetAllByNoteID(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
