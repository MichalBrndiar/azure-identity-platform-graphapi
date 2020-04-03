# Azure-GraphAPI
Demonstrates invocation of GraphAPI from Web API secured by Azure Active Directory (Microsoft Identity Platform)

## Folders

| Folder          | Description             
| -------------- | ----------------------------------------------------------- |
| src/Client.Console | Console application calling AAD protected Web API via Microsoft.Identity.Client    
| src/WebAPI    | Protected Web API that in turn invokes Graph API on behalf of itself (application) 
| src/Microsoft.Identity.Web    | Enhancements for web over Microsoft.Idenitity.Client


## Various links
[Microsoft identity platform documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/)

[Original github repo this solution is based on](https://github.com/Azure-Samples/active-directory-dotnet-native-aspnetcore-v2)

[Microsoft.Identity.Web repo](https://github.com/AzureAD/microsoft-identity-web)

## Hint

Project contains submodule, so use following git command to fetch all.

```shell
git clone --recurse-submodules  https://github.com/MichalBrndiar/azure-identity-platform-graphapi.git
```

