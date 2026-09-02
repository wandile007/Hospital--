using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;

namespace Final.Models
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public special Specialty { get; set; }

        public enum special
        {
            Radiologist,
            SportsMedicine,
            Gynecologist,
            Dentist,
            GeneralPractitioner,
            Pediatrician,
            Cardiologist,
            Neurologist,
            Dermatologist,
            OrthopedicSurgeon,
            Psychiatrist,
            Endocrinologist
        }

        public string OfficeLocation { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date joined is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Joined")]

        public DateTime DateJoined { get; set; } = DateTime.Now;


        public bool IsAvailable { get; set; } = true;

        public string IdentityUserId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}
