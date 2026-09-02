using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Final.Models
{
    public class MedicalRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [Required]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        [StringLength(1000)]
        public string TreatmentPlan { get; set; }

        [StringLength(500)]
        public string PrescribedMedications { get; set; }

        [StringLength(500)]
        public string LabTestsOrdered { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(1000)]
        public string Notes { get; set; }

        public bool IsFollowUpRequired { get; set; } = false;

        [DataType(DataType.Date)]
        public DateTime? FollowUpDate { get; set; }
    }
}
