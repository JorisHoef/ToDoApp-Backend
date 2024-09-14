# Load environment variables from .env file
if (Test-Path .env) {
    Get-Content .env | ForEach-Object {
        if ($_ -match "^\s*([^#=]+?)\s*=\s*(.*)\s*$") {
            $name = $matches[1]
            $value = $matches[2]
            [System.Environment]::SetEnvironmentVariable($name, $value, 'Process')
            Write-Output "Set $name=$value"
        }
    }
    Write-Output "Environment variables loaded from .env"
} else {
    Write-Output ".env file not found"
}
