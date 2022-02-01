using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bogus.DataSets;

namespace XebecPortal.UI.Services.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }

        private List<EmploymentHistory> EmploymentHistory { get; set; }
        private List<EducationHistory> EducationHistory { get; set; }
        private List<OnlinePrecedence> OnlinePrecedence { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "First name is too long.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Last name is too long.")]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public Gender Gender { get; set; }
        public string IdNumber { get; set; }
        
    }

    public class OnlinePrecedence
    {
        public int OnlinePrecedenceId { get; set; }
        
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public string SiteName { get; set;}
        public string Description { get; set; }
        public string Url { get; set; }
    }

    public class EducationHistory
    {
        public int EducationHistoryId { get; set; }
        
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        
        [Required]
        public string Institution { get; set; }
        [Required]
        public string Qualification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }

    public class EmploymentHistory
    {
        public int EmploymentHistoryId { get; set; }
        
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "First name is too long.")]
        public string Company { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }

    public enum Ethnicity
    {
        Coloured,
        Black,
        White,
        Indian,
        Asian,
        RatherNotSay
    }

    public enum Gender
    {
        Female,
        Male,
        RatherNotSay
    }
}