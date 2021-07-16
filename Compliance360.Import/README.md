## .NET Example

The application is written in C# and is a .NET Core 2.0 based application. To build and run this application: 
1. Ensure that you have the .Net Core Command-Line interface (CLI) tools installed. 
2. Clone the repository.
3. From the command line run the following:
```
-- from the Compliance360.Import directory.
dotnet restore
dotnet build
``` 

### Running the .NET Application
To run the app from the command lime:
```
dotnet run ./bin/Debug/netcoreapp2.0/Compliance360.Import.dll [options]
```
The options are as follows:
| Option | Description |
| ------ | ----------- |
| --filepath | The path to the *.CSV file to import |
| --baseuri | The base uri to the Compliance 360 application. Most likely this value should be https://secure.compliance360.com |
| --organization | The organization login name of the C360 client. |
| --integrationkey | The integration key as supplied by the C360 client. |
| --module | The module name of the target C360 module (no spaces). |
| --component | The component name of the target C360 component (no spaces). |

The command line options are in the format: {option}[space]{value}. 
```
Example: 
--baseuri https://secure.compliance360.com --organization testorg --integrationkey DDIICZ1UORHTFNO5E8JTAVTEBWYH3ZGU9Y0JUPLKQHT --module inicidents --component incident --filepath Example-Incidents-Incident-data.xlsx 
```

Example Command files are also provided that show these options. 

