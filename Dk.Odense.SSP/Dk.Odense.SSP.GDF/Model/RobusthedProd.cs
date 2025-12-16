using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dk.Odense.SSP.Gdf.Model.Interfaces;

namespace Dk.Odense.SSP.Gdf.Model
{
    [Table("Robusthedsskema")]
    public class RobusthedProd : IRobusthed
    {
        public Guid ID { get; set; }
        // ------------------------------ Reported Person ------------------------------------------
        //Navn på barn eller ung
        [Column("cabd4dd4-d0f4-4f9e-aaa4-a314b4330c1a")]
        public string ReportedName { get; set; }
        //Barnets eller den unges CPR-nummer
        [Column("8d1b2704-7468-41d6-99d3-1589a93521f1")]
        public string ReportedCpr { get; set; }
        // ------------------------------ Assessment ------------------------------------------
        //Social adfærd
        [Column("5f8ee1f9-f7fe-4cec-b431-5ab231e49666")]
        public string SocialBehavior { get; set; }
        //Uddyb venligst
        [Column("8dba1a41-0a35-4b6d-ad39-1d29c31e5257")]
        public string SocialBehaviorElaborate { get; set; }
        //Positiv opbakning fra forældre/værge
        [Column("dc181ebc-e432-4021-b6dc-083e4b8bf971")]
        public string PositiveSupport { get; set; }
        //Uddyb venligst
        [Column("7f9798b8-1262-4e8c-b890-c64bdf5253e8")]
        public string PositiveSupportElaborate { get; set; }
        //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder feks fremmøde og forhold til klassekammerater)
        [Column("6f7849b1-7b8a-4301-90f3-9e22d29cf9fe")]
        public string Skills { get; set; }
        //Uddyb venligst
        [Column("7785005d-4949-46f1-b0ea-669b3f185e1f")]
        public string SkillsElaborate { get; set; }
        //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        [Column("3b63bc79-073e-42e1-8000-161a2060e27f")]
        public string DrugRelationship { get; set; }
        //Uddyb venligst
        [Column("5c255d79-b578-4592-8a82-679f5c8d79a5")]
        public string DrugRelationshipElaborate { get; set; }
        //Gode, jævnaldrende venner
        [Column("64f6101c-69e2-41c6-9bb0-c69a13da1638")]
        public string GoodFriends { get; set; }
        //Uddyb venligst
        [Column("7832c384-86b9-4973-804d-af0ddae6268a")]
        public string GoodFriendsElaborate { get; set; }
        //Drømme og/eller tanker om fremtiden
        [Column("fceb49e1-bd4d-4a5c-92f5-db710f51ec4c")]
        public string FutureDreams { get; set; }
        //Uddyb venligst
        [Column("f5111ee8-bd9a-4b84-834e-8490a9b28e9c")]
        public string FutureDreamsElaborate { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Dit navn
        [Column("ad150d71-b48b-4d2f-ba92-f48eaf099bc9")]
        public string Name { get; set; }
        //Dit telefonnummer
        [Column("4509b6eb-d720-4509-83db-16761ef6ba11")]
        public string Phonenumber { get; set; }
        //Din mailadresse
        [Column("eb1ae191-299a-4c08-ce4c-9181249bb154")]
        public string Email { get; set; }
        //Din arbejdsplads
        [Column("fe6fcf72-2e4f-46c5-be3e-c82bb4b18e8a")]
        public string Workplace { get; set; }
        //Din nærmeste leder
        [Column("c671b2ca-1c9a-434b-ac77-111e307af67f")]
        public string ImmediateLeader { get; set; }
        //Din nærmeste leders telefonnummer
        [Column("7d182a78-2bcf-4f79-b0a3-6ef929d4d51f")]
        public string ImmediateLeaderPhone { get; set; }
        // ------------------------------ Robustness ------------------------------------------
        //Navn på modtager af tilbagemelding
        [Column("6a0944be-ba5e-485c-a568-100c93eb8bfa")]
        public string ReplyRecipientName { get; set; }
        //Mailadresse på modtager af tilbagemeldingen
        [Column("0b7946fe-3b6f-4f6e-93f7-df038ab8583f")]
        public string ReplyRecipientMail { get; set; }

        //---------------------- Branch 2 in Robustness Scheme ------------------------------
        // ------------------------------ Assessment ------------------------------------------
        //Social adfærd
        [Column("ae5f8358-7d44-40f1-916a-2077c87faeb3")]
        public string SocialBehavior1 { get; set; }
        //Uddyb venligst
        [Column("213db3fd-5a2e-4c85-9cfc-7a6c4c10e887")]
        public string SocialBehaviorElaborate1 { get; set; }
        //Positiv opbakning fra forældre/værge
        [Column("8527e4bc-2c77-4eac-878f-51e51bc26446")]
        public string PositiveSupport1 { get; set; }
        //Uddyb venligst
        [Column("ddb43f13-f16f-4688-9727-f9af40fd0e0f")]
        public string PositiveSupportElaborate1 { get; set; }
        //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder feks fremmøde og forhold til klassekammerater)
        [Column("8959b743-61c4-46cd-e31d-990c7f5b0856")]
        public string Skills1 { get; set; }
        //Uddyb venligst
        [Column("7a8fc301-4bef-462c-c41a-9039dfa20b91")]
        public string SkillsElaborate1 { get; set; }
        //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        [Column("5cd35844-1afb-418a-8707-df454929562a")]
        public string DrugRelationship1 { get; set; }
        //Uddyb venligst
        [Column("9383511e-8c0e-4989-9af1-6ab0421c9044")]
        public string DrugRelationshipElaborate1 { get; set; }
        //Gode, jævnaldrende venner
        [Column("e853d4a5-cba1-488f-d061-4dbf48f08888")]
        public string GoodFriends1 { get; set; }
        //Uddyb venligst
        [Column("dd074458-6cad-449b-ac92-567b3047243b")]
        public string GoodFriendsElaborate1 { get; set; }
        //Drømme og/eller tanker om fremtiden
        [Column("336691ff-91f4-40ed-eac3-bc4d73c3c952")]
        public string FutureDreams1 { get; set; }
        //Uddyb venligst
        [Column("45272680-90d7-4c3b-f22f-6f253fa4eda5")]
        public string FutureDreamsElaborate1 { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Dit navn
        [Column("99407791-e68b-45c8-aac8-889aeaa5b515")]
        public string Name1 { get; set; }
        //Dit telefonnummer
        [Column("ff8bf694-877d-45cd-9ed4-4df1ac163fd6")]
        public string Phonenumber1 { get; set; }
        //Din mailadresse
        [Column("297081d9-eda1-4c7b-fe53-33fec1923215")]
        public string Email1 { get; set; }
        //Din arbejdsplads
        [Column("a0290fc1-b8cb-4398-ea5f-49cf57024089")]
        public string Workplace1 { get; set; }
        //Din nærmeste leder
        [Column("d1a0babc-a3c6-4ddf-b9b1-ee5a16709585")]
        public string ImmediateLeader1 { get; set; }
        //Din nærmeste leders telefonnummer
        [Column("d9739f64-cf26-4417-8e42-93e471f1481d")]
        public string ImmediateLeaderPhone1 { get; set; }
        // ------------------------------ Robustness ------------------------------------------
        //Navn på modtager af tilbagemelding
        [Column("3b8fef9a-2499-408e-d9aa-752662c9cc43")]
        public string ReplyRecipientName1 { get; set; }
        //Mailadresse på modtager af tilbagemeldingen
        [Column("2599c64e-c5df-44d3-f4e9-6b6dc3ec63d7")]
        public string ReplyRecipientMail1 { get; set; }
    }
}
