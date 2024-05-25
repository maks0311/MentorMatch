#via single command (run PowerShell as Administrator)


# Script register new Event-Source with name given as a parameter.
# Script register new Event-Source with name given as a variable below (sourceName).


$sourceName="Mentor"

try {
New-EventLog -LogName Application  -Source "$args" -ErrorAction Stop
    Write-Host "EventLog [$args] registration Successful"
New-EventLog -LogName Application  -Source $sourceName -ErrorAction Stop
    Write-Host "EventLog [$sourceName] registration Successful"
}
catch {
try{
   Get-Item -Path HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\$args -ErrorAction Stop
   }
   catch
   {
     Write-Host "An error occurred that could not be resolved."
   }
}