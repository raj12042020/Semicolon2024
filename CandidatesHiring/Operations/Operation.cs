using CandidatesHiring.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace CandidatesHiring.Operations
{
    public class Operation : IOperation
    {
        private readonly IMongoCollection<Profile> profilescollection;
      
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
            List<Dictionary<string,string>> filteringCriterias = new List<Dictionary<string,string>>();
            dynamic values = JsonSerializer.Deserialize<Dictionary<string, string>>(criterias.ToString());
            foreach (var item in values)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>(item.Key, item.Value);

                filteringCriterias.Add(temp);
            }

            return await profilescollection.Find(_ => true).Limit(20).ToListAsync();
        }

        public void GetProfilesFilteredBasedonCriteriaProvided(Criterias criterias)
        {
            IMongoCollection<Profile> profiles = (IMongoCollection<Profile>)profilescollection.Find(x => x.Designation == criterias.CurrentRole).Limit(20).ToList();
            IMongoCollection<Profile> profiles1 = (IMongoCollection<Profile>)(profiles.Find(x => x.Designation == criterias.CurrentRole)).Limit(20).ToList();
            IMongoCollection<Profile> profiles2 = (IMongoCollection<Profile>)profiles.Find(x => x.Designation == criterias.CurrentRole).Limit(20).ToList();
            IMongoCollection<Profile> profiles3 = (IMongoCollection<Profile>)profiles.Find(x => x.Designation == criterias.CurrentRole).Limit(20).ToList();
            IMongoCollection<Profile> profiles4 = (IMongoCollection<Profile>)profiles.Find(x => x.Designation == criterias.CurrentRole).Limit(20).ToList();
        }
    }
}
