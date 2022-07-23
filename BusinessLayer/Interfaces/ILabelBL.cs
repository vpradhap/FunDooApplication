using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBL
    {
        public LabelEntity Create(long noteId, long userId, string labelname);
        public bool Delete(long userId, string labelname);
        public IEnumerable<LabelEntity> GetAllLabel();
        public IEnumerable<LabelEntity> GetLabelByID(long noteId, long userId);
        public IEnumerable<LabelEntity> UpdateName(long userId, string oldLabelName, string newlabelName);
    }
}
