# Compliance 360 Import (Batch Import)
The Compliance 360 Import contains example methods and files for posting data to the Compliance 360 API for importing purposes. Its focus is for clients (or third-party partners) who are looking to automate the importing of data into a compliance 360 client organization on a regular basis. It first requires that the given Compliance 360 (C360) client organization admin perform relevant integration configurations to permit this activity. This produces an Integration Key which is then provideded when calling the C360 API Authenticate method.

NOTE: The files contained here are intended as examples.

# Configuration Steps

1. Client Organization's admin must create an Integration definition within the Compliance 360 (C360) website. This is located under the Maintenance -> Integrations menu. Insure that the Host Account chosen has 'API Access' checked under the Module Access tab as well as all necessary permissions to create and update the intended data. Check all 'Allow Access' options except Allow Delete.
2. In the Mappings tab of the Integration definition designate the indentifier field that will determine whether an imported record is created vs. updated.  
| Unique identifier field | e.g. 'Number' for Incidents, 'Number' for Employee |
3. In the Mappings tab of the Integration definition, create mappings for field defaults as needed. Recommended are:
| Employee Import | | | |
| C360 MODULE - COMPONENT | C360 FIELD | EXTERNAL FIELD | DEFAULT VALUE |
| ----- | ------ | ------ | ----------- |
| Employee Management - Employee | Primary Division | (leave empty) | (select the Division where new employees should be created. If not supplied then the primary division of the Host Account that is defined with the Integration will be used.) |
| Employee Management - Employee | Workflow Template Id | (leave empty) | (select the Worflow Template to be used when new employees are created. If not supplied then the default workflow template, as shown in workflow template maintenance, will be used.) |

| Incident Import | | | |
| C360 MODULE - COMPONENT | C360 FIELD | EXTERNAL FIELD | DEFAULT VALUE |
| ----- | ------ | ------ | ----------- |
| Incidents - Incident | Module Type Name | (leave empty) | Module Type where new incidents should be created. If not supplied then the The Module Type named "Default" will be used. |
| Incidents - Incident | Folder Id | (leave empty) | Folder where new incidents should be created. If not supplied then the then the root folder of the primary division of the Host Account that is defined with the Integration will be used. |
| Incidents - Incident | Workflow Template Id | (leave empty) | (select the Worflow Template to be used when new incidents are created. If not supplied then the default workflow template, as shown in workflow template maintenance, will be used.) |
4. Format the incoming data to the example file. Note that the field names in the example file are the standard field names but your organization may have custom fields instead as well as additional custom fields to be imported. Review your potential fields as follows; 
* Compile a list of the desired import fields. You may find it helpful to review the Edit form of the importing component (Employee, Incident, etc.). Do this by navigating to Maintenance -> Forms Configuration and editing the appopriate form.
* Obtain the exact spelling of the field by placing the mouse cursor over the caret to the right of the field and waiting 2-3 seconds. The exact field name will appear. This is the name to use in the first row of your import file.
* Do not assume that the field label is the field name as they can be (and often are) different. 
* Note that a full list of fields can be found under the Maintenance -> Modules menu and selecting the appropriate component. Use the Name value and NOT the System Name value.
5. In the Field Mappings tab of the Integration definition create additional Mappings as needed.

# Testing
All imports should be thoroughly tested and the results reviewed before running on production data. This can be done by arranging for a test database to be created by contacting SAI support (support@sai360.com).

# Example Methods for posting data

## PowerShell Example

The powerscript file Example-Employee-Import.ps1 is a 3 line script that posts the import file directly to Compliance360 which then queues it for import. 
1. Edit this powerscript file to supply the url that is copied from the Integration definition.
2. Invoke it by going to a command prompt, navigating to the folder where it resides and isssuing the following command;
```
powershell -File .\Example-Employee-Import.ps1
```

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

# Example Files
| Format | Description |
| ------ | ----------- |
| Excel | The file Example-Incidents-Incident-data.xlsx contains sample Incident fields and data. |
| Delimited | The file Example-EmployeeManagement-Employee-data.csv contains sample Employee fields and data, This is a comma-delimited file (field values are seperated by commas) but any delimiter can be used if the url contains a &FieldDelimiter= parameter identifying the desired delimiter. | 
| JSON | Example available upon request.

