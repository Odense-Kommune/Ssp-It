using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dk.Odense.SSP.Gdf.Model.Interfaces;

namespace Dk.Odense.SSP.Gdf.Model
{
    [Table("SSPOdense")]
    public class AvaDev : IAva
    {
        public Guid ID { get; set; }
        // ------------------------------ Reported Person ------------------------------------------
        [Column("2fbcac84-a538-41ef-b266-455f2da35594")] //Navn på barn eller ung
        public string ReportedName { get; set; }
        [Column("5199661f-d418-4353-8751-82c6b417ca0a")] //Barnets eller den unges CPR-nummer
        public string ReportedCpr { get; set; }
        [Column("f105d33b-db92-4f93-85f0-65fad2fd16df")] //Barnets eller den unges adresse
        public string ReportedAddress { get; set; }
        // ------------------------------ Concern ------------------------------------------
        [Column("39f546eb-8318-4c01-c10e-2275ef3f970a")] //Beskriv din kriminalitetsbekymring
        public string CrimeConcern { get; set; }
        [Column("c0978157-0534-41d6-a98e-1354c3f1ab2f")] //Er bekymringen anmeldt til politiet?
        public string ReportedToPolice { get; set; }
        [Column("f4847b5d-9542-458d-9559-4eba6df88bee")] //Er der lavet en underretning på bekymringen?
        public string NotifyConcern { get; set; }
        // ------------------------------ Assessment ------------------------------------------
        [Column("60fe2351-d0e6-43a1-99d0-aa530990ef37")] //Social adfærd
        public string SocialBehavior { get; set; }
        [Column("ba126345-6af6-4554-cc84-4392710af715")] //Uddyb venligst
        public string SocialBehaviorElaborate { get; set; }
        [Column("defd1b8a-56b9-41d7-ba28-ac8ba4142abb")] //Positiv opbakning fra forældre/værge
        public string PositiveSupport { get; set; }
        [Column("c6fa601d-ddbb-4982-a8af-1d80eb06eec5")] //Uddyb venligst
        public string PositiveSupportElaborate { get; set; }
        [Column("07df48c6-a912-4662-e04b-52d4defa8b77")] //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder f.eks. fremmøde og forhold til klassekammerater)
        public string Skills { get; set; }
        [Column("b41953e6-ca3d-4ebf-8642-f0aca8081563")] //Uddyb venligst
        public string SkillsElaborate { get; set; }
        [Column("7faa0645-5f8a-4245-9336-38a22ac35b7f")] //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        public string DrugRelationship { get; set; }
        [Column("63745661-a448-4c1e-c0bd-e3f71854047d")] //Uddyb venligst
        public string DrugRelationshipElaborate { get; set; }
        [Column("0b5d26a6-3774-4b17-8017-034713e1c3b0")] //Gode, jævnaldrende venner
        public string GoodFriends { get; set; }
        [Column("b1980335-4b0a-448d-e701-966da2baffab")] //Uddyb venligst
        public string GoodFriendsElaborate { get; set; }
        [Column("bba6ded9-1402-41fc-88b8-5320e37c5881")] //Drømme og/eller tanker om fremtiden
        public string FutureDreams { get; set; }
        [Column("66964195-91fa-42e7-c8bb-206ea59315ab")] //Uddyb venligst
        public string FutureDreamsElaborate { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        [Column("5cdcf1ae-2d5b-489c-8c55-12c8d90284d9")] //Dit navn
        public string Name { get; set; }
        [Column("efea0595-6aff-43df-d1fe-e155e9bcfbae")] //Dit telefonnummer
        public string Phonenumber { get; set; }
        [Column("ae42fec3-f433-422e-beef-905197fa51de")] //Din mailadresse
        public string Email { get; set; }
        [Column("c05bb080-9bf9-4118-fea5-b7d572a9ec17")] //Din arbejdsplads
        public string Workplace { get; set; }
        [Column("2e160c2e-6fed-4258-b946-76e866984482")] //Din nærmeste leder
        public string ImmediateLeader { get; set; }
        [Column("43a9c042-ae8c-4b8b-fc03-0622e9a48a7f")] //Din nærmeste leders mailadresse
        public string ImmediateLeaderEmail { get; set; }
        [Column("691d6cd8-8a99-4037-fe43-841e4b83bc27")] //Din nærmeste leders telefonnummer
        public string ImmediateLeaderPhone { get; set; }
    }
}
