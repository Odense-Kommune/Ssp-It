using System;
using Dk.Odense.SSP.Core.Configuration;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Enum = Dk.Odense.SSP.Core.Enum;

namespace Dk.Odense.SSP.Infrastructure
{
    public class SspDbContext : DbContext, IDatabaseContext
    {
        private readonly XFlowConfig _xFlowConfig;

        public SspDbContext(DbContextOptions<SspDbContext> options, IOptions<XFlowConfig> xFlowConfig) : base(options)
        {
            _xFlowConfig = xFlowConfig.Value;
        }

        public DbSet<AgendaItem> AgendaItems { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Categorization> Categorizations { get; set; }
        public DbSet<Concern> Concerns { get; set; }
        public DbSet<Grouping> Groupings { get; set; }
        public DbSet<PersonGrouping> PersonGroupings { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PoliceWorryCategory> PoliceWorryCategories { get; set; }
        public DbSet<PoliceWorryRole> PoliceWorryRoles { get; set; }
        public DbSet<ReportedPerson> ReportedPersons { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
        public DbSet<Robustness> Robustnesses { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Worry> Worries { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteShared> NoteShareds { get; set; }
        public DbSet<NoteAdditional> AdditionalNotes { get; set; }
        public DbSet<SspArea> SspAreas { get; set; }
        public DbSet<AreaRule> AreaRules { get; set; }
        public DbSet<InternalSchoolData> SchoolData { get; set; }
        public DbSet<Classification> Classifications { get; set; }
        public DbSet<XFlowRobustness> XFlowRobustnesses { get; set; }
        public DbSet<XFlowWorry> XFlowWorries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worry>().Property(x => x.Increment).UseIdentityColumn();
            modelBuilder.Entity<Robustness>().Property(x => x.Increment).UseIdentityColumn();
            modelBuilder.Entity<Worry>().Property(x => x.Increment).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Robustness>().Property(x => x.Increment).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            // TODO Skal den her slettes?
            modelBuilder.Entity<Worry>().HasOne(q => q.ReportedPerson).WithMany(t => t.Worries).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Worry>().Property(x => x.PendingAutoVerify).HasDefaultValue(true);
            modelBuilder.Entity<Worry>().HasOne(q => q.Person).WithMany(q => q.Worries).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Worry>().HasOne(q => q.Concern).WithOne(q => q.Worry).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Person>().HasMany(q => q.Robustnesses).WithOne(q => q.Person).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Person>().HasOne(q => q.SchoolData).WithOne(q => q.Person).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Person>().HasMany(q => q.Notes).WithOne(q => q.Person).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Person>().HasMany(q => q.PersonGroupings).WithOne(q => q.Person).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Source>().HasData(new
            {
                Id = Guid.Parse("54a3812c-39a1-4d9b-849f-0d032684c303"),
                Value = "XFlow"
            });

            base.OnModelCreating(modelBuilder);
            //Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {

            #region Agenda
            modelBuilder.Entity<Agenda>().HasData(
                new Agenda
                {
                    Id = Guid.Parse("AB8F1B19-12D4-437D-A3EB-5AD012369F3A"),
                    AgendaNumber = 1,
                    Date = new DateTime(2018, 07, 23, 11, 35, 00),
                    AgendaSent = true,
                    MeetingHeld = true
                });
            #endregion

            #region AgendaItem
            modelBuilder.Entity<AgendaItem>().HasData(
                new AgendaItem
                {
                    Id = Guid.Parse("B0DCBF27-2860-4B3D-B0B5-D7AEAC8A16BD"),
                    Agenda_Id = Guid.Parse("AB8F1B19-12D4-437D-A3EB-5AD012369F3A"),
                    Categorization_Id = Guid.Parse("DDB439ED-5C88-4AF6-A6EE-8B48486B33FC"),
                    //Person_Id = Guid.Parse("894765DD-E97E-44AC-9EF0-6FD1A586920F"),
                    //Note = "This is a note on the current Agenda Item",
                    ProcesseDate = new DateTime(2018, 08, 03, 13, 45, 00),
                    SortOrder = 1
                });
            #endregion

            #region Assessment
            modelBuilder.Entity<Assessment>().HasData(
                new Assessment
                {
                    Id = Guid.Parse("11230D03-B061-4650-AC10-92EE7019A13C"),
                    SocialBehavior = Enum.Status.Green,
                    Skills = Enum.Status.Red,
                    GoodFriends = Enum.Status.Yellow,
                    PositiveSupport = Enum.Status.Yellow,
                    DrugRelationship = Enum.Status.Green,
                    FutureDreams = Enum.Status.Green
                });
            #endregion

            #region Categorization
            modelBuilder.Entity<Categorization>().HasData(
                new Categorization
                {
                    Id = Guid.Parse("DDB439ED-5C88-4AF6-A6EE-8B48486B33FC"),
                    Value = "Category 1",
                    DaysToExpire = 323
                });
            #endregion

            #region Concern
            modelBuilder.Entity<Concern>().HasData(
                new Concern
                {
                    Id = Guid.Parse("CCD55403-A930-4301-A999-01289582E05F"),
                    CrimeConcern = "Lorem ipsum dolor sit amet consectetur adipisicing elit. " +
                                   "Ex reprehenderit asperiores ad dolore iure? Nihil illum alias sequi sunt ad, " +
                                   "illo magnam id, esse repellendus sed quam repellat, perspiciatis cupiditate.",
                    ReportedToPolice = Enum.Answer.No,
                    NotifyConcern = Enum.Answer.Yes
                });
            #endregion

            #region Grouping
            modelBuilder.Entity<Grouping>().HasData(
                new Grouping
                {
                    Id = Guid.Parse("9278443B-3073-402B-90DA-F115C531F256"),
                    Value = "Pomfrit Rockerne"
                });
            #endregion

            #region Person
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = Guid.Parse("894765DD-E97E-44AC-9EF0-6FD1A586920F")
                });
            #endregion

            #region PoliceWorryCategory
            modelBuilder.Entity<PoliceWorryCategory>().HasData(
                new PoliceWorryCategory
                {
                    Id = Guid.Parse("740644A7-E101-4A7B-8A60-22C64EA82264"),
                    Value = "Bananskræller"
                });
            #endregion

            #region PoliceWorryRole
            modelBuilder.Entity<PoliceWorryRole>().HasData(
                new PoliceWorryCategory
                {
                    Id = Guid.Parse("323BF742-AFA5-4673-904E-A1A6F4DBE925"),
                    Value = "Sigtet"
                });
            #endregion

            #region PersonGrouping
            modelBuilder.Entity<PersonGrouping>().HasData(
                new PersonGrouping
                {
                    Id = Guid.Parse("FC837495-F6A0-43BD-A1D9-3AF83D084C25"),
                    Person_Id = Guid.Parse("894765DD-E97E-44AC-9EF0-6FD1A586920F"),
                    Grouping_Id = Guid.Parse("9278443B-3073-402B-90DA-F115C531F256")
                });
            #endregion

            #region ReportedPerson
            modelBuilder.Entity<ReportedPerson>().HasData(
                new ReportedPerson
                {
                    Id = Guid.Parse("3010E0A7-74EC-4660-BEAB-EB35DF4AEE73"),
                    ReportedCpr = "0101010000",
                    ReportedName = "Conan Barberian",
                    ReportedAdress = "Det ved jeg ikke"
                });
            #endregion

            #region Reporter
            modelBuilder.Entity<Reporter>().HasData(
                new Reporter
                {
                    Id = Guid.Parse("F62605D9-91CC-4441-8921-579897872B91"),
                    Name = "Bjarke Holmgaard",
                    Phonenumber = "14 14 14 14",
                    Email = "bjarke@morsdyt.dk",
                    Workplace = "Fantasifabrikken",
                    ImmediateLeader = "Rut Hansen",
                    ImmediateLeaderPhone = "01 02 03 04",
                    ImmediateLeaderEmail = "gladerut@morsdyt.dk"
                });
            #endregion

            #region Robustness
            modelBuilder.Entity<Robustness>().HasData(
                new Robustness
                {
                    Id = Guid.Parse("1BEF9572-4812-45F1-A869-49B8B7E0511B"),
                    CreatedDate = new DateTime(2017, 09, 23, 13, 45, 29),
                    EnrollmentPlace = "Borgerservice",
                    Assessment_Id = Guid.Parse("11230D03-B061-4650-AC10-92EE7019A13C"),
                    Person_Id = Guid.Parse("894765DD-E97E-44AC-9EF0-6FD1A586920F"),
                    ReportedPerson_Id = Guid.Parse("3010E0A7-74EC-4660-BEAB-EB35DF4AEE73"),
                    Reporter_Id = Guid.Parse("F62605D9-91CC-4441-8921-579897872B91")
                });
            #endregion

            #region Source
            modelBuilder.Entity<Source>().HasData(
                new Source
                {
                    Id = Guid.Parse("A60050AD-0ED0-467D-A97F-DE3E10207326"),
                    Value = "Politi"
                },
                new Source
                {
                    Id = Guid.Parse("439AD34B-FE8F-4167-B8EB-19D81689C34B"),
                    Value = "Ava"
                },
                new Source
                {
                    Id = Guid.Parse(_xFlowConfig.WorrySourceId), 
                    Value = "XFlow"
                });
            #endregion

            #region Worry
            modelBuilder.Entity<Worry>().HasData(
                new Worry
                {
                    Id = Guid.Parse("59C80F50-8E16-4C4E-ACE0-4C3B6C52A67F"),
                    CrimeScene = "Outside",
                    CreatedDate = new DateTime(2017, 09, 23, 13, 45, 29),
                    Concern_Id = Guid.Parse("CCD55403-A930-4301-A999-01289582E05F"),
                    Person_Id = Guid.Parse("894765DD-E97E-44AC-9EF0-6FD1A586920F"),
                    ReportedPerson_Id = Guid.Parse("3010E0A7-74EC-4660-BEAB-EB35DF4AEE73"),
                    Reporter_Id = Guid.Parse("F62605D9-91CC-4441-8921-579897872B91"),
                    Assessment_Id = Guid.Parse("11230D03-B061-4650-AC10-92EE7019A13C"),
                    PoliceWorryCategory_Id = Guid.Parse("740644A7-E101-4A7B-8A60-22C64EA82264"),
                    PoliceWorryRole_Id = Guid.Parse("323BF742-AFA5-4673-904E-A1A6F4DBE925"),
                    Source_Id = Guid.Parse("439AD34B-FE8F-4167-B8EB-19D81689C34B"),
                    Increment = 1
                });
            #endregion
        }

        #region SQL For View
        /*
        SELECT        dbo.Worries.Id, dbo.Worries.CreatedDate, dbo.Concerns.ReportedToPolice, dbo.Concerns.NotifyConcern, dbo.PoliceWorryCategories.Value AS PoliceWorryCategory, dbo.PoliceWorryRoles.Value AS PoliceWorryRole, 
                         dbo.Sources.Value AS Sources, dbo.Reporters.Workplace, dbo.Agendas.Date AS AgendaDate, dbo.Agendas.MeetingHeld, dbo.SspAreas.Value AS SspArea, dbo.Categorizations.Value AS Categorization
        FROM            dbo.Worries LEFT OUTER JOIN
                         dbo.AgendaItems ON dbo.Worries.AgendaItem_Id = dbo.AgendaItems.Id LEFT OUTER JOIN
                         dbo.Agendas ON dbo.AgendaItems.Agenda_Id = dbo.Agendas.Id LEFT OUTER JOIN
                         dbo.Categorizations ON dbo.AgendaItems.Categorization_Id = dbo.Categorizations.Id LEFT OUTER JOIN
                         dbo.Persons ON dbo.Worries.Person_Id = dbo.Persons.Id LEFT OUTER JOIN
                         dbo.Concerns ON dbo.Worries.Concern_Id = dbo.Concerns.Id LEFT OUTER JOIN
                         dbo.PoliceWorryCategories ON dbo.Worries.PoliceWorryCategory_Id = dbo.PoliceWorryCategories.Id LEFT OUTER JOIN
                         dbo.PoliceWorryRoles ON dbo.Worries.PoliceWorryRole_Id = dbo.PoliceWorryRoles.Id LEFT OUTER JOIN
                         dbo.Sources ON dbo.Worries.Source_Id = dbo.Sources.Id LEFT OUTER JOIN
                         dbo.Reporters ON dbo.Worries.Reporter_Id = dbo.Reporters.Id LEFT OUTER JOIN
                         dbo.SspAreas ON dbo.Persons.SspArea_Id = dbo.SspAreas.Id
        WHERE        (dbo.Agendas.MeetingHeld = 1)
        */
        #endregion
    }
}
