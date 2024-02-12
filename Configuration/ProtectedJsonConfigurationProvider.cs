using System.Diagnostics;
using Microsoft.Extensions.Configuration.Json;

namespace NetCore6Https.Configuration
{
    /// <summary>
    /// Расширенный провайдер <see cref="JsonConfigurationProvider"/> позволяющий работать с конфигурацией содержащие шифрованные данные.
    /// </summary>
    public class ProtectedJsonConfigurationProvider : JsonConfigurationProvider
    {
        private readonly ProtectedJsonConfigurationSource _protectedJsonConfigurationSource;

        /// <inheritdoc/>
        public ProtectedJsonConfigurationProvider(ProtectedJsonConfigurationSource source) : base(source)
        {
            _protectedJsonConfigurationSource = source;
        }

        /// <inheritdoc/>
        public override void Load(Stream stream)
        {
            base.Load(stream);

            //IDataProtector dataProtector = DataProtection.CreateProtector(new DirectoryInfo(dataProtectionKeysDirectory), dataProtectionPurpose);

            foreach (string key in _protectedJsonConfigurationSource.ProtectedKeys)
            {
                if (Data.TryGetValue(key, out var value))
                {
                    Debug.WriteLine("Key ={0} : Value = {1}", key, Data[key]);
                    
                    if (value == "$CREDENTIAL_PLACEHOLDER$")
                    {
                        //Data[key] = dataProtector.Unprotect(Data[key]);
                        Data[key] = "myPassword";
                    }
                }
            }
        }
    }
}
