using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AIHiringPortal.Database;
using AIHiringPortal.Model;
using System.Text.Json;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;

namespace AIHiringPortal.DatabaseService
{
    public class SearchService : ISearchService
    {
        private readonly IDatabaseSettings profilescollection;
        public string ConnectionString { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Serveradmin { get; set; } = string.Empty;
        public string AdminSecret { get; set; } = string.Empty;
        public SearchService(IOptions<DatabaseSettings> db)
        {
            ConnectionString = db.Value.ConnectionString;
            Database = db.Value.Database;
            Serveradmin = db.Value.Serveradmin;
            AdminSecret = db.Value.AdminSecret;
        }

        public SqlConnection GetDatabaseConnection()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = ConnectionString;
            connectionStringBuilder.UserID = Serveradmin;
            connectionStringBuilder.Password = AdminSecret;
            connectionStringBuilder.InitialCatalog = Database;
            return new SqlConnection(connectionStringBuilder.ConnectionString);
        }

        public async Task<IEnumerable<Profile>> GetfilteredprofileswithmatchingCriteria(object criterias)
        {
            List<Dictionary<string, object>> filteringCriterias = new List<Dictionary<string, object>>();
            dynamic values = JsonSerializer.Deserialize<Dictionary<string, object>>(criterias.ToString());

            Criterias criterias1 = MapDictToModel(values);
            var sortedProfiles = GetProfiles(criterias1);
            return sortedProfiles;
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
            //if (dict.TryGetValue("Optional Skills", out var optionalSkillsObject) && optionalSkillsObject is JsonElement optionalSkillsElement && optionalSkillsElement.ValueKind == JsonValueKind.Array)
            //{
            //    if (optionalSkillsElement.EnumerateArray().Any())
            //    {
            //        try
            //        {
            //            criterias.OptionalSkills = JsonSerializer.Deserialize<List<string>>(optionalSkillsElement.GetRawText());
            //        }
            //        catch (JsonException ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //}
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
                    criterias.MaxExpYears = expYears[expYears.Count - 1];
                }
            }
            return criterias;
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
        public IEnumerable<Profile> GetProfiles(Criterias criterias)
        {
            List<Profile> profileList = new List<Profile>();
            List<Profile> finalList = new List<Profile>();
            SqlConnection databaseConnection = GetDatabaseConnection();
            string statement = "Select * from dbo.Candidates where Education='" + criterias.Education +
                "' AND Prefered_Location='" + criterias.PreferedLocation +
                "' AND Exp_in_Years='" + criterias.MinExpYears +
                "' AND Official_notice_Period='" + criterias.NoticePeriod +"'";

            databaseConnection.Open();

            SqlCommand _sqlcommand = new SqlCommand(statement, databaseConnection);
           
            using (SqlDataReader dataReader = _sqlcommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Profile _product = new Profile()
                    {
                        Id = dataReader.GetString(0),
                        Name = dataReader.GetString(1),
                        Email = dataReader.GetString(2),
                        ContactNo = dataReader.GetInt32(3),
                        Education = dataReader.GetString(4),
                        DateOfBirth = dataReader.GetString(5),
                        CurrentLocation = dataReader.GetString(6),
                        PreferedLocation = dataReader.GetString(7),
                        Designation = dataReader.GetString(8),
                        MandatorySkills = dataReader.GetString(9),
                        OptionalSkills = dataReader.GetString(10),
                        ExpInYears = dataReader.GetInt32(11),
                        ExpInMonths = dataReader.GetInt32(12),
                        CurrentCompany = dataReader.GetString(13),
                        NoticePeriod = dataReader.GetInt32(14),
                        ServingNoticePeriod = dataReader.GetString(15),
                        RemainingDays = dataReader.GetInt32(16)
                    };
                    profileList.Add(_product);
                }
            }
            databaseConnection.Close();
            foreach(var profile in profileList)
            {
                var skills = profile.MandatorySkills.Split(',').Select(e => e.Trim()).ToList();
                var matchedOpSkills = skills.Intersect(criterias.TechnicalSkills.Select(e => e.Trim())).ToList();
                if(matchedOpSkills.Count > 0)
                {
                    finalList.Add(profile);
                }
            }
            return finalList;
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
                    "Link: " + body
                };
                smtp.Send(email);
            }

            smtp.Disconnect(true);
            return true;
        }
    }

    public interface ISearchService
    {
       IEnumerable<Profile> GetProfiles(Criterias criterias);
       Criterias MapDictToModel(Dictionary<string, object> dict);
       Task<IEnumerable<Profile>> GetfilteredprofileswithmatchingCriteria(object criterias);
       bool sendMail(string body, List<Profile> profiles);
    }
}
