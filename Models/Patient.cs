using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gende Gender { get; set; }

        public enum Gende
        {
            Male,
            Female
        }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Residential Address")]
        public string Address { get; set; }

        [Display(Name = "Medical Aid Provider ")]
        public string MedicalAidProvider { get; set; }

        [Display(Name = "Medical Aid Number ")]
        public string MedicalAidNumber { get; set; }
        public string Allergies { get; set; }

        [Display(Name = "Chronic Conditions")]
        public string ChronicConditions { get; set; }

        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContactName { get; set; }

        [Phone]
        [Display(Name = "Emergency Contact Phone")]
        public string EmergencyContactPhone { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public string IdentityUserId { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {0} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
