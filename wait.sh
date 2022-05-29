set sqlcmdCommand = "sqlcmd -b -Q \"SELECT @@SERVERNAME as ServerName\""
eval $sqlcmdCommand