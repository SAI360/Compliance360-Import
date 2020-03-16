# Compliance 360 Import (Simple batch import)
## Introduction
The Compliance 360 Import application is a sample application that demonstrates importing data into Compliance 360 in the simplest manner. It's focus is for third-party partners who intend on automating the importing of data into a compliance 360 client organization on a regular basis. It first requires that the given C360 client organization perform relevant integration configurations to permit this operation. 

NOTE: This application is intended as a sample only.

## Configuration
The C360 client organization must first do the following:
1. Create an Inbound Integration definition within the C360 website under the Maintenance->Integrations menu option.
2. Whomever creates the integration should then relay the following target field information for use in this application:
 a. Target C360 module name with no spaces (e.g. EmployeeManagement, Incidents, etc.)
 b. Target C360 component name (e.g. Incident, Employee, etc.)
 c. Target field names (e.g. Last Name, Description, etc.). Fields are denoted by NAME as shown in the Maintenance->Modules->Component Fields list for a given component (Note: NOT System Name). Therefore, take note of any spaces that are in the NAME and supply them accordingly.

## Unique Identifier Field
C360 will automatically determine if a data item to import already exists before adding it and, instead, update it. This is done based upon the value supplied in a unique identifier field. By default, the unique identifier field for the Employee component is the "Number" field. For the Incident component it is the "Import Id" field. However, an alternate unique identifier field can be designated for any component as needed when configuring the Integration definition. 
 
## Default Values
Specific required system fields will automatically default as follows;
1. Division (Employee): If a "Primary Division" field is not supplied for each data item then the primary division of the Host User that is defined with the Integration will be used.
2. Workflow Template (Employee, Incident): Standard default workflow template as shown in workflow template maintenance for the given component.
3. Folder (Incident): If a "Folder" field is not supplied for each data item, then the root folder of the primary division of the Host User that is defined with the Integration will be used.
4. Module Type (Incident): The Module Type named "Default".

Alternate defaults can be configured for these or any field. This is done in the Integration definition. 

## Reference Values
Any field that is a reference value can be set simply by providing the value for the Display field for the related component. For instance, setting the Division for an Employee item is done by designating the Division Name for the ‘Primary Division’ field. Setting a lookup value is done by designating the displayed text for the intended lookup item. 

## Field Mappings
C360 module components generally have consistent fields and names from C360 client organization to C360 client organization. However, C360 is highly customizable. Therefore, if coding a single custom application to be used across multiple C360 clients it should be anticipated that field names could differ. If it is critical to code to a standard specification this can be accommodated by configuring Field Mappings within the Integration definition. Each client organization can configure mappings as needed. These would map an incoming 'standard' field name to a client field name. No mapping is required when the field names match.

# Sample Application Project Setup
The application is written in C# and is a .NET Core 2.0 based application. To build and run this application: 
1. Ensure that you have the .Net Core Command-Line interface (CLI) tools installed. 
2. Clone the repository.
3. From the command line run the following:
```
-- from the Compliance360.Import directory.
dotnet restore
dotnet build
``` 

# Running the Application
To run the app from the command lime:
```
dotnet run ./bin/Debug/netcoreapp2.0/Compliance360.Import.dll [options]
```

The following lists the required command line options:
| Option | Description |
| ------ | ----------- |
| --filepath | The path to the *.CSV file to import |
| --baseuri | The base uri to the Compliance 360 application. Most likely this value should be https://secure.compliance360.com |
| --organization | The organization login name of the C360 client. |
| --integrationkey | The integration key as supplied by the C360 client. |
| --module | The module name of the target C360 module (no spaces). |
| --component | The component name of the target C360 component (no spaces). |

The command line options are in specified in the format: {option}[space]{value}. 
```
Example: 
--baseuri https://secure.compliance360.com --organization testorg --integrationkey DDIICZ1UORHTFNO5E8JTAVTEBWYH3ZGU9Y0JUPLKQHT --module inicidents --component incident --filepath Incidents-Incident-data.csv 
```

# Processing Overview
1. This application posts all data to C360 in a single http post to the 'Authenticate' REST API method. The Authenticate REST API method requires an Integration Key and is recommended instead of the Login method if authenticating to perform subsequent REST API calls. Integration Keys require someone to create the Integration via the client organization’s Maintenance Integrations menu. Upon a successful call, the Authenticate method returns a token that can be used for further REST API calls if desired. The token does not need to be utilized if providing an import payload in the post (see below).
2. The Authenticate method can optionally contain a payload (body). When it does, the payload is evaluated and processed as a data import. Importing in this manner can be done for oan individual data item or multiple items (i.e. batch). For convenience, this sample application reads all data from the associated csv file, transforms to JSON and posts it all at once in a single batch.
3. Currently, the payload supports only a JSON format but will eventually support Excel files, csv files etc. The data to import is provided in the body of the authentication request in a simple json structure where the first level is module name (no spaces) and the second is the component name (no spaces).
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
# Tips/Known Issues
1. This sample application does not test for embedded commas in field values while converting from csv to JSON. This must be coded in to the conversion if desired.
2. Lookup values that are supplied that are not an exact match will generate a HTTP 500 error. This will be fixed in 2020.4 release.


# API Documentation
C360 APIs are documented in the [OpenAPI Specification](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md) format and can be found here:
1. [Security API](https://github.com/SAIGlobal/compliance360-security-api)
2. [Metadata API](https://github.com/SAIGlobal/compliance360-metadata-api)
3. [Data API](https://github.com/SAIGlobal/compliance360-data-api) 
