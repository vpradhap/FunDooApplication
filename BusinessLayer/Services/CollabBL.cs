using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollabBL:ICollabBL
    {
        private readonly ICollabRL collabRL;
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }

        public CollabEntity Create(long noteId, long userId, string email)
        {
            try
            {
                return collabRL.Create(noteId, userId, email);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Delete(long collabId)
        {
            try
            {
                return collabRL.Delete(collabId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteId)
        {
            try
            {
                return collabRL.GetAllByNoteID(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
