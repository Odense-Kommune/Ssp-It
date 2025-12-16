namespace Dk.Odense.SSP.Gdf.Model.Interfaces
{
    public interface IRobusthed : IGdfEntity
    {
        //Navn på modtager af tilbagemelding
        string ReplyRecipientName { get; set; }
        //Mailadresse på modtager af tilbagemeldingen
        string ReplyRecipientMail { get; set; }

        //---------------------- Branch 2 in Robustness Scheme ------------------------------
        // ------------------------------ Assessment ------------------------------------------
        //Social adfærd
        string SocialBehavior1 { get; set; }
        //Uddyb venligst
        string SocialBehaviorElaborate1 { get; set; }
        //Positiv opbakning fra forældre/værge
        string PositiveSupport1 { get; set; }
        //Uddyb venligst
        string PositiveSupportElaborate1 { get; set; }
        //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder feks fremmøde og forhold til klassekammerater)
        string Skills1 { get; set; }
        //Uddyb venligst
        string SkillsElaborate1 { get; set; }
        //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        string DrugRelationship1 { get; set; }
        //Uddyb venligst
        string DrugRelationshipElaborate1 { get; set; }
        //Gode, jævnaldrende venner
        string GoodFriends1 { get; set; }
        //Uddyb venligst
        string GoodFriendsElaborate1 { get; set; }
        //Drømme og/eller tanker om fremtiden
        string FutureDreams1 { get; set; }
        //Uddyb venligst
        string FutureDreamsElaborate1 { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Dit navn
        string Name1 { get; set; }
        //Dit telefonnummer
        string Phonenumber1 { get; set; }
        //Din mailadresse
        string Email1 { get; set; }
        //Din arbejdsplads
        string Workplace1 { get; set; }
        //Din nærmeste leder
        string ImmediateLeader1 { get; set; }
        //Din nærmeste leders telefonnummer
        string ImmediateLeaderPhone1 { get; set; }
        // ------------------------------ Robustness ------------------------------------------
        //Navn på modtager af tilbagemelding
        string ReplyRecipientName1 { get; set; }
        //Mailadresse på modtager af tilbagemeldingen
        string ReplyRecipientMail1 { get; set; }
    }
}
