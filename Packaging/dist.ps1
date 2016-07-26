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
        Copy-Item -Path "$path1\*" -Destination $path2 -Force -Recurse
    } catch {
        # 
    }
}

function Process($path, $dest) {
    $origin = Get-Location
    try {    
        if (-not (Test-Path -Path $path)) {
            return
        }
        Set-Location $path
        Set-Location ".."
        # if (Test-Path -Path "Norma.zip") {
        #     Remove-Item "Norma.zip"
        # }
        $local = Get-Location
        # Compress-Archive -Path "Release\*" -DestinationPath "Norma.zip"
        Compress-Archive2 "$local\Release" "$local\$dest"
        Write-Host "Output to $local\$dest"
    } catch {
        Write-Host $error[0].exception
    } finally {
        Set-Location $origin
    }
}

if (Test-Path -Path "Source\Norma\bin\x64\Release") {
    # x64
    $arch = "x64"
} else {
    # x86
    $arch = "x86"
}

$artifact = "Norma_$($arch)_$($env:APPVEYOR_BUILD_VERSION).zip"

$ips_dir = "Source\Norma.Ipsilon\bin\$arch\Release"
$iota_dir = "Source\Norma.Iota\bin\$arch\Release"
$main_dir = "Source\Norma\bin\$arch\Release"
$bin_dir = "Source\Norma\bin\$arch"

Cleanup $ips_dir
Cleanup $iota_dir

Copy-To $ips_dir $main_dir
Copy-To $iota_dir $main_dir
Copy-Item -Path "Assemblies\$arch\SQLite.Interop.dll" -Destination "$main_dir\SQLite.Interop.dll"

Cleanup $main_dir
Process $main_dir $artifact
Push-AppveyorArtifact "$bin_dir\$artifact" -FileName $artifact
