using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Dac;

namespace DacShipper
{

    public class Function1
    {
        private readonly IDacServicesFactory _dacServicesFactory;

        public Function1(IDacServicesFactory dacServicesFactory)
        {
            _dacServicesFactory = dacServicesFactory;
        }

        [FunctionName("Function1")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("dacpacs/maindb.dacpac", FileAccess.Read)] Stream dacpac,
            ILogger log)
        {
            var sqlDiff = string.Empty;

            try
            {
                // Load the DacPac
                var pkg = DacPackage.Load(dacpac);

                // Connect to the DB
                var dacService = _dacServicesFactory.Create("ckittel", "main");

                // Generate Diff for capture purposes
                sqlDiff = dacService.GenerateDeployScript(pkg, "main");

                // Do the deployment
                dacService.Deploy(pkg, "main", upgradeExisting: true);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                sqlDiff += Environment.NewLine + Environment.NewLine + ex.ToString();
                log.LogInformation(sqlDiff);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return new OkObjectResult(sqlDiff);
        }
    }
}
