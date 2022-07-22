using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollabRL:ICollabRL
    {
        private readonly FundooContext fundooContext;

        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public CollabEntity Create(long noteId, long userId, string email)
        {
            try
            {
                CollabEntity Entity = new CollabEntity();
                Entity.CollabEmail = email;
                Entity.UserId = userId;
                Entity.NoteId = noteId;

                this.fundooContext.CollabTable.Add(Entity);
                int result = this.fundooContext.SaveChanges();
                if (result > 0)
                {
                    return Entity;
                }
                else { return null; }
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
                var result = this.fundooContext.CollabTable.FirstOrDefault(x => x.CollabId == collabId);
                fundooContext.Remove(result);
                int delete = this.fundooContext.SaveChanges();
                if (delete > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteId)
        {
            return fundooContext.CollabTable.Where(n => n.NoteId == noteId).ToList();
        }
    }
}
