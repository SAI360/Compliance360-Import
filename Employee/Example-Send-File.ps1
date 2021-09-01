param (
    [string]$FileToSend = $(throw "-FileToSend is required."),
    [string]$Url = $(throw "-Url is required."),
	[string]$FieldDelimiter = ""
)
[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
$upload= Invoke-WebRequest -Method Post -ContentType 'application/x-binary' -UseBasicParsing -InFile $FileToSend -Uri ($Url + (&{If($FieldDelimiter -ne "") {"&FieldDelimiter=" + $FieldDelimiter}}))