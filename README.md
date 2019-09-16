# Compliance 360 Import (Simple batch import)

## Introduction
The Compliance 360 Import application is a sample application that demonstrates importing data into Compliance 360
in the simplest manner. It's focus is for third-party partners who intend on automating the importing of data into
a compliance 360 client organization on a regular basis. It first requires that the given C360 client organization
perform relevant integration configurations to permit this operation.

NOTE: This application is intended as a sample only.

## Configuration

The C360 client organization must first do the following:

1. Create an Integration definition within the C360 website
2. This Integration will need configurations defined for it. Typically:
   - Unique identifier field (e.g. Import Id). This field can be used to supply a unique value to prevent duplicates.
   - Default Module Type (if other than 'Default')
   - Default Workflow Template
   - Default Division/Folder
   - Field Mappings (as needed)
3. Relay the following values for use in this application:
   - Organization
   - ApplicationKey
4. Relay the following target field information for use in this application:
   - Target C360 module name (e.g. Incidents)
   - Target C360 component name (e.g. Incident)
   - Target field names (e.g. Name, Description, etc.)

## Field Mappings

C360 module components 'generally' have consistent fields and names from C360 client organization to
C360 client organization. However, C360 is highly customizable. Therefore it should be anticipated
that field names 'could' differ. If it is critical to code to a standard specification this can be
accommodated by configuring field mappings within the Integration definition. Each client organization
can configure mappings as needed. These would map an incoming field name to a client field name. No
mapping is required when the field names match.

## Project Setup

The application is written in C# and is a .NET Core 2.0 based application. To build and run this application:

1. Ensure that you have the .Net Core Command-Line interface (CLI) tools installed.
2. Clone the repository.
3. From the command line run the following:
```
-- from the Compliance360.Import directory.
dotnet restore
dotnet build
```
## Running the Application

To run the app from the command lime:
```
dotnet run ./bin/Debug/netcoreapp2.0/Compliance360.Import.dll [options]
```

The following lists the required command line options:

| Option | Description |
| ------ | ----------- |
| --filepath | The path to the *.CSV file to import |
| --baseuri | The base uri to the Compliance 360 application. Most likely this value should be https://secure.compliance360.com |
| --organization | The organization name of the C360 client. |
| --integrationkey | The integration key as supplied by the C360 client. |
| --module | The module name of the target C360 Module. |
| --component | The component name of the target C360 component. |

The command line options are in specified in the format: {option}[space]{value}.
```
Example:
--baseuri https://secure.compliance360.com --organization testorg --integrationkey DDIICZ1UORHTFNO5E8JTAVTEBWYH3ZGU9Y0JUPLKQHT --module inicidents --component incident --filepath Incidents-Incident-data.csv
```

## Processing Overview

In general, this application posts all data to C360 in a single http request. The data to input is provided in the body of the authentication request in a simple json structure.
```
Example:
{
	"Incidents":
	{
		"Incident": [
			{
				"Import Id": "1011",
				"Name": "Sample Incident 01",
				"Resp. Party": "David Brown",
				"Description": "Description for Sample Incident 01"
			},
			{
				"Import Id": "1012",
				"Name": "Sample Incident 02",
				"Resp. Party": "David Brown",
				"Description": "Description for Sample Incident 01"
			}
		]
	}
}
```

For convenience, this sample application reads data from a csv file and transforms it in to the above json structure.

## API Documentation

C360 APIs are documented in the [OpenAPI Specification](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md) format and can be found here:

1. [Security API](https://github.com/SAIGlobal/compliance360-security-api)
2. [Metadata API](https://github.com/SAIGlobal/compliance360-metadata-api)
3. [Data API](https://github.com/SAIGlobal/compliance360-data-api)
