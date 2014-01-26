Imports System.IdentityModel.Tokens
Imports System.Runtime.CompilerServices
Imports System.Web.Hosting
Imports System.Xml.Linq

Namespace Utils
    Public Class DatabaseIssuerNameRegistry
        Inherits ValidatingIssuerNameRegistry

        Public Shared Function ContainsTenant(tenantId As String) As Boolean
            Using context As New TenantDbContext()
                Return context.Tenants.Where(Function(tenant) tenant.Id = tenantId).Any()
            End Using
        End Function

        Public Shared Function ContainsKey(thumbprint As String) As Boolean
            Using context As New TenantDbContext()
                Return context.IssuingAuthorityKeys.Where(Function(key) key.Id = thumbprint).Any()
            End Using
        End Function

        Public Shared Sub RefreshKeys(metadataLocation As String)
            Dim issuingAuthority As IssuingAuthority = ValidatingIssuerNameRegistry.GetIssuingAuthority(metadataLocation)

            Dim newKeys As Boolean = False
            For Each thumbprint As String In issuingAuthority.Thumbprints
                If Not ContainsKey(thumbprint) Then
                    newKeys = True
                    Exit For
                End If
            Next

            If newKeys Then
                Using context As New TenantDbContext()
                    context.IssuingAuthorityKeys.RemoveRange(context.IssuingAuthorityKeys)
                    For Each thumbprint As String In issuingAuthority.Thumbprints
                        context.IssuingAuthorityKeys.Add(New IssuingAuthorityKey() With {
                            .Id = thumbprint
                        })
                    Next
                    For Each issuer As String In issuingAuthority.Issuers
                        context.Tenants.Add(New Tenant() With {
                            .Id = issuer.TrimEnd("/"C).Split("/"C).Last()
                        })
                    Next
                    context.SaveChanges()
                End Using
            End If
        End Sub

        Protected Overrides Function IsThumbprintValid(thumbprint As String, issuer As String) As Boolean
            Dim issuerID As String = issuer.TrimEnd("/"C).Split("/"C).Last()

            Return ContainsTenant(issuerID) AndAlso ContainsKey(thumbprint)
        End Function
    End Class
End Namespace
