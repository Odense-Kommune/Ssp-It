using System.Threading;
using System.Threading.Tasks;
using Dk.Odense.SSP.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dk.Odense.SSP.Infrastructure.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<Agenda> Agendas { get; set; }
        DbSet<AgendaItem> AgendaItems { get; set; }
        DbSet<Assessment> Assessments { get; set; }
        DbSet<Categorization> Categorizations { get; set; }
        DbSet<Concern> Concerns { get; set; }
        DbSet<Grouping> Groupings { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<PersonGrouping> PersonGroupings { get; set; }
        DbSet<ReportedPerson> ReportedPersons { get; set; }
        DbSet<Reporter> Reporters { get; set; }
        DbSet<Robustness> Robustnesses { get; set; }
        DbSet<Source> Sources { get; set; }
        DbSet<Worry> Worries { get; set; }
        DbSet<PoliceWorryCategory> PoliceWorryCategories { get; set; }
        DbSet<PoliceWorryRole> PoliceWorryRoles { get; set; }
        DbSet<Note> Notes{ get; set; }
        DbSet<NoteShared> NoteShareds { get; set; }
        DbSet<NoteAdditional> AdditionalNotes { get; set; }
        DbSet<Classification> Classifications { get; set; }

        //Metods
        void Dispose();
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry Entry(object entity);
        string ToString();
    }
}
