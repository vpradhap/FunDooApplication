using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBL:INotesBL
    {
        private readonly INotesRL notesRL;

        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }

        public NotesEntity Create(NotesCreateModel notesCreateModel, long userid)
        {
            try
            {
                return notesRL.Create(notesCreateModel,userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<NotesEntity> GetAllNotes()
        {
            try
            {
                return notesRL.GetAllNotes();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<NotesEntity> GetNotesbyNoteid(long noteId)
        {
            try
            {
                return notesRL.GetNotesbyNoteid(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity Update(NotesCreateModel notes, long noteId)
        {
            try
            {
                return notesRL.Update(notes, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Delete(long noteId)
        {
            try
            {
                return notesRL.Delete(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity Archive(NotesIdModel notesIdModel)
        {
            try
            {
                return notesRL.Archive(notesIdModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity Pin(long noteId)
        {
            try
            {
                return notesRL.Pin(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity Trash(long noteId)
        {
            try
            {
                return notesRL.Trash(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NotesEntity Image(long noteid, IFormFile image)
        {
            try
            {
                return notesRL.Image(noteid, image);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity Color(long noteid, string color)
        {
            try
            {
                return notesRL.Color(noteid,color);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
