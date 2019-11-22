using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Dac;

namespace DacShipper
{
    public static class Function1
    {

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("dacpacs/maindb.dacpac", FileAccess.Read)] Stream dacpac,
            ILogger log)
        {
            string sqlDiff = string.Empty;

            try
            {
                // Load the DacPac
                var pkg = DacPackage.Load(dacpac);

                // Connect to the DB (obviously don't inline this...)
                var dacService = new DacServices("Server=tcp:CHANGEME.database.windows.net,1433;Initial Catalog=main;Persist Security Info=False;User ID=CHANGEME;Password=CHANGEME;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

                // Generate Diff for capture purposes
                sqlDiff = dacService.GenerateDeployScript(pkg, "main");

                // Do the deployment
                dacService.Deploy(pkg, "main", upgradeExisting: true);
            }
            catch (Exception ex)
            {
                sqlDiff += Environment.NewLine + Environment.NewLine + ex.ToString();
                log.LogInformation(sqlDiff);
            }

            return new OkObjectResult(sqlDiff);
        }
    }
}
