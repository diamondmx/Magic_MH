using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data.LocalSetup
{
    public class Constants
    {
        public const string connectionStringkCura = @"Data Source=P-DV-DSK-MHIL;Initial Catalog=Magic;User ID=mhMagic;Password=mtgMagic";
        public const string connectionStringSekhmet = @"Data Source=SEKHMET\SQLSEKHMET;Initial Catalog=Magic;User ID=mhMagic;Password=mtgMagic";
				public const string connectionStringAzure = @"Server=tcp:kmagic.database.windows.net,1433;Database=kMagic;User ID=kmagic@kmagic;Password=^1lRoBoW55;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const string currentConnectionString = connectionStringSekhmet;
    }
}
