# Migration PowerShell Script
# Usage: .\migrate.ps1

# Get current timestamp
$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$migrationName = "Migration_$timestamp"

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Oduyo.V2 Database Migration Script" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Migration Name: $migrationName" -ForegroundColor Yellow
Write-Host ""

# Set project paths
$startupProject = "Oduyo.Test\Oduyo.Test.csproj"
$dataAccessProject = "Oduyo.DataAccess\Oduyo.DataAccess.csproj"

# Check if EF Core tools are installed
Write-Host "Checking EF Core tools..." -ForegroundColor Green
$efToolsCheck = dotnet ef --version 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "EF Core tools not found. Installing..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to install EF Core tools." -ForegroundColor Red
        exit 1
    }
}

Write-Host "EF Core tools version: $efToolsCheck" -ForegroundColor Green
Write-Host ""

# Ask for confirmation to add migration
Write-Host "Do you want to create a new migration?" -ForegroundColor Yellow
Write-Host "Migration name will be: $migrationName" -ForegroundColor Cyan
$addMigration = Read-Host "Continue? (Y/N)"

if ($addMigration -ne "Y" -and $addMigration -ne "y") {
    Write-Host "Migration creation cancelled." -ForegroundColor Yellow
    exit 0
}

# Add migration
Write-Host ""
Write-Host "Creating migration '$migrationName'..." -ForegroundColor Green
dotnet ef migrations add $migrationName `
    --project $dataAccessProject `
    --startup-project $startupProject `
    --context ApplicationDbContext

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to create migration." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Migration created successfully!" -ForegroundColor Green
Write-Host ""

# Ask for confirmation to update database
Write-Host "Do you want to apply this migration to the database?" -ForegroundColor Yellow
Write-Host "WARNING: This will modify your database schema!" -ForegroundColor Red
$updateDatabase = Read-Host "Continue? (Y/N)"

if ($updateDatabase -ne "Y" -and $updateDatabase -ne "y") {
    Write-Host "Database update cancelled." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Migration '$migrationName' has been created but not applied to database." -ForegroundColor Cyan
    Write-Host "You can apply it later using:" -ForegroundColor Yellow
    Write-Host "  dotnet ef database update --project $dataAccessProject --startup-project $startupProject" -ForegroundColor Gray
    exit 0
}

# Update database
Write-Host ""
Write-Host "Applying migration to database..." -ForegroundColor Green
dotnet ef database update `
    --project $dataAccessProject `
    --startup-project $startupProject `
    --context ApplicationDbContext

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to update database." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Migration completed successfully!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Migration Name: $migrationName" -ForegroundColor Yellow
Write-Host "Database updated successfully." -ForegroundColor Green
