# Employee Batch Import
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
2. Compile a list of the desired import fields. Best practice is to review the actual Edit form of the importing component (i.e. Employee).
3. Only include fields for which you have relevant data to import. There's no need to import all C360 fields such that every item imports an empty value for a field. 
4. Use field names in your input file that match the C360 field names. Doing this makes it easier to maintain by avoiding field mapping. Use these tips to find the names;
* Navigate to Maintenance -> Forms Configuration, choose the module and component and edit the appopriate form (e.g. 'Edit Employees'). 
* Obtain the exact spelling of the field by placing the mouse cursor over the caret to the right of the field and waiting 2-3 seconds. The exact field name will appear. This is the name to use in the first row of your import file.
* Do not assume that the field label is the field name as they can be (and often are) different. 
* Note that a full list of fields can be found under the Maintenance -> Modules menu and selecting the appropriate component and choosing Component Fields. Use the Name value and NOT the System Name value.
5. It is unnessary to include fields with values that are the same for every record (e.g. Division, workflow template). A single Default Field Mapping for all items can be configured instead.
6. File size can range from 1 to 100,000 records (larger available) and submitted at any reasonable frequency. Some examples; 100,000 weekly, 50,000 daily, 1 every minute (approx.). Each submittal is queued and typically begins processing within seconds. 

Name the input file as you prefer. Once posted, it is immediately read and queued to process.  

### Example Files
The following example files are not necessarily complete to a given client's needs but should work as an initial test and can then be used as a guide for building-out the desired input file;

| Format | Description |
| ------ | ----------- |
| Excel | The file [Example-EmployeeManagement-Employee-data.xlsx](Example-EmployeeManagement-Employee-data.xlsx) contains sample Employee fields and data. |
| Delimited | The file [Example-EmployeeManagement-Employee-data.csv](Example-EmployeeManagement-Employee-data.csv) contains sample Employee fields and data, This is a comma-delimited file (field values are seperated by commas) but any delimiter can be used if the url contains a &FieldDelimiter= parameter identifying the desired delimiter. | 
| JSON | Example available upon request.

## II. Configure Integration

1. Client Organization's admin must create an Integration definition within the Compliance 360 (C360) website. This is located under the Maintenance -> Integrations menu. Choose Integration Type of 'Employee'. Ensure that the Host Account chosen has 'API Access' checked under the Module Access tab as well as all necessary permissions to create and update the intended data.
2. In the Field Mappings tab of the Integration definition create an entry that designates the one (1) C360 Field that will be the unique identifier which will determine whether an imported item is added vs. updated. Recommended:

| C360 MODULE - COMPONENT | C360 FIELD | EXTERNAL FIELD | DEFAULT VALUE | IDENTIFIER |
| --------------------------------------- | ----------------------------------- | ---------------------------------- | ------------------ | - |
| Employee Management - Employee | Number | (field from import file) | (leave empty) | Checked |

3. In the Field Mappings tab of the Integration definition, create mapping defaults for fields whose values are not supplied in the import file. Recommended are:

| C360 MODULE - COMPONENT | C360 FIELD | EXTERNAL FIELD | DEFAULT VALUE |
| -------------------------------------- | ------------------------- | ------------------ | ------------------------------------------------------------------ |
| Employee Management - Employee | Primary Division | (leave empty) | (select the Division where new employees should be created. If not supplied then the primary division of the Host Account that is defined with the Integration will be used.) |
| Employee Management - Employee | Workflow Template Id | (leave empty) | (select the Worflow Template to be used when new employees are created. If not supplied then the default workflow template, as shown in workflow template maintenance, will be used.) |

4. In the Field Mappings tab of the Integration definition create additional Mappings as needed. For instance, if field names in the import file cannot be made to match C360 field names, you can add mappings to designate the corresponding field name in the input file (External Field).

5. If importing an Employee Relationship it must be imported at the same time as the Employee information and from a field in the employee input file record. Each relationship should appear in the employee input file record as a separate field which holds the identifier value of the related employee. For example; an Employee import file field named 'Manager' would hold the identifier value of the employee's manager and a field named 'Supervisor' would hold the identifier value of the employee's supervisor. Field Mappings are then required to import these properly. Here is how to enter the mappings for this example (note that mapping entries for Type must have the Identifer field checked); ![Employee Relationship Mapping](Images/EmployeeRelationshipMapping.PNG)

## III. Testing
All imports should be thoroughly tested and the results reviewed before importing to production data. This can be done by;
1. Arranging for a test database to be created by contacting SAI support (support@sai360.com).
2. Post the data to the test site (see Posting Data below).
3. Monitor the progress of the job via the C360 Home -> Job Status menu. Cllck on the Magnifying glass on the relevant entry to see a detailed summary of the import.
4. Review results via C360 Forms, Reporting and Audit Trail.

## IV. Posting Data

The file named 'Example-Employee-Import.cmd' is a simple command file that demonstrates posting the import file directly to Compliance360 and queueing it for import. It does this by invoking the simple PowerShell script named 'Example-Send-File.ps1'.
1. If not already downloaded, download this GitHub repository ([https://github.com/SAIGlobal/Compliance360-Import](https://github.com/SAIGlobal/Compliance360-Import)) by clicking on the Code button. 
2. Locate the Command file named 'Example-Employee-Import.cmd' and open in an editor such as Notepad or Notepad++. <br/>
  2a. Edit line 2 to supply the name of the file being imported. Do not remove the caret (^) at the end of the line. <br/>
  2b. Edit line 3 to supply the url that is copied from the Integration definition. Do not remove the caret (^) at the end of the line. <br/>
  2c. Edit line 4 if the import file is delimited and uses a field delimiter other than a comma(,). Specify the delimiter used.
3. Place the input file in the same folder as the command file file.
4. Double-clicking on the command file 'Example-Employee-Import.cmd' will immediately post the input file to C360 via the PowerShell script and queue it for import. 
5. Job status can then be viewed in C360 under the Home -> Job Status menu. Click on the Magnifying glass on the relevant entry to see a detailed summary of the import.

NOTE: ALL files contained here are intended as examples only of the Batch API call and do not constitute supportable C360 application components.

Additional methods for posting the input file to C360 are as follows;
* The input file can be posted directly from several commercial products (WorkDay, etc.). 
* If developing a custom application, the following example demonstrates a .NET implementation. [Compliance360.Import](https://github.com/SAIGlobal/Compliance360-Import/tree/master/Compliance360.Import)

## V. Support
Issues and questions can be addressed by contacting SAI30 Support by sending an email to support@sai360.com.
You must provide the following information for your question to be researched;
* The url that was used (with the integration key blanked out for security purposes).
* The data or file that was posted.
* The specific date and time of the posted data.
* The results that did not occur as expected.

Please note that without this information, your question cannot be fully addressed by our technical staff.
