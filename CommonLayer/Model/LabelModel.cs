using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class LabelModel
    {
        public long userId { get; set; }
        public long noteId { get; set; }
        public string LabelName { get; set; }
    }
    public class LabelNoteIdModel
    {
        public long noteId { get; set; }
    }   
    public class LabelNameModel
    {
        public string LabelName { get; set; }
    }
    public class LabelRenameModel
    {
        public string LabelName { get; set; }
        public string NewLabelName { get; set; }
    }
    public class LabelNameIdModel
    {
        public long noteId { get; set; }
        public string LabelName { get; set; }
    }
}
