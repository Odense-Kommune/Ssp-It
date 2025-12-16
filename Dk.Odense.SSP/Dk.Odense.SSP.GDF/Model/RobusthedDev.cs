using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dk.Odense.SSP.Gdf.Model.Interfaces;

namespace Dk.Odense.SSP.Gdf.Model
{
    [Table("Robusthedsskema")]
    public class RobusthedDev : IRobusthed
    {
        public Guid ID { get; set; }
        // ------------------------------ Reported Person ------------------------------------------
        //Navn på barn eller ung
        [Column("CABD4DD4-D0F4-4F9E-AAA4-A314B4330C1A")]
        public string ReportedName { get; set; }
        //Barnets eller den unges CPR-nummer
        [Column("8D1B2704-7468-41D6-99D3-1589A93521F1")]
        public string ReportedCpr { get; set; }
        // ------------------------------ Assessment ------------------------------------------
        //Social adfærd
        [Column("584606B1-19D5-47E8-9E19-4C8AE6A2C3F7")]
        public string SocialBehavior { get; set; }
        //Uddyb venligst
        [Column("AF99BD78-3AB6-424C-9E64-F135B7FEA2E6")]
        public string SocialBehaviorElaborate { get; set; }
        //Positiv opbakning fra forældre/værge
        [Column("4C533F55-1CBD-4D8F-C83C-338D9BFC35DE")]
        public string PositiveSupport { get; set; }
        //Uddyb venligst
        [Column("AA079693-E82C-4BE0-FE1B-7D6606F6C1B7")]
        public string PositiveSupportElaborate { get; set; }
        //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder feks fremmøde og forhold til klassekammerater)
        [Column("BE9F1AD1-C428-4DEB-B9C8-C74DEF112F2E")]
        public string Skills { get; set; }
        //Uddyb venligst
        [Column("293AEB33-E8D4-4BCC-D51D-468C158FD4F9")]
        public string SkillsElaborate { get; set; }
        //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        [Column("DA1693F5-8F75-42CA-AEAF-D69180A2A346")]
        public string DrugRelationship { get; set; }
        //Uddyb venligst
        [Column("DB53D8EC-6C16-4C4E-C155-7325C42477A6")]
        public string DrugRelationshipElaborate { get; set; }
        //Gode, jævnaldrende venner
        [Column("52D2D75C-421D-4015-EC23-5EBB62873681")]
        public string GoodFriends { get; set; }
        //Uddyb venligst
        [Column("FFE14A59-DF16-4F4B-E742-65E1E4D4995D")]
        public string GoodFriendsElaborate { get; set; }
        //Drømme og/eller tanker om fremtiden
        [Column("B4A4ACD9-C861-493C-BB92-27DA8854DEEE")]
        public string FutureDreams { get; set; }
        //Uddyb venligst
        [Column("92B5310F-2E84-443B-92E0-8F1496397E73")]
        public string FutureDreamsElaborate { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Dit navn
        [Column("BC713774-1315-474F-FA48-E309565BBA42")]
        public string Name { get; set; }
        //Dit telefonnummer
        [Column("C07F8FA0-1037-40BC-DD9C-FC7BF16B1F28")]
        public string Phonenumber { get; set; }
        //Din mailadresse
        [Column("C05BA333-D0D9-4147-94C1-19FC08B804A9")]
        public string Email { get; set; }
        //Din arbejdsplads
        [Column("1B1FC779-BE08-4CA3-DDCE-116836A01492")]
        public string Workplace { get; set; }
        //Din nærmeste leder
        [Column("2ECE6971-CD06-4A6B-AB1F-3DD896F56756")]
        public string ImmediateLeader { get; set; }
        //Din nærmeste leders telefonnummer
        [Column("AC982566-77E5-4140-E0A8-221D9FEBF23A")]
        public string ImmediateLeaderPhone { get; set; }
        // ------------------------------ Robustness ------------------------------------------
        //Navn på modtager af tilbagemelding
        [Column("6F74DF9E-4C28-4804-E16D-737BAB81DEEB")]
        public string ReplyRecipientName { get; set; }
        //Mailadresse på modtager af tilbagemeldingen
        [Column("89CC867F-3623-42ED-9A45-F2F06DCEAC11")]
        public string ReplyRecipientMail { get; set; }


        //---------------------- Branch 2 in Robustness Scheme ------------------------------
        // ------------------------------ Assessment ------------------------------------------
        //Social adfærd
        [Column("5f8ee1f9-f7fe-4cec-b431-5ab231e49666")]
        public string SocialBehavior1 { get; set; }
        //Uddyb venligst
        [Column("8dba1a41-0a35-4b6d-ad39-1d29c31e5257")]
        public string SocialBehaviorElaborate1 { get; set; }
        //Positiv opbakning fra forældre/værge
        [Column("dc181ebc-e432-4021-b6dc-083e4b8bf971")]
        public string PositiveSupport1 { get; set; }
        //Uddyb venligst
        [Column("7f9798b8-1262-4e8c-b890-c64bdf5253e8")]
        public string PositiveSupportElaborate1 { get; set; }
        //Færdigheder og trivsel i forhold til skole, uddannelse eller arbejde (herunder feks fremmøde og forhold til klassekammerater)
        [Column("6f7849b1-7b8a-4301-90f3-9e22d29cf9fe")]
        public string Skills1 { get; set; }
        //Uddyb venligst
        [Column("7785005d-4949-46f1-b0ea-669b3f185e1f")]
        public string SkillsElaborate1 { get; set; }
        //Forhold til rusmidler (alkohol, hash, præstationsfremmende midler og hårdere stoffer)
        [Column("3b63bc79-073e-42e1-8000-161a2060e27f")]
        public string DrugRelationship1 { get; set; }
        //Uddyb venligst
        [Column("5c255d79-b578-4592-8a82-679f5c8d79a5")]
        public string DrugRelationshipElaborate1 { get; set; }
        //Gode, jævnaldrende venner
        [Column("64f6101c-69e2-41c6-9bb0-c69a13da1638")]
        public string GoodFriends1 { get; set; }
        //Uddyb venligst
        [Column("7832c384-86b9-4973-804d-af0ddae6268a")]
        public string GoodFriendsElaborate1 { get; set; }
        //Drømme og/eller tanker om fremtiden
        [Column("fceb49e1-bd4d-4a5c-92f5-db710f51ec4c")]
        public string FutureDreams1 { get; set; }
        //Uddyb venligst
        [Column("f5111ee8-bd9a-4b84-834e-8490a9b28e9c")]
        public string FutureDreamsElaborate1 { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Dit navn
        [Column("ad150d71-b48b-4d2f-ba92-f48eaf099bc9")]
        public string Name1 { get; set; }
        //Dit telefonnummer
        [Column("4509b6eb-d720-4509-83db-16761ef6ba11")]
        public string Phonenumber1 { get; set; }
        //Din mailadresse
        [Column("eb1ae191-299a-4c08-ce4c-9181249bb154")]
        public string Email1 { get; set; }
        //Din arbejdsplads
        [Column("fe6fcf72-2e4f-46c5-be3e-c82bb4b18e8a")]
        public string Workplace1 { get; set; }
        //Din nærmeste leder
        [Column("c671b2ca-1c9a-434b-ac77-111e307af67f")]
        public string ImmediateLeader1 { get; set; }
        //Din nærmeste leders telefonnummer
        [Column("7d182a78-2bcf-4f79-b0a3-6ef929d4d51f")]
        public string ImmediateLeaderPhone1 { get; set; }
        // ------------------------------ Robustness ------------------------------------------
        //Navn på modtager af tilbagemelding
        [Column("6a0944be-ba5e-485c-a568-100c93eb8bfa")]
        public string ReplyRecipientName1 { get; set; }
        //Mailadresse på modtager af tilbagemeldingen
        [Column("0b7946fe-3b6f-4f6e-93f7-df038ab8583f")]
        public string ReplyRecipientMail1 { get; set; }
    }
}
