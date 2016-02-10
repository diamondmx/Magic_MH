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
        public const string currentConnectionString = connectionStringSekhmet;
    }
}
