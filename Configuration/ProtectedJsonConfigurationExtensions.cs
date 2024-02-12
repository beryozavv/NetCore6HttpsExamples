namespace NetCore6Https.Configuration
{
    /// <summary>
    /// Класс расширений содержащий методы добавления защищенной конфигурации приложения.
    /// </summary>
    public static class ProtectedJsonConfigurationExtensions
    {
        /// <summary>
        /// Добавить защищенную конфигурацию приложения.
        /// </summary>
        public static IConfigurationBuilder AddProtectedJsonFile(this IConfigurationBuilder builder, string path, IEnumerable<string> protectedKeys,
            bool optional, bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Неверный путь файла конфигураций", nameof(path));
            }

            return builder.AddProtectedJsonFile(s =>
            {
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ProtectedKeys = protectedKeys;
                s.ResolveFileProvider();
            });
        }

        private static IConfigurationBuilder AddProtectedJsonFile(this IConfigurationBuilder builder,
            Action<ProtectedJsonConfigurationSource> configureSource)
        {
            ProtectedJsonConfigurationSource configurationSource = new ProtectedJsonConfigurationSource();
            configureSource(configurationSource);
            return builder.Add(configurationSource);
        }
    }
}
