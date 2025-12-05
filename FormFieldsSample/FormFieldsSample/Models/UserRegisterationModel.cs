
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormFieldsSample.Models
{
    public class UserRegisterationModel
    {
        public string Name { get; set; } = string.Empty;
        public string EmailID { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public string Gender { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; }

        public SelectList GenderList { get; set; } = new SelectList(new[] { "Male", "Female", "Other" });
        public SelectList MaritalStatusList { get; set; } = new SelectList(new[] { "Single", "Married", "Other" });
        public SelectList OccupationList { get; set; } = new SelectList(new[] { "Doctor", "Teacher", "Software Engineer", "Student", "Entrepreneur", "Other" });
    }
}
