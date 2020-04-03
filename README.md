# Azure-GraphAPI
Demonstrates invocation of GraphAPI from Web API secured by Azure Active Directory (Microsoft Identity Platform)

## Folders

| Folder          | Description             
| -------------- | ----------------------------------------------------------- |
| src/Client.Console | Console application calling AAD protected Web API via Microsoft.Identity.Client    
| src/WebAPI    | Protected Web API that in turn invokes Graph API on behalf of itself (application) 
| src/Microsoft.Identity.Web    | Enhancements for web over Microsoft.Idenitity.Client


## Various links
[https://docs.microsoft.com/en-us/azure/active-directory/develop/](Microsoft identity platform documentation)

[https://github.com/Azure-Samples/active-directory-dotnet-native-aspnetcore-v2](Original github repo this solution is based on)

[https://github.com/AzureAD/microsoft-identity-web](Microsoft.Identity.Web repo)

