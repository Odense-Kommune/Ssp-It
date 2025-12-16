using System.Collections.Generic;

namespace Dk.Odense.SSP.Logger
{
    public interface ILoggerRepository
    {
        //void Log(string user, string method, string reqParams);
        void LogAsync(string user, string method, string reqParams);
        void LogListAsync(string user, string method, List<string> reqParamList);
    }
}
