using Dk.Odense.SSP.Domain.Model;

namespace Dk.Odense.SSP.Gdf.Services.Interface
{
    public interface IBaseRobusthedService<TValue>
    {
        ReportedPerson MapReportedPerson(TValue x);
        Assessment MapAssessment(TValue x);
        Reporter MapReporter(TValue x);
        Core.Enum.Answer MapEnumAnswer(string str);
        Core.Enum.Status MapEnumStatus(string str);
        string ReplaceChars(string str);
    }
}
