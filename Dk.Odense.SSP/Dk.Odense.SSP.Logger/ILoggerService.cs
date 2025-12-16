using System.Collections.Generic;

namespace Dk.Odense.SSP.Logger
{
    public interface ILoggerService
    {
        void LogHangfire(string user, string method, List<string> reqParamList);
        void LogHangfire(string user, string method, string reqParam);
    }
}
