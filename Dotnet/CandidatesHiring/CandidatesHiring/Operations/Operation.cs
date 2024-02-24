using CandidatesHiring.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;

namespace CandidatesHiring.Operations
{
    public class Operation : IOperation
    {
        private readonly IMongoCollection<Profile> profilescollection;
        public List<Profile> sortedProfiles {  get; set; }
      
        public Operation(IOptions<DatabaseSettings> db)
        {
            profilescollection = new MongoClient(db.Value.ConnectionString)
                .GetDatabase(db.Value.Database)
                .GetCollection<Profile>(db.Value.Collection);

        }
        public async Task<IEnumerable<Profile>> GetProfiles() =>
              await profilescollection.Find(_ => true).Limit(5).ToListAsync();

        public async Task<IEnumerable<Profile>> GetProfilewithId(string id) =>

            (IEnumerable<Profile>)await profilescollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Profile>> GetfilteredprofileswithmatchingCriteria(object criterias)
        {
            List<Dictionary<string,object>> filteringCriterias = new List<Dictionary<string,object>>();
            dynamic values = JsonSerializer.Deserialize<Dictionary<string, object>>(criterias.ToString());
            /*foreach (var item in values)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>(item.Key, item.Value);

                filteringCriterias.Add(temp);
            }*/

            Criterias criterias1 = MapDictToModel(values);
            sortedProfiles = GetProfilesFilteredBasedonCriteriaProvided(criterias1);
            return sortedProfiles;
        }

        public List<Profile> GetProfilesFilteredBasedonCriteriaProvided(Criterias criterias)
        {
            var builder = Builders<Profile>.Filter;
            var query = builder.Empty;

            if (!string.IsNullOrEmpty(criterias.CurrentRole))
                query &= builder.Eq(x => x.Designation, criterias.CurrentRole);

            /*if (criterias.TechnicalSkills != null && criterias.TechnicalSkills.Any())
            {
                var technicalSkillsFilter = builder.In(x => x.Skills.Split(',').Select(skill => skill.Trim()), criterias.TechnicalSkills);
                query &= technicalSkillsFilter;
            }*/

            if (!string.IsNullOrEmpty(criterias.Education))
                query &= builder.Eq(x => x.Education, criterias.Education);

            if (!string.IsNullOrEmpty(criterias.CurrentLocation))
                query &= builder.Eq(x => x.CurrentLocation, criterias.CurrentLocation);

            if (!string.IsNullOrEmpty(criterias.PreferedLocation))
                query &= builder.Eq(x => x.PreferedLocation, criterias.PreferedLocation);

            if (!string.IsNullOrEmpty(criterias.NoticePeriod))
                query &= builder.Eq(x => x.NoticePeriod, int.Parse(criterias.NoticePeriod));

            if (!string.IsNullOrEmpty(criterias.RemainingDays))
                query &= builder.Eq(x => x.RemainingDays, int.Parse(criterias.RemainingDays));

            if (!string.IsNullOrEmpty(criterias.MinExpYears))
                query &= builder.Gte(x => x.ExpInYears, int.Parse(criterias.MinExpYears));

            if (!string.IsNullOrEmpty(criterias.MaxExpYears))
                query &= builder.Lte(x => x.ExpInYears, int.Parse(criterias.MaxExpYears));

            List<Profile> profilesWithoutSkill = profilescollection.Find(query).ToList();
            List<Profile> profiles = profilesWithoutSkill.FindAll(x =>
                (criterias.TechnicalSkills == null || !criterias.TechnicalSkills.Any() ||
                x.Skills.Split(',').Any(skill => criterias.TechnicalSkills.Contains(skill.Trim()))));

            foreach (var profile in profiles)
            {
                profile.Rating = 5.0;

                if (profile.OptionalSkills != null)
                {
                    var optionalSkills = profile.OptionalSkills.Split(',').Select(e => e.Trim()).ToList();
                    var matchedOpSkills = optionalSkills.Intersect(criterias.OptionalSkills.Select(e => e.Trim())).ToList();
                    var isNotNull = optionalSkills.Count - matchedOpSkills.Count;
                    if(isNotNull > 0)
                    {
                        profile.Rating += isNotNull * 0.15;
                    }
                    profile.Rating += matchedOpSkills.Count * 0.25;
                }
            }

            return profiles;
        }

        public Criterias MapDictToModel(Dictionary<string, object> dict)
        {
            Criterias criterias = new Criterias();
            if (dict.TryGetValue("Education", out var educationObject))
            {
                string educationValue = educationObject?.ToString();

                if (!string.IsNullOrEmpty(educationValue))
                {
                    criterias.Education = educationValue;
                }
            }
            if (dict.TryGetValue("Technical Skills", out var technicalSkillsObject) && technicalSkillsObject is JsonElement technicalSkillsElement && technicalSkillsElement.ValueKind == JsonValueKind.Array)
            {
                if (technicalSkillsElement.EnumerateArray().Any())
                {
                    try
                    {
                        criterias.TechnicalSkills = JsonSerializer.Deserialize<List<string>>(technicalSkillsElement.GetRawText());
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (dict.TryGetValue("Optional Skills", out var optionalSkillsObject) && optionalSkillsObject is JsonElement optionalSkillsElement && optionalSkillsElement.ValueKind == JsonValueKind.Array)
            {
                if (optionalSkillsElement.EnumerateArray().Any())
                {
                    try
                    {
                        criterias.OptionalSkills = JsonSerializer.Deserialize<List<string>>(optionalSkillsElement.GetRawText());
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (dict.TryGetValue("Current Location", out var currentLocationObject))
            {
                string currentLocationValue = currentLocationObject?.ToString();

                if (!string.IsNullOrEmpty(currentLocationValue))
                {
                    criterias.CurrentLocation = currentLocationValue;
                }
            }
            if (dict.TryGetValue("Prefered Location", out var preferedLocationObject))
            {
                string preferedLocationValue = preferedLocationObject?.ToString();

                if (!string.IsNullOrEmpty(preferedLocationValue))
                {
                    criterias.PreferedLocation = preferedLocationValue;
                }
            }
            if (dict.TryGetValue("Current Role", out var currentRoleObject))
            {
                string currentRoleValue = currentRoleObject?.ToString();

                if (!string.IsNullOrEmpty(currentRoleValue))
                {
                    criterias.CurrentRole = currentRoleValue;
                }
            }
            if (dict.TryGetValue("Notice Period", out var noticePeriodObject))
            {
                string noticePeriodValue = noticePeriodObject?.ToString();

                if (!string.IsNullOrEmpty(noticePeriodValue))
                {
                    criterias.NoticePeriod = noticePeriodValue;
                }
            }
            if (dict.TryGetValue("Remaining Days", out var remainingDaysObject))
            {
                string remainingDaysValue = remainingDaysObject?.ToString();

                if (!string.IsNullOrEmpty(remainingDaysValue))
                {
                    criterias.RemainingDays = remainingDaysValue;
                }
            }
            if (dict.TryGetValue("Willing to Travell", out var willingtoTravellObject))
            {
                string willingtoTravellValue = willingtoTravellObject?.ToString();

                if (!string.IsNullOrEmpty(willingtoTravellValue))
                {
                    criterias.WillingtoTravell = willingtoTravellValue;
                }
            }
            if (dict.TryGetValue("Is Certified", out var isCertifiedObject))
            {
                string isCertifiedValue = isCertifiedObject?.ToString();

                if (!string.IsNullOrEmpty(isCertifiedValue))
                {
                    criterias.IsCertified = bool.Parse(isCertifiedValue);
                }
            }
            if (dict.TryGetValue("Certified From", out var certifiedFromObject))
            {
                string certifiedFromValue = certifiedFromObject?.ToString();

                if (!string.IsNullOrEmpty(certifiedFromValue))
                {
                    criterias.CertifiedFrom = certifiedFromValue;
                }
            }

            if (dict.TryGetValue("Exp Years", out var expYearsObject))
            {
                string expYearsValue = expYearsObject.ToString();
                List<string> expYears = ExtractExp(expYearsValue);
                if (expYears.Count > 0)
                {
                    criterias.MinExpYears = expYears[0];
                }
                if (expYears.Count > 1)
                {
                    criterias.MaxExpYears = expYears[expYears.Count -1];
                }
            }
            return criterias;
        }
        public bool sendMail(string body, List<Profile> profiles)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("Semicolon.RecruitRevolution@outlook.com"));
            email.Subject = "Test Link";

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("Semicolon.RecruitRevolution@outlook.com", "Semicolon@2024");

            foreach (var profile in profiles)
            {
                email.To.Add(MailboxAddress.Parse(profile.Email));
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "Hi " + profile.Name +
                    "<br>" +
                    "Please complete the MCQ test provided in the link below for further processing of your candidature for the applied position with us.\r\n" +
                    "<br><br>" +
                    "Link:"
                };
                smtp.Send(email);
            }

            smtp.Disconnect(true);
            return true;
        }
        static List<string> ExtractExp(string expYears)
        {
            string pattern = @"(\d+)(?:\s?-\s?(\d+))?(\+)?";

            Match match = Regex.Match(expYears, pattern);

            List<string> expList = new List<string>();

            if (match.Success)
            {
                int start = int.Parse(match.Groups[1].Value);

                if (match.Groups[2].Success)
                {
                    int end = int.Parse(match.Groups[2].Value);

                    for (int i = start; i <= end; i++)
                    {
                        expList.Add(i.ToString());
                    }
                }
                else if (match.Groups[3].Success)
                {
                    expList.Add(start.ToString());
                }
                else
                {
                    expList.Add(start.ToString());
                    expList.Add(start.ToString());
                }
            }

            return expList;
        }
    }
}
