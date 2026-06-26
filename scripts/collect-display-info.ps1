[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"

function Write-Section($Title) {
    Write-Host ""
    Write-Host "## $Title"
}

function Convert-MonitorName($WmiString) {
    if ($null -eq $WmiString) {
        return ""
    }

    $chars = foreach ($charCode in $WmiString) {
        if ($charCode -ne 0) {
            [char]$charCode
        }
    }

    -join $chars
}

Write-Host "# minimoniclock display diagnostics"
Write-Host "CollectedAt: $((Get-Date).ToString("yyyy-MM-dd HH:mm:ss zzz"))"

Write-Section "Windows"
$os = Get-CimInstance Win32_OperatingSystem
[PSCustomObject]@{
    Caption = $os.Caption
    Version = $os.Version
    BuildNumber = $os.BuildNumber
} | Format-List

Write-Section "System DPI"
$desktopKey = "HKCU:\Control Panel\Desktop\WindowMetrics"
$appliedDpi = (Get-ItemProperty -Path $desktopKey -Name AppliedDPI -ErrorAction SilentlyContinue).AppliedDPI
if ($null -ne $appliedDpi) {
    $scalePercent = [Math]::Round(($appliedDpi / 96) * 100)
    [PSCustomObject]@{
        AppliedDPI = $appliedDpi
        EstimatedScalePercent = $scalePercent
    } | Format-List
}
else {
    Write-Host "AppliedDPI was not available."
}

Write-Section "Screens"
Add-Type -AssemblyName System.Windows.Forms
[System.Windows.Forms.Screen]::AllScreens | ForEach-Object {
    [PSCustomObject]@{
        DeviceName = $_.DeviceName
        Primary = $_.Primary
        Bounds = "$($_.Bounds.Width)x$($_.Bounds.Height)+$($_.Bounds.X)+$($_.Bounds.Y)"
        WorkingArea = "$($_.WorkingArea.Width)x$($_.WorkingArea.Height)+$($_.WorkingArea.X)+$($_.WorkingArea.Y)"
        BitsPerPixel = $_.BitsPerPixel
    }
} | Format-Table -AutoSize

Write-Section "Monitor IDs"
Get-CimInstance -Namespace root\wmi -ClassName WmiMonitorID -ErrorAction SilentlyContinue | ForEach-Object {
    [PSCustomObject]@{
        InstanceName = $_.InstanceName
        Manufacturer = Convert-MonitorName $_.ManufacturerName
        ProductCode = Convert-MonitorName $_.ProductCodeID
        UserFriendlyName = Convert-MonitorName $_.UserFriendlyName
    }
} | Format-Table -AutoSize

Write-Section "Video Controllers"
Get-CimInstance Win32_VideoController | ForEach-Object {
    [PSCustomObject]@{
        Name = $_.Name
        CurrentHorizontalResolution = $_.CurrentHorizontalResolution
        CurrentVerticalResolution = $_.CurrentVerticalResolution
        CurrentRefreshRate = $_.CurrentRefreshRate
        DriverVersion = $_.DriverVersion
    }
} | Format-Table -AutoSize

Write-Section "Manual Checks"
Write-Host "- Confirm Windows display scale in Settings > System > Display."
Write-Host "- Confirm the JN-MD-IPS7842 logical size in portrait and landscape."
Write-Host "- Run minimoniclock with -WindowSize 400x1280 and -WindowSize 1280x400 for representative layout checks."
