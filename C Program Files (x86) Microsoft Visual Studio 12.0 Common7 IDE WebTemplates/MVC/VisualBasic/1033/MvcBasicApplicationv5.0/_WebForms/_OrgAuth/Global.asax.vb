Imports System.IdentityModel.Services
Imports System.Web.Optimization

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        IdentityConfig.ConfigureIdentity()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Sub WSFederationAuthenticationModule_RedirectingToIdentityProvider(sender As Object, e As RedirectingToIdentityProviderEventArgs)
        If Not String.IsNullOrEmpty(IdentityConfig.Realm) Then
            e.SignInRequestMessage.Realm = IdentityConfig.Realm
        End If
    End Sub
End Class
