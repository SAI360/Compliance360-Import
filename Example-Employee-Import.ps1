$Uri = '[PASTE Batch Data Import Url HERE]'
$FilePath = 'Example-EmployeeManagement-Employee-data.csv'
[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
$upload= Invoke-WebRequest -Uri $Uri -Method Post -InFile $FilePath -ContentType 'application/x-binary'
