using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRL:ILabelRL
    {
        private readonly FundooContext fundooContext;
        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public LabelEntity Create(long noteId, long userId, string labelname)
        {
            try
            {
                LabelEntity Entity = new LabelEntity();
                Entity.LabelName = labelname;
                Entity.UserId = userId;
                Entity.NoteId = noteId;

                this.fundooContext.Label.Add(Entity);
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
        public bool Delete(long userId,string labelname)
        {
            try
            {
                var result = fundooContext.Label.FirstOrDefault(x => x.UserId==userId && x.LabelName==labelname);
                
                if (result != null)
                {
                    fundooContext.Remove(result);
                    fundooContext.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<LabelEntity> GetAllLabel()
        {
            return fundooContext.Label.ToList();
        }

        public IEnumerable<LabelEntity> GetLabelByID(long noteId, long userId)
        {
            return fundooContext.Label.Where(x => x.NoteId == noteId && x.UserId==userId).ToList();
        }

        public IEnumerable<LabelEntity> UpdateName(long userId, string oldLabelName, string newlabelName)
        {
            IEnumerable<LabelEntity> labels;
            labels = fundooContext.Label.Where(x => x.UserId == userId && x.LabelName == oldLabelName).ToList();
            if (labels != null)
            {
                foreach (var newlabel in labels)
                {
                    newlabel.LabelName = newlabelName;
                }
                fundooContext.SaveChanges();
                return labels;
            }
            else { return null; }
        }
    }
}
