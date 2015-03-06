/* This class creates the database context class for SchoolContext. it must
 * be derived from  System.Data.Entity.DbContext. This code will specify
 * which entities are included in the data model. Entity Framework behavior
 * can also be customized.
*/
using ContosoUniversity.Models;
//Following using statement necessary to inherit from DbContext
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {
        //The name of the connection string is passed in to the constructor.
        // This needs to be manualy added to the Web.config file as well
        public SchoolContext() : base("SchoolContext")
        {

        }
        
        //Creates database sets of the models
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        /* NOTE: DbSet<Enrollment> and DbSet<Course> statements could have been omitted
         * and it would of worked the same. The Entity Framework would include them implicitly 
         * because the Student entity references the Enrollment entity and the Enrollment 
         * entity references the Course entity
         */

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Removes the table naming convention that names database tables the pural of the entity type.
            //The table names for this application will be Student, Course, and Enrollment
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}