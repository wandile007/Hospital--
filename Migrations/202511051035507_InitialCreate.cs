namespace Health__.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Notes = c.String(maxLength: 500),
                        CreatedOn = c.DateTime(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Specialty = c.Int(nullable: false),
                        OfficeLocation = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        DateJoined = c.DateTime(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        IdentityUserId = c.String(),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            CreateTable(
                "dbo.MedicalRecords",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        Diagnosis = c.String(nullable: false, maxLength: 500),
                        TreatmentPlan = c.String(nullable: false, maxLength: 1000),
                        PrescribedMedications = c.String(maxLength: 500),
                        LabTestsOrdered = c.String(maxLength: 500),
                        VisitDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Notes = c.String(maxLength: 1000),
                        IsFollowUpRequired = c.Boolean(nullable: false),
                        FollowUpDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        MedicalAidProvider = c.String(),
                        MedicalAidNumber = c.String(),
                        Allergies = c.String(),
                        ChronicConditions = c.String(),
                        EmergencyContactName = c.String(),
                        EmergencyContactPhone = c.String(),
                        Notes = c.String(),
                        IdentityUserId = c.String(),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.DoctorSchedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        ScheduleDate = c.DateTime(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        IsAvailable = c.Boolean(nullable: false),
                        IsRecurring = c.Boolean(nullable: false),
                        RecurrencePattern = c.String(),
                        Notes = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false, maxLength: 100),
                        ContactNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        TimeSlot = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        Notes = c.String(maxLength: 500),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.PatientRegistrationViewModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PatientAppointments",
                c => new
                    {
                        Patient_PatientId = c.Int(nullable: false),
                        Appointment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Patient_PatientId, t.Appointment_Id })
                .ForeignKey("dbo.Patients", t => t.Patient_PatientId, cascadeDelete: true)
                .ForeignKey("dbo.Appointments", t => t.Appointment_Id, cascadeDelete: true)
                .Index(t => t.Patient_PatientId)
                .Index(t => t.Appointment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bookings", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorSchedules", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.MedicalRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.PatientAppointments", "Appointment_Id", "dbo.Appointments");
            DropForeignKey("dbo.PatientAppointments", "Patient_PatientId", "dbo.Patients");
            DropForeignKey("dbo.MedicalRecords", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.PatientAppointments", new[] { "Appointment_Id" });
            DropIndex("dbo.PatientAppointments", new[] { "Patient_PatientId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Bookings", new[] { "DoctorId" });
            DropIndex("dbo.DoctorSchedules", new[] { "DoctorId" });
            DropIndex("dbo.MedicalRecords", new[] { "DoctorId" });
            DropIndex("dbo.MedicalRecords", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropTable("dbo.PatientAppointments");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PatientRegistrationViewModels");
            DropTable("dbo.Bookings");
            DropTable("dbo.DoctorSchedules");
            DropTable("dbo.Patients");
            DropTable("dbo.MedicalRecords");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
