using Dk.Odense.SSP.Core;
using Enum = Dk.Odense.SSP.Core.Enum;

namespace Dk.Odense.SSP.Domain.Model
{
    public class Assessment : Entity
    {
        public Enum.Status SocialBehavior { get; set; }
        public string SocialBehaviorElaborate { get; set; }
        public Enum.Status PositiveSupport { get; set; }
        public string PositiveSupportElaborate { get; set; }
        public Enum.Status Skills { get; set; }
        public string SkillsElaborate { get; set; }
        public Enum.Status DrugRelationship { get; set; }
        public string DrugRelationshipElaborate { get; set; }
        public Enum.Status GoodFriends { get; set; }
        public string GoodFriendsElaborate { get; set; }
        public Enum.Status FutureDreams { get; set; }
        public string FutureDreamsElaborate { get; set; }
    }
}
