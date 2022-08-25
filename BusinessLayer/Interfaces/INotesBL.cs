using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INotesBL
    {
        public NotesEntity Create(NotesCreateModel notesCreateModel, long userid);
        public IEnumerable<NotesEntity> GetAllNotes();
        public IEnumerable<NotesEntity> GetNotesbyNoteid(long noteId);
        public NotesEntity Update(NotesCreateModel notes, long noteId);
        public bool Delete(long noteId);
        public NotesEntity Archive(NotesIdModel notesIdModel);
        public NotesEntity Pin(long noteId);
        public NotesEntity Trash(long noteId);
        public NotesEntity Image(long noteid, IFormFile image);
        public NotesEntity Color(long noteid, string color);
    }
}
