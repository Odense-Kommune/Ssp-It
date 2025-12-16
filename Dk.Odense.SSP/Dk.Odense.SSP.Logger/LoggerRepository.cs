using System;
using System.Collections.Generic;

namespace Dk.Odense.SSP.Logger
{
    public class LoggerRepository : ILoggerRepository
    {
        private readonly LogDbContext logDbContext;

        public LoggerRepository(LogDbContext logDbContext)
        {
            this.logDbContext = logDbContext;
        }

        public void LogAsync(string user, string method, string reqParams)
        {
            try
            {
                logDbContext.Logs.AddAsync(new Log()
                {
                    User = user,
                    Method = method,
                    RequestParm = reqParams,
                    CreatedDate = DateTime.Now
                });

                logDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void LogListAsync(string user, string method, List<string> reqParamList)
        {
            try
            {
                foreach (var reqParam in reqParamList)
                {
                    logDbContext.Logs.AddAsync(new Log()
                    {
                        User = user,
                        Method = method,
                        RequestParm = reqParam,
                        CreatedDate = DateTime.Now
                    });
                }

                logDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
