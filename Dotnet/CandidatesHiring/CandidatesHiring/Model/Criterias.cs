namespace CandidatesHiring.Model
{
    public class Criterias
    {
        public string Education { get; set; }
        public string CurrentLocation { get; set; }
        public string PreferedLocation { get; set; }
        public string CurrentRole { get; set; }
        public List<string> TechnicalSkills { get; set; }
        public List<string> OptionalSkills { get; set; }
        public string NoticePeriod { get; set; }
        public string RemainingDays { get; set; }
        public string MinExpYears { get; set; }
        public string MaxExpYears { get; set; }
        public string WillingtoTravell { get; set;}
        public bool IsCertified { get; set; }
        public string CertifiedFrom { get; set; }
    }
}
