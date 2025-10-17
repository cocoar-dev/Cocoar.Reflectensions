#!/usr/bin/env pwsh
# Run tests with code coverage and generate reports

param(
    [switch]$SkipReport = $false
)

$ErrorActionPreference = "Stop"

Write-Host "Running tests with code coverage..." -ForegroundColor Cyan

# Clean previous coverage results
$coverageDir = "TestResults"
if (Test-Path $coverageDir) {
    Remove-Item $coverageDir -Recurse -Force
    Write-Host "Cleaned previous coverage results" -ForegroundColor Yellow
}

# Run tests with coverage
dotnet test src/Cocoar.Reflectensions.Tests/Cocoar.Reflectensions.Tests.csproj `
    --configuration Release `
    --collect:"XPlat Code Coverage" `
    --results-directory $coverageDir `
    --settings coverlet.runsettings `
    -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Tests completed successfully!" -ForegroundColor Green

if (-not $SkipReport) {
    # Check if reportgenerator is installed
    $reportGenerator = Get-Command reportgenerator -ErrorAction SilentlyContinue
    if (-not $reportGenerator) {
        Write-Host "Installing ReportGenerator tool..." -ForegroundColor Yellow
        dotnet tool install --global dotnet-reportgenerator-globaltool
    }

    # Find coverage file
    $coverageFile = Get-ChildItem -Path $coverageDir -Filter "coverage.opencover.xml" -Recurse | Select-Object -First 1

    if ($coverageFile) {
        Write-Host "Generating coverage report..." -ForegroundColor Cyan
        
        $reportDir = "TestResults/CoverageReport"
        reportgenerator `
            -reports:"$($coverageFile.FullName)" `
            -targetdir:"$reportDir" `
            -reporttypes:"Html;TextSummary" `
            -assemblyfilters:"+Cocoar.Reflectensions*"

        Write-Host "`nCoverage Report Generated!" -ForegroundColor Green
        Write-Host "Open: $reportDir/index.html" -ForegroundColor Cyan

        # Display summary
        $summaryFile = "$reportDir/Summary.txt"
        if (Test-Path $summaryFile) {
            Write-Host "`n=== Coverage Summary ===" -ForegroundColor Cyan
            Get-Content $summaryFile
        }
    } else {
        Write-Host "Warning: Coverage file not found!" -ForegroundColor Yellow
    }
} else {
    Write-Host "Skipping report generation (use without -SkipReport to generate)" -ForegroundColor Yellow
}
