$cef_ignore = "cef_extensions.pak", "d3dcompiler_43.dll", "d3dcompiler_47.dll", "devtools_resources.pak",
    "libEGL.dll", "libGLESv2.dll"
$cef_locale = "en-US.pak", "ja.pak"
$locale_ignore = "de", "es", "fr", "it", "ko", "ru", "zh-Hans", "zh-Hant", "x86", "x64"

[Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")

function Remove-Directory($path) {
    Remove-Item -Path $path -Recurse -Force
}

function Compress-Archive2([string] $path, [string] $dest) {
    $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
    $includeBaseDir = $false
    [System.IO.Compression.ZipFile]::CreateFromDirectory($path, $dest, $compressionLevel, $includeBaseDir)
}

function Cleanup($path) {
    $origin = Get-Location
    if (-not (Test-Path -Path $path)) {
        return
    }
    Set-Location $path
    Remove-Item -Path "*.log", "*.xml", "*.pdb", "*.vshost.exe.config", "*.vshost.exe", "*.manifest"

    foreach ($file in $cef_ignore) {
        if (Test-Path -Path $file) {
            Remove-Item $file
        }
    }
    foreach ($dir in $locale_ignore) {
        if (Test-Path -Path $dir) {
            Remove-Directory $dir
        }
    }

    if (Test-Path -Path "locales") {
        Set-Location "locales"
        $files = Get-ChildItem
        foreach ($file in $files) {
            if (-not ($cef_locale -contains $file)) {
                Remove-Item $file
            }
        }
    }
    Set-Location $origin
}

function Copy-To($path1, $path2) {
    try {
        if (-not (Test-Path -Path $path1)) {
            return
        }
        if (-not (Test-Path -Path $path2)) {
            return
        }
        Copy-Item -Path "$path1\*" -Destination $path2 -Force
    } catch {
        # 
    }
}

function Process($path) {
    $origin = Get-Location
    try {    
        if (-not (Test-Path -Path $path)) {
            return
        }
        Set-Location $path
        Set-Location ".."
        if (Test-Path -Path "Norma.zip") {
            Remove-Item "Norma.zip"
        }
        $local = Get-Location
        # Compress-Archive -Path "Release\*" -DestinationPath "Norma.zip"
        Compress-Archive2 "$local\Release" "$local\Norma.zip"
    } catch {
        Write-Host "Error throwed"
    } finally {
        Set-Location $origin
    }
}

$ips_x64_dir = "Source\Norma.Ipsilon\bin\x64\Release"
$ips_x86_dir = "Source\Norma.Ipsilon\bin\x86\Release"
$iota_x64_dir = "Source\Norma.Iota\bin\x64\Release"
$iota_x86_dir = "Source\Norma.Iota\bin\x86\Release"
$x64_dir = "Source\Norma\bin\x64\Release"
$x86_dir = "Source\Norma\bin\x86\Release"

if (Test-Path -Path "$x64_dir\Norma.exe") {
    # x64
    Cleanup $ips_x64_dir
    Cleanup $iota_x64_dir

    Copy-To $ips_x64_dir $x64_dir
    Copy-To $iota_x64_dir $x64_dir
    Copy-Item -Path "Assemblies\x64\SQLite.Interop.dll" -Destination "$x64_dir\SQLite.Interop.dll"

    Cleanup $x64_dir
    Process $x64_dir
} else {
    # x86
    Cleanup $ips_x86_dir
    Cleanup $iota_x86_dir

    Copy-To $ips_x86_dir $x86_dir
    Copy-To $iota_x86_dir $x86_dir
    Copy-Item -Path "Assemblies\x86\SQLite.Interop.dll" -Destination "$x86_dir\SQLite.Interop.dll"


    Cleanup $x86_dir
    Process $x86_dir
}



