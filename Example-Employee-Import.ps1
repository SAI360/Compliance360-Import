$Uri = '[PASTE Batch Data Import Url HERE]'
$FilePath = 'Example-EmployeeManagement-Employee-data.csv'
$upload= Invoke-WebRequest -Uri $Uri -Method Post -InFile $FilePath -ContentType 'application/x-binary'