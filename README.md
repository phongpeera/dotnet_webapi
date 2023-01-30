
# DISE VRU Training Dotnet WebApi

#### Installation Dev Tools
1. [Download .Net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. [Download Visual Studio Code](https://code.visualstudio.com/)
3. [Install C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
4. [Install SQLTools Extension](https://marketplace.visualstudio.com/items?itemName=mtxr.sqltools)
5. [Install SQLTools PostgreSQL/Cockroach Driver Extension](https://marketplace.visualstudio.com/items?itemName=mtxr.sqltools-driver-pg)
6. [Install Thunder Client Extension](https://marketplace.visualstudio.com/items?itemName=rangav.vscode-thunder-client)

#### Run project
1. Open New Terminal in Visual Studio Code
2. **Run** dotnet restore
3. **Run** dotnet ef migrations add Init
4. **Edit** appsettings.json and appsettings.Development.json
4. **Run** dotnet ef database update