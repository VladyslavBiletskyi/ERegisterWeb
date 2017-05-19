using System;

namespace MVCClient.Models
{
    public class AddLessonViewModel
    {
        public int SubjectId { get; set; }

        public int GroupId { get; set; }

        public DateTime BeginingDateTime { get; set; }

        public string Room { get; set; }

        public int ControllerId { get; set; }
    }
}