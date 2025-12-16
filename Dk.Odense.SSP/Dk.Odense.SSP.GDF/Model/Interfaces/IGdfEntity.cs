using System;

namespace Dk.Odense.SSP.Gdf.Model.Interfaces
{
    public interface IGdfEntity
    {
        Guid ID { get; set; }
        // ------------------------------ Reported Person ------------------------------------------
        //Navn på barn eller ung
        string ReportedName { get; set; }
        //Barnets eller den unges CPR-nummer
        string ReportedCpr { get; set; }
        // ------------------------------ Assessment ------------------------------------------
        //Social adfærd
        string SocialBehavior { get; set; }
        //Uddyb venligst
        string SocialBehaviorElaborate { get; set; }
        //Positiv opbakning fra forældre/værge
        string PositiveSupport { get; set; }
        //Uddyb venligst
        string PositiveSupportElaborate { get; set; }
        //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder feks fremmøde og forhold til klassekammerater)
        string Skills { get; set; }
        //Uddyb venligst
        string SkillsElaborate { get; set; }
        //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        string DrugRelationship { get; set; }
        //Uddyb venligst
        string DrugRelationshipElaborate { get; set; }
        //Gode, jævnaldrende venner
        string GoodFriends { get; set; }
        //Uddyb venligst
        string GoodFriendsElaborate { get; set; }
        //Drømme og/eller tanker om fremtiden
        string FutureDreams { get; set; }
        //Uddyb venligst
        string FutureDreamsElaborate { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Dit navn
        string Name { get; set; }
        //Dit telefonnummer
        string Phonenumber { get; set; }
        //Din mailadresse
        string Email { get; set; }
        //Din arbejdsplads
        string Workplace { get; set; }
        //Din nærmeste leder
        string ImmediateLeader { get; set; }
        //Din nærmeste leders telefonnummer
        string ImmediateLeaderPhone { get; set; }
    }
}
