using CandidatesHiring.Model;

namespace CandidatesHiring.Operations
{
    public interface IOperation
    {
        Task<IEnumerable<Profile>> GetProfiles();
        Task<IEnumerable<Profile>> GetProfilewithId(string id);

        Task<IEnumerable<Profile>> GetfilteredprofileswithmatchingCriteria(object criterias);
        bool sendMail(string body, List<Profile> profiles);
    }
}
