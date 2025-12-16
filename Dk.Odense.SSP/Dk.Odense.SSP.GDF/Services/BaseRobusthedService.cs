using System;
using System.Linq;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Dk.Odense.SSP.Gdf.Repository.Interface;
using Dk.Odense.SSP.Gdf.Services.Interface;

namespace Dk.Odense.SSP.Gdf.Services
{
    public class BaseRobusthedService<TValue> : IBaseRobusthedService<TValue> where TValue : class, IRobusthed, new()
    {
        private readonly IGdfRepository<TValue> repository;

        public BaseRobusthedService(IGdfRepository<TValue> repository)
        {
            this.repository = repository;
        }


        public IQueryable<TValue> List()
        {
            return repository.List();
        }

        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        public ReportedPerson MapReportedPerson(TValue x)
        {
            return new ReportedPerson()
            {
                Id = x.ID,
                ReportedName = ReplaceChars(x.ReportedName),
                ReportedCpr = ReplaceChars(x.ReportedCpr)
            };
        }

        public Assessment MapAssessment(TValue x)
        {
            return new Assessment()
            {
                Id = x.ID,
                DrugRelationship = x.DrugRelationship != null ? MapEnumStatus(ReplaceChars(x.DrugRelationship)) : MapEnumStatus(ReplaceChars(x.DrugRelationship1)),
                DrugRelationshipElaborate = x.DrugRelationshipElaborate != null ? ReplaceChars(x.DrugRelationshipElaborate) : ReplaceChars(x.DrugRelationshipElaborate1),
                FutureDreams = x.FutureDreams != null ? MapEnumStatus(ReplaceChars(x.FutureDreams)) : MapEnumStatus(ReplaceChars(x.FutureDreams1)),
                FutureDreamsElaborate = x.FutureDreamsElaborate != null ? ReplaceChars(x.FutureDreamsElaborate) : ReplaceChars(x.FutureDreamsElaborate1),
                GoodFriends = x.GoodFriends != null ? MapEnumStatus(ReplaceChars(x.GoodFriends)) : MapEnumStatus(ReplaceChars(x.GoodFriends1)),
                GoodFriendsElaborate = x.GoodFriendsElaborate != null ? ReplaceChars(x.GoodFriendsElaborate) : ReplaceChars(x.GoodFriendsElaborate1),
                PositiveSupport = x.PositiveSupport != null ? MapEnumStatus(ReplaceChars(x.PositiveSupport)) : MapEnumStatus(ReplaceChars(x.PositiveSupport1)),
                PositiveSupportElaborate = x.PositiveSupportElaborate != null ? ReplaceChars(x.PositiveSupportElaborate) : ReplaceChars(x.PositiveSupportElaborate1),
                Skills = x.Skills != null ? MapEnumStatus(ReplaceChars(x.Skills)) : MapEnumStatus(ReplaceChars(x.Skills1)),
                SkillsElaborate = x.SkillsElaborate != null ? ReplaceChars(x.SkillsElaborate) : ReplaceChars(x.SkillsElaborate1),
                SocialBehavior = x.SocialBehavior != null ? MapEnumStatus(ReplaceChars(x.SocialBehavior)) : MapEnumStatus(ReplaceChars(x.SocialBehavior1)),
                SocialBehaviorElaborate = x.SocialBehaviorElaborate != null ? ReplaceChars(x.SocialBehaviorElaborate) : ReplaceChars(x.SocialBehaviorElaborate1)
            };
        }

        public Reporter MapReporter(TValue x)
        {
            var mail = x.Email != null ? ReplaceChars(x.Email.Replace("\'{\"FirstMail\":\"", "").Replace("\"SecondMail\":\"", "").Replace("\"}\'", "")).Split(',') 
                : ReplaceChars(x.Email1.Replace("\'{\"FirstMail\":\"", "").Replace("\"SecondMail\":\"", "").Replace("\"}\'", "")).Split(',');

            return new Reporter()
            {
                Id = x.ID,
                Name = x.Name != null ? ReplaceChars(x.Name) : ReplaceChars(x.Name1),
                Phonenumber = x.Phonenumber != null ? ReplaceChars(x.Phonenumber) : ReplaceChars(x.Phonenumber1),
                Workplace = x.Workplace != null ? ReplaceChars(x.Workplace) : ReplaceChars(x.Workplace1),
                Email = mail[0],
                ImmediateLeader = x.ImmediateLeader != null ? ReplaceChars(x.ImmediateLeader) : ReplaceChars(x.ImmediateLeader1),
                ImmediateLeaderPhone = x.ImmediateLeaderPhone != null ? ReplaceChars(x.ImmediateLeaderPhone) : ReplaceChars(x.ImmediateLeaderPhone1),
                ImmediateLeaderEmail = mail[1]
            };
        }

        public Core.Enum.Answer MapEnumAnswer(string str)
        {
            switch (str)
            {
                case "Ja":
                    return Core.Enum.Answer.Yes;
                case "Nej":
                    return Core.Enum.Answer.No;
                case "Ved ikke":
                    return Core.Enum.Answer.DontKnow;
                default:
                    return Core.Enum.Answer.DontKnow;
            }
        }

        public Core.Enum.Status MapEnumStatus(string str)
        {
            switch (str)
            {
                case "Rød":
                    return Core.Enum.Status.Red;
                case "Gul":
                    return Core.Enum.Status.Yellow;
                case "Grøn":
                    return Core.Enum.Status.Green;
                default:
                    return Core.Enum.Status.Yellow;
            }
        }

        public string ReplaceChars(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            str = str.Replace("\"", "");
            str = str.Replace("'", "");
            return str;
        }
    }
}
