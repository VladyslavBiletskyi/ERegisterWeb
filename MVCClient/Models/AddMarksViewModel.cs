using System.Collections.Generic;

namespace MVCClient.Models
{
    public class AddMarksViewModel
    {
        public int LessonId { get; set; }
        public List<MarkViewModel> Marks { get; set; }
    }
}