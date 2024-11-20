using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeNumber { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        [StringLength(6)]
        public string Zipcode { get; set; }
        [Required]
        [StringLength(10)]
        public string HouseNumber { get; set; }
        [Required, Range(0, 55)]
        public int ContractHours { get; set; }
        [Required]
        public DateOnly StartOfEmployment { get; set; }
        public DateOnly? EndOfEmployment { get; set; }

        // Navigation properties
        public List<Notification> notifications { get; set; }
        public List<SchoolSchedule> SchoolSchedules { get; set; }
        public List<LeaveRequest> leaveRequests { get; set; }
        public List<SickLeave> sickLeaves { get; set; }
        public List<Availability> Availabilities { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<ShiftTakeOver> shiftTakeOvers { get; set; }
    }
}
