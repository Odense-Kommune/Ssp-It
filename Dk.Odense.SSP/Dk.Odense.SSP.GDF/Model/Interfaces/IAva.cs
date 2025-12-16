namespace Dk.Odense.SSP.Gdf.Model.Interfaces
{
    public interface IAva : IGdfEntity
    {
        // ------------------------------ Reported Person ------------------------------------------
        //Barnets eller den unges adresse
        string ReportedAddress { get; set; }
        // ------------------------------ Concern ------------------------------------------
        //Beskriv din kriminalitetsbekymring
        string CrimeConcern { get; set; }
        //Er bekymringen anmeldt til politiet
        string ReportedToPolice { get; set; }
        //Er der lavet en underretning på bekymringen
        string NotifyConcern { get; set; }
        // ------------------------------ Reporter ------------------------------------------
        //Din nærmeste leders mailadresse
        string ImmediateLeaderEmail { get; set; }
    }
}
