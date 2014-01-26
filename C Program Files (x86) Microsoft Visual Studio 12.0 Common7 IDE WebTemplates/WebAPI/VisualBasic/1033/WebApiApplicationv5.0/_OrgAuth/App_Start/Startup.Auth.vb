﻿Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.ActiveDirectory
Imports Owin

Public Partial Class Startup
    ' For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    Public Sub ConfigureAuth(app As IAppBuilder)
        app.UseWindowsAzureActiveDirectoryBearerAuthentication(New WindowsAzureActiveDirectoryBearerAuthenticationOptions() With {
          .Audience = ConfigurationManager.AppSettings("ida:Audience"),
          .Tenant = ConfigurationManager.AppSettings("ida:Tenant")
        })
    End Sub
End Class
