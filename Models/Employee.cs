﻿using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeNumber { get; set; }
        [Required]
        public string UserId { get; set; }
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
        public int LeaveHours { get; set; }
        [Required]
        public DateOnly StartOfEmployment { get; set; }
        public DateOnly? EndOfEmployment { get; set; }

        // Navigation properties
        public User User { get; set; }

        public List<Notification>? Notifications { get; set; }
        public List<SchoolSchedule>? SchoolSchedules { get; set; }
        public List<LeaveRequest>? LeaveRequests { get; set; }
        public List<SickLeave>? SickLeaves { get; set; }
        public List<Availability>? Availabilities { get; set; }
        public List<Shift>? Shifts { get; set; }
        public List<ShiftTakeOver>? ShiftTakeOvers { get; set; }
        public List<WorkedHour>? WorkedHours { get; set; }
        public List<StandardAvailability> StandardAvailability { get; set; }
    }
}
