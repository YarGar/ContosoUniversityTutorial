using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A,B,C,D,F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        //Foreign key
        public int CourseID { get; set; }
        //Foriegn key
        public int StudentID { get; set; }
        //"?" indicates the grad property is nullable
        public Grade? Grade { get; set; }

        //Navigation properties:
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}