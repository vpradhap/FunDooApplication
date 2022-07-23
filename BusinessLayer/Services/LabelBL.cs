using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBL:ILabelBL
    {
        private readonly ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public LabelEntity Create(long noteId, long userId, string labelname)
        {
            try
            {
                return labelRL.Create(noteId, userId, labelname);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Delete(long userId, string labelname)
        {
            try
            {
               // return labelRL != null && labelRL.Delete(userId, labelname);
                return labelRL.Delete(userId, labelname);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<LabelEntity> GetAllLabel()
        {
            try
            {
                return labelRL.GetAllLabel();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<LabelEntity> GetLabelByID(long noteId, long userId)
        {
            try
            {
                return labelRL.GetLabelByID(noteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<LabelEntity> UpdateName(long userId, string oldLabelName, string newlabelName)
        {
            try
            {
                return labelRL.UpdateName(userId, oldLabelName, newlabelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
