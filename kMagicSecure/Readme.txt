Setup Requirement:
Must create kMagicSecure\ConnectionStrings.cs as follows:
	<connectionStrings>
		<add name ="DefaultConnection" connectionString="Server=tcp:kmagic.database.windows.net,1433;Database=kMagic;User ID=kmagic@kmagic;Password=PASSWORD;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" providerName="System.Data.SqlClient"/>
		<add name="SendGridAPI" connectionString="API KEY HERE"/>
	</connectionStrings>
	