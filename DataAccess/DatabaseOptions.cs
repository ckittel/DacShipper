namespace DacShipper
{
    public class DatabaseOptions
    {
        public bool UseManagedIdentity { get; set; } = true;
        public string UserId { get; set; } = null;
        public string Password { get; set; } = null;

        public string BuildConnectionString(string serverName, string databaseName)
        {
            // TODO: Initial tests indicate that we might not t need to provide the "Initial Catalog=" part here, simplifying the base connection string builder
            var baseConnectionString = $"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog={databaseName};Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            if (UseManagedIdentity)
            {
                return baseConnectionString;
            }

            // slug in the U/P
            return $"{baseConnectionString}User ID={UserId};Password={Password};";
        }
    }
}
