using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace AIHiringPortal.Model
{
    public class Profile
    {
        public Profile() { }
        
        public string? Id { get; set; }

        public string? Name { get; set; }

       
        public string? Email { get; set; }

     
        public int? ContactNo { get; set; }

        
        public string? Designation { get; set; }

        
        public string? Education { get; set; }

      
        public string? DateOfBirth { get; set; }

        public string? CurrentLocation { get; set; }

        public string? PreferedLocation { get; set; }

        
        public int? ExpInYears { get; set; }

        
        public int? ExpInMonths { get; set; }

       
        public int? NoticePeriod { get; set; }

        
        public int? RemainingDays { get; set; }

       
        public string? ServingNoticePeriod { get; set; }

       
        public string? CurrentCompany { get; set; }

       
        public string? MandatorySkills { get; set; }
        public string? OptionalSkills { get; set; }

        public string? Rating { get; set; }
    }
}
