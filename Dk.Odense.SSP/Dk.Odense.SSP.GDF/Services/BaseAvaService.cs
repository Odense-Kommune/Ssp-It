using System;
using System.Linq;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Gdf.Model.Interfaces;
using Dk.Odense.SSP.Gdf.Repository.Interface;
using Dk.Odense.SSP.Gdf.Services.Interface;

namespace Dk.Odense.SSP.Gdf.Services
{
    public class BaseAvaService<TValue> : IBaseAvaService<TValue> where TValue : class,IAva, new()
    {
        private readonly IGdfRepository<TValue> repository;

        public BaseAvaService(IGdfRepository<TValue> repository)
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

        public Concern MapConcern(TValue x)
        {
            return new Concern()
            {
                Id = x.ID,
                CrimeConcern = ReplaceChars(x.CrimeConcern),
                NotifyConcern = MapEnumAnswer(ReplaceChars(x.NotifyConcern)),
                ReportedToPolice = MapEnumAnswer(ReplaceChars(x.ReportedToPolice))
            };
        }

        public ReportedPerson MapReportedPerson(TValue x)
        {
            return new ReportedPerson()
            {
                Id = x.ID,
                ReportedAdress = ReplaceChars(x.ReportedAddress),
                ReportedName = ReplaceChars(x.ReportedName),
                ReportedCpr = ReplaceChars(x.ReportedCpr)
            };
        }

        public Assessment MapAssessment(TValue x)
        {
            return new Assessment()
            {
                Id = x.ID,
                DrugRelationship = MapEnumStatus(ReplaceChars(x.DrugRelationship)),
                DrugRelationshipElaborate = ReplaceChars(x.DrugRelationshipElaborate),
                FutureDreams = MapEnumStatus(ReplaceChars(x.FutureDreams)),
                FutureDreamsElaborate = ReplaceChars(x.FutureDreamsElaborate),
                GoodFriends = MapEnumStatus(ReplaceChars(x.GoodFriends)),
                GoodFriendsElaborate = ReplaceChars(x.GoodFriendsElaborate),
                PositiveSupport = MapEnumStatus(ReplaceChars(x.PositiveSupport)),
                PositiveSupportElaborate = ReplaceChars(x.PositiveSupportElaborate),
                Skills = MapEnumStatus(ReplaceChars(x.Skills)),
                SkillsElaborate = ReplaceChars(x.SkillsElaborate),
                SocialBehavior = MapEnumStatus(ReplaceChars(x.SocialBehavior)),
                SocialBehaviorElaborate = ReplaceChars(x.SocialBehaviorElaborate)
            };
        }

        public Reporter MapReporter(TValue x)
        {
            var mail = ReplaceChars(x.Email.Replace("\'{\"FirstMail\":\"", "").Replace("\"SecondMail\":\"", "")
                .Replace("\"}\'", "")).Split(',');
                
            return new Reporter()
            {
                Id = x.ID,
                Name = ReplaceChars(x.Name),
                Phonenumber = ReplaceChars(x.Phonenumber),
                Workplace = ReplaceChars(x.Workplace),
                Email = mail[0] ?? "",
                ImmediateLeader = ReplaceChars(x.ImmediateLeader),
                ImmediateLeaderEmail = mail[1] ?? "",
                ImmediateLeaderPhone = ReplaceChars(x.ImmediateLeaderPhone)
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
