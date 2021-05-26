# Compliance 360 Import (batch import)
* [Description](#description)
* [Installation](#installation)
* [Configuration](#configuration)
* [Usage](#usage)
* [API Documentation](#api-documentation)

# Description
The Compliance 360 Import application is a sample application that demonstrates posting data to the Compliance 360 API for importing in the most efficient manner. It's focus is for clients (or third-party partners) who are looking to automate the importing of data into a compliance 360 client organization on a regular basis. It first requires that the given Compliance 360 (C360) client organization admin perform relevant integration configurations to permit this activity. This produces an Integration Key which is then provideded when calling the C360 API Authenticate method.

NOTE: This application is intended as a sample only.

# Installation
The application is written in C# and is a .NET Core 2.0 based application. To build and run this application: 
1. Ensure that you have the .Net Core Command-Line interface (CLI) tools installed. 
2. Clone the repository.
3. From the command line run the following:
```
-- from the Compliance360.Import directory.
dotnet restore
dotnet build
``` 
# Configuration
## Create Integration Definition within Compliance 360
1. Client Organization's admin must create an Integration definition within the Compliance 360 (C360) website (found under the Maintenance -> Integrations menu).
2. The Integration definition should then contain the following configurations (C360 Support can assist with this: https://support.sai360.com):

| Field | Applicable | Defaulting Behavior |
| ----- | ------ | ----------- |
| Unique identifier field | e.g. 'Import Id' for Incidents, 'Number' for Employee | With this, C360 will automatically determine if a data item to import already exists before adding it and, instead, update it. |
| Module Type | Incident | Module Type Name where new items should be created (if other than the Module Type named 'Default') |
| Workflow Template | Incident, Employee | if other than the workflow template designated as the default |
| Division | Employee | Division Path where new items should be created |
| Folder | Incident | Folder Path where new items should be created |
3. The following values should then be supplied for use with this application:

| Value | Description |
| ----- | ----------- |
| Login Organization | Used at Login |
| Integration Key | This is generated from Integration definition in step 1 |
| Module name | as shown in Maintenance -> Modules menu (with no spaces. e.g. 'EmployeeManagement', 'Incidents', etc.) |
| Component name | As shown in Component Fields page (with no spaces. e.g. 'Incident', 'Employee', etc.|
| Field Names | (e.g. Last Name, Description, etc.). Fields are denoted by NAME as shown in the Maintenance -> Modules -> Component Fields list for a given component (Note: NOT System Name). Therefore, take note of any spaces that are in the field NAME and explicitly supply them accordingly. |

## Default Values
Specific required system fields will automatically default as follows if not provided in the Integration definition or the data itself;
| Field | Applicable | Defaulting Behavior |
| ----- | ------ | ----------- |
| Module Type | Incident | The Module Type named "Default". |
| Workflow Template | Employee, Incident | Standard default workflow template as shown in workflow template maintenance for the given component. |
| Division | Employee | If a "Primary Division" field is not supplied for each data item then the primary division of the Host User that is defined with the Integration will be used. |
| Folder | Incident | If a "Folder" field is not supplied for each data item, then the root folder of the primary division of the Host User that is defined with the Integration will be used. |

Alternate defaults can be configured for these or any field in the Integration definition. 

## Reference Values
Any field that is a reference value can be set simply by providing the value for the Display field for the related component. For instance, setting the Division for an Employee item is done by designating the Division Name for the ‘Primary Division’ field. Setting a lookup value is done by designating the displayed text for the intended lookup item. 

## Field Mappings
C360 module components generally have consistent fields and names from C360 client organization to C360 client organization. However, C360 is highly customizable. Therefore, if coding a single custom application to be used across multiple C360 clients it should be anticipated that field names could differ. If it is critical to code to a standard specification this can be accommodated by configuring Field Mappings within the Integration definition. Each client organization can configure mappings as needed. These would map an incoming 'standard' field name to a client field name. No mapping is required when the field names match.
# Usage
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
| --organization | The organization login name of the C360 client. |
| --integrationkey | The integration key as supplied by the C360 client. |
| --module | The module name of the target C360 module (no spaces). |
| --component | The component name of the target C360 component (no spaces). |

The command line options are in specified in the format: {option}[space]{value}. 
```
Example: 
--baseuri https://secure.compliance360.com --organization testorg --integrationkey DDIICZ1UORHTFNO5E8JTAVTEBWYH3ZGU9Y0JUPLKQHT --module inicidents --component incident --filepath Sample-Incidents-Incident-data.xlsx 
```

## Processing Overview
1. This application posts all data to C360 in a single http post to the 'Authenticate' API method. Up to 128 meg of datat can be posted. The Authenticate API method requires an Integration Key. An Integration Key requires that someone first create the Integration via the client organization’s Maintenance -> Integrations menu. Upon a successful call, the Authenticate method returns a token that can be used for further API calls if needed.
2. The Authenticate method can optionally contain a payload (body). When it does, the payload is evaluated and processed as a data import. Importing in this manner can be done for an individual data item or multiple items (i.e. batch). Once posted, the import will be queued for import. Upon completion, a summary email will be delivered to the recipients defined in the Integration. Status can also be viewed under the Home - Job Status menu.
3. Whether or not a payload is provided, the Authenticate method returns a token that can then be used for further API calls.
4. Currently, the payload supports Excel (.xlsx), Delimited (e.g. csv) and JSON formats.

# Samples Provided
| Format | Description |
| ------ | ----------- |
| Excel | The file Sample-Incidents-Incident-data.xlsx will be posted and imported by running the Sample-Incident-Import.cmd command file (note that it must first be edited to supply the login organization and Integration Key). |
| Delimited | The file Sample-EmployeeManagement-Employee-data.csv will be posted and imported by running the Sample-Employee-Import.cmd command file (note that it must first be edited first to supply the login organization and Integration Key). This is a comma-delimited file (field values are seperated by commas) but any delimiter can be used if the url contains a &FieldDelimiter= parameter identifying the desired delimiter (the application must be modified to do this). | 
| JSON | Sample not currently provided. See below for a example of what the payload (body) of the post should contain in order to utilize Json.
```
Example JSON format: 
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

# API Documentation
Full API Documentation can be found here [Compliance 360 REST API Documentation](https://github.com/SAIGlobal/Compliance360-API-Documentation)
