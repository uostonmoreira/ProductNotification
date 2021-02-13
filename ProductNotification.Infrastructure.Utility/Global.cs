using System;

namespace ProductNotification.Infrastructure.Utility
{
    public static class Global
    {
        public static readonly string ASPNETCORE_ENVIRONMENT = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").ToLower();
    }
}
