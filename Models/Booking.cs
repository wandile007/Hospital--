using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(100, ErrorMessage = "Patient name cannot exceed 100 characters.")]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "Time Slot")]
        public DateTime TimeSlot { get; set; }

        [Required(ErrorMessage = "Doctor selection is required.")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

     
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // Optional: Navigation property for Doctor (if using EF relationships)
        public Doctor Doctor { get; set; }

        public status Status { get; set; } = status.Pending;

        public enum status
        {
            Pending,
            Shortlisted,
            Cancelled,
            Rescheduled,
            Confirmed
        }
    }
}
