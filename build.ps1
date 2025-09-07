New-Item -ItemType Directory -Path ./BuildOutput -Force | Out-Null

$runtimes = @(
    "win-x64",
    "linux-x64",
    "osx-x64",
    "linux-arm64"
)

pushd ./Stub
$runtimes | ForEach-Object {
    Write-Host "Building stub for runtime: $_" -ForegroundColor Green
    dotnet publish -c Release -r $_ --self-contained true -o ../BuildOutput/$_ /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true
}
popd

pushd ./Builder
$runtimes | ForEach-Object {
    Write-Host "Building builder for runtime: $_" -ForegroundColor Green
    dotnet publish -c Release -r $_ --self-contained true -o ../BuildOutput/$_ /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true
}
popd