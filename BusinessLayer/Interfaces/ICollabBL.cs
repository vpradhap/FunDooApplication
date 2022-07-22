using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBL
    {
        public CollabEntity Create(long noteId, long userId, string email);
        public bool Delete(long collabId);
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteId);
    }
}
