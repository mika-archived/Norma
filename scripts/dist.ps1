$cef_ignore = "cef_extensions.pak", "d3dcompiler_43.dll", "d3dcompiler_47.dll", "devtools_resources.pak",
    "libEGL.dll", "libGLESv2.dll"
$cef_locale = "en-US.pak", "ja.pak"
$locale_ignore = "de", "es", "fr", "it", "ko", "ru", "zh-Hans", "zh-Hant"

function Remove-Directory($path) {
    Remove-Item -Path $path -Recurse -Force
}

function Process($path) {
    $origin = Get-Location
    Set-Location $path
    Remove-Item -Path "*.log", "*.xml", "*.pdb", "*.config", "*.vshost.exe"

    foreach ($file in $cef_ignore) {
        Remove-Item $file
    }
    foreach ($dir in $locale_ignore) {
        Remove-Directory $dir
    }
    
    Set-Location "locales"
    $files = Get-ChildItem
    foreach ($file in $files) {
        if (-not ($cef_locale -contains $file)) {
            Remove-Item $file
        }
    }
    
    Set-Location "..\.."
    if (Test-Path -Path "Norma.zip") {
        Remove-Item "Norma.zip"
    }
    Compress-Archive -Path "Release\*" -DestinationPath "Norma.zip"
    
    Set-Location $origin
}

$x64_dir = "..\Norma\bin\x64\Release"
$x86_dir = "..\Norma\bin\x86\Release"

Process $x64_dir
Process $x86_dir