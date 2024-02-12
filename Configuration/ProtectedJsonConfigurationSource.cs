using Microsoft.Extensions.Configuration.Json;

namespace NetCore6Https.Configuration
{
    public class ProtectedJsonConfigurationSource : JsonConfigurationSource
    {
        /// <summary>
        /// Идентификаторы ключей конфигурации, значения которых содержат шифрованные данные. 
        /// </summary>
        public IEnumerable<string> ProtectedKeys { get; set; }

        /// <inheritdoc/>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            ResolveFileProvider();
            EnsureDefaults(builder);
            return new ProtectedJsonConfigurationProvider(this);
        }
    }
}
