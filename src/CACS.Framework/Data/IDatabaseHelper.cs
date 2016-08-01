using System.Collections.Generic;

namespace CACS.Framework.Data
{
    public interface IDatabaseHelper
    {
        string ProviderName { get; }

        Dictionary<string, object> Variables { get; }

        string CreateConnectionString();
    }
}
