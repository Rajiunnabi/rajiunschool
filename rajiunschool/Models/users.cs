namespace rajiunschool.Models
{
    public class users
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; } // Later, hash for security
        public string role { get; set; } // Values: "Admin", "Student", "Teacher", "Employee",
        public string? ProfilePicture { get; set; }
    }

}
