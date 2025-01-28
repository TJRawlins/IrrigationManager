using Infisical.Sdk;

namespace IrrigationManager.Extensions
{
    public static class LicenseServiceExtension
    {
        public static void AddZEntityFrameworkExtenssionsLicense()
        {
            // Environment Variables (.env)
            var ClientId = Environment.GetEnvironmentVariable("INFISICAL_DROPLET_CLIENT_ID");
            var ClientSecret = Environment.GetEnvironmentVariable("INFISICAL_DROPLET_CLIENT_SECRET");
            var ProjectId = Environment.GetEnvironmentVariable("INFISICAL_DROPLET_PROJECT_ID");

            // Infisical Secrets Key Vault
            ClientSettings settings = new ClientSettings
            {
                Auth = new AuthenticationOptions
                {
                    UniversalAuth = new UniversalAuthMethod
                    {
                        ClientId = ClientId!,
                        ClientSecret = ClientSecret!
                    }
                }
            };

            var infisicalClient = new InfisicalClient(settings);
            var secretLicenseName = infisicalClient.GetSecret(new GetSecretOptions
            {
                SecretName = "Z_ENTITY_FRAMEWORK_EXTENSIONS_LICENSE_NAME",
                ProjectId = ProjectId!,
                Environment = "dev",
            });
            var secretLicenseKey = infisicalClient.GetSecret(new GetSecretOptions
            {
                SecretName = "Z_ENTITY_FRAMEWORK_EXTENSIONS_LICENSE_KEY",
                ProjectId = ProjectId!,
                Environment = "dev",
            });   

            string licenseName = secretLicenseName.SecretValue;
            string licenseKey = secretLicenseKey.SecretValue;

            // Add License via code
            Z.EntityFramework.Extensions.LicenseManager.AddLicense(licenseName, licenseKey);

            string licenseErrorMessage;
            if (!Z.EntityFramework.Extensions.LicenseManager.ValidateLicense(out licenseErrorMessage))
            {
                throw new Exception(licenseErrorMessage);
            }
        }
    }
}
