using System.Collections.Generic;

namespace Dk.Odense.SSP.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerRepository loggerRepository;

        public LoggerService(ILoggerRepository loggerRepository)
        {
            this.loggerRepository = loggerRepository;
        }

        public void LogHangfire(string user, string method, List<string> reqParamList)
        {
            loggerRepository.LogListAsync(user, method, reqParamList);
        }

        public void LogHangfire(string user, string method, string reqParam)
        {
            loggerRepository.LogAsync(user, method, reqParam);
        }
    }
}
