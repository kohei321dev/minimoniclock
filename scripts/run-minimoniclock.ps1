[CmdletBinding()]
param(
    [switch]$NoBuild,
    [switch]$Detached,
    [int]$StartupGraceSeconds = 3
)

$ErrorActionPreference = "Stop"

$RepoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$ProjectPath = Join-Path $RepoRoot "src\Minimoniclock\Minimoniclock.csproj"
$ExePath = Join-Path $RepoRoot "src\Minimoniclock\bin\Debug\net8.0-windows\Minimoniclock.exe"
$LogDirectory = Join-Path ([Environment]::GetFolderPath("LocalApplicationData")) "minimoniclock\logs"

function Fail($Message) {
    Write-Host "minimoniclock launcher error: $Message" -ForegroundColor Red
    exit 1
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Fail ".NET SDK 8 is required. Install it from https://dotnet.microsoft.com/download/dotnet/8.0"
}

$SdkList = & dotnet --list-sdks 2>$null
$HasDotnet8Sdk = $SdkList | Where-Object { $_ -match "^8\." }
if (-not $HasDotnet8Sdk) {
    Fail ".NET SDK 8 is required. Runtime-only installs cannot build this app."
}

if (-not (Test-Path $ProjectPath)) {
    Fail "Project file was not found: $ProjectPath"
}

if (-not $NoBuild) {
    Write-Host "Building minimoniclock..."
    & dotnet build $ProjectPath --nologo
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

if (-not (Test-Path $ExePath)) {
    Fail "Executable was not found after build: $ExePath"
}

Write-Host "Starting minimoniclock..."
Write-Host "App logs: $LogDirectory"

if ($Detached) {
    Start-Process -FilePath $ExePath -WorkingDirectory (Split-Path $ExePath -Parent)
    exit 0
}

$Stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$Process = Start-Process -FilePath $ExePath -WorkingDirectory (Split-Path $ExePath -Parent) -PassThru -Wait
$Stopwatch.Stop()

$ExitCode = $Process.ExitCode
if ($null -eq $ExitCode) {
    $ExitCode = 0
}

if ($Stopwatch.Elapsed.TotalSeconds -lt $StartupGraceSeconds) {
    Write-Host "minimoniclock closed after $([Math]::Round($Stopwatch.Elapsed.TotalSeconds, 1)) seconds." -ForegroundColor Yellow
    Write-Host "Check the app log directory above for startup errors." -ForegroundColor Yellow
    if ($ExitCode -eq 0) {
        exit 1
    }
}

exit $ExitCode
