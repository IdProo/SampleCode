# ASP.NET MVC SAML Client

This project is an ASP.NET MVC SAML client, modified from the [ITfoxtec.Identity.Saml2](https://github.com/ITfoxtec/ITfoxtec.Identity.Saml2/tree/4.10.9-beta2/test/TestWebAppCore) library. It uses the following libraries:

- ITfoxtec.Identity.Saml2
- ITfoxtec.Identity.Saml2.MvcCore

## Conditions for Running the Project
- Client (this project) metadata must be able to be accessed from IdP
- Certificate *.pfx *needs to be provided

## Important Endpoints for SAML Login Process

- `/Auth/Login`
- `/Auth/AssertionConsumerService`
- `/Metadata`

## Important Appsettings Configuration

The following appsettings configuration is required:

- `IdPMetadata`: The URL of the IdP metadata
- `Issuer`: The client ID in IdP
- `SigningCertificate`: The certificate for signing
- `SigningCertificatePassword`: The password for the certificate