using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course
    {
        //Allows you to manually generate the CourseID
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        //Navigation property showing a one(Course) to many(Enrollment) relationship
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}