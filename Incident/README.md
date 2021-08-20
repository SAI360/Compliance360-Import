# Incident Batch Import
Batch Import consists of a single API call (Authenticate) which accepts a data file and then performs many common processing tasks to import the contents. This includes determining whether to add or update each item, defaulting field values, resolving internal references to related data, mapping fields as needed, import job summary and job notifications. Its focus is for clients (or third-party partners) who are looking to automate the importing of data into a compliance 360 client organization on a regular basis.

Steb-by-Step Instructions:

[I. Determine Input File](#i-determine-input-file)<br />
[II. Configure Integration](#ii-configure-integration)<br />
[III. Testing](#iii-testing)<br />
[IV. Posting Data](#iv-posting-data)<br />
[V. Support](#v-support)<br />

## I. Determine Input File
Supported file formats are;
* Excel
* Delimited (CSV) 
* JSON.

If a pre-existing input file format is preferred, this can be supported via Integration Field Mapping. Alternatively, use the following recommendations when establishing the input file format;
1. Consider using an Excel file format. This eliminates issues with embedded delimiters (i.e. commas) and new lines. Do not use a Compliance360 IMA template file (.XML) as they are not compatible. Follow these guidelines to determine the fields (columns) in your input file.
2. Compile a list of the desired import fields. Best practice is to review the actual Edit form of the importing component (i.e. Incident).
3. Only include fields for which you have relevant data to import. There's no need to import all C360 fields such that every item imports an empty value for a field. 
4. Use field names in your input file that match the C360 field names. Doing this makes it easier to maintain by avoiding field mapping. Use these tips to find the names;
* Navigate to Maintenance -> Forms Configuration, choose the module and component and edit the appopriate form (e.g. 'Incident Form'). 
* Obtain the exact spelling of the field by placing the mouse cursor over the caret to the right of the field and waiting 2-3 seconds. The exact field name will appear. This is the name to use in the first row of your import file.
* Do not assume that the field label is the field name as they can be (and often are) different. 
* Note that a full list of fields can be found under the Maintenance -> Modules menu and selecting the appropriate component and choosing Component Fields. Use the Name value and NOT the System Name value.
5. It is unnessary to include fields with values that are the same for every record (e.g. Division, workflow template). A single Default Field Mapping for all items can be configured instead.

Name the input file as you prefer. Once posted, it is immediately read and queued to process.  

### Example Files
The following example files are not necessarily complete to a given client's needs but should work as an initial test and can then be used as a guide for building-out the desired input file;

| Format | Description |
| ------ | ----------- |
| Excel | The file [Example-Incidents-Incident-data.xlsx](Example-Incidents-Incident-data.xlsx) contains sample Incident fields and data. |
| Delimited | The file [Example-Incidents-Incident-data.csv](Example-Incidents-Incident-data.csv) contains sample Incident fields and data, This is a comma-delimited file (field values are seperated by commas) but any delimiter can be used if the url contains a &FieldDelimiter= parameter identifying the desired delimiter. | 
| JSON | Example available upon request.

## II. Configure Integration

1. Client Organization's admin must create an Integration definition within the Compliance 360 (C360) website. This is located under the Maintenance -> Integrations menu. Choose Integration Type of 'User-Defined'. Ensure that the Host Account chosen has 'API Access' checked under the Module Access tab as well as all necessary permissions to create and update the intended data.
2. In the Field Mappings tab of the Integration definition create an entry that designates the one (1) C360 Field that will be the unique identifier which will determine whether an imported item is added vs. updated. Recommended:

| C360 MODULE - COMPONENT | C360 FIELD | EXTERNAL FIELD | DEFAULT VALUE | IDENTIFIER |
| --------------------------- | ----------------------------------- | ---------------------------------- | ------------------ | - |
| Incidents - Incident | Number | (field from import file) | (leave empty) | Checked |

3. In the Field Mappings tab of the Integration definition, create mapping defaults for fields whose values are not supplied in the import file. Recommended are:

| C360 MODULE - COMPONENT | C360 FIELD | EXTERNAL FIELD | DEFAULT VALUE |
| --------------------------- | --------------------- | ------------------ | ------------------------------------------------------------------------ |
| Incidents - Incident | Module Type Name | (leave empty) | Module Type where new incidents should be created. If not supplied then the The Module Type named "Default" will be used. |
| Incidents - Incident | Folder Id | (leave empty) | Folder where new incidents should be created. If not supplied then the then the root folder of the primary division of the Host Account that is defined with the Integration will be used. |
| Incidents - Incident | Workflow Template Id | (leave empty) | (select the Worflow Template to be used when new incidents are created. If not supplied then the default workflow template, as shown in workflow template maintenance, will be used.) |
4. Format the incoming data to the example file. Note that the field names in the example file are the standard field names but your organization may have custom fields instead as well as additional custom fields to be imported. Review your potential fields as follows; 
* Compile a list of the desired import fields. You may find it helpful to review the Edit form of the importing component (i.e. Incident). Do this by navigating to Maintenance -> Forms Configuration and editing the appopriate form.
* Obtain the exact spelling of the field by placing the mouse cursor over the caret to the right of the field and waiting 2-3 seconds. The exact field name will appear. This is the name to use in the first row of your import file.
* Do not assume that the field label is the field name as they can be (and often are) different. 
* Note that a full list of fields can be found under the Maintenance -> Modules menu and selecting the appropriate component. Use the Name value and NOT the System Name value.
5. In the Field Mappings tab of the Integration definition create additional Mappings as needed. For instance, if field names in the import file cannot be made to match C360 field names, you can add mappings to designate how they relate.

4. In the Field Mappings tab of the Integration definition create additional Mappings as needed. For instance, if field names in the import file cannot be made to match C360 field names, you can add mappings to designate the corresponding field name in the input file (External Field).

## III. Testing
All imports should be thoroughly tested and the results reviewed before importing to production data. This can be done by;
1. Arranging for a test database to be created by contacting SAI support (support@sai360.com).
2. Post the data to the test site
3. Monitor the progress of the job via the C360 Home -> Job Status menu. Cllck on the Magnifying glass on the relevant entry to see a detailed summary of the import.
4. Review results via C360 Forms, Reporting and Audit Trail.

## IV. Posting Data

The powershell Example-Incident-Import.ps1 is a simple 4 line script that demonstrates posting the import file directly to Compliance360 and queueing it for import.
1. If not already downloaded, download this GitHub repository ([https://github.com/SAIGlobal/Compliance360-Import](https://github.com/SAIGlobal/Compliance360-Import)) by clicking on the Code button. 
2. Locate the powershell file Example-Incident-Import.ps1. Edit line 1 of this file to supply the url that is copied from the Integration definition. Edit line 2 of this file to supply the name of the file being imported. Place the input file in the same folder.
3. Locate the companion windows command file Example-Incident-Import.cmd in the same folder. Double-clicking on this command file will immediately post the input file to C360 and queue it for import. 
4. Job status can then be viewed in C360 under the Home -> Job Status menu. Click on the Magnifying glass on the relevant entry to see a detailed summary of the import.

NOTE: ALL files contained here are intended as examples only of the Batch API call and do not constitute supportable C360 application components.

Additional methods for posting the input file to C360 are as follows;
* The input file can be posted directly from several commercial products. 
* If developing a custom application, the following example demonstrates a .NET implementation. [Compliance360.Import](https://github.com/SAIGlobal/Compliance360-Import/tree/master/Compliance360.Import)

## V. Support
Issues and questions can be addressed by contacting SAI30 Support by sending an email to support@sai360.com.
You must provide the following information for your question to be researched;
* The url that was used with the integration key blanked out for security purposes.
* The specific date and time of the posted data.
* The results that did not occur as expected.

Please note that without this information, your question cannot be addressed by our technical staff.
