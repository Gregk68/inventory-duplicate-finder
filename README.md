# EQL Inventory Duplicate Finder

A Windows desktop utility for identifying duplicate inventory rows in exported inventory data.

## Features
- Parses inventory rows from CSV/text exports
- Detects duplicates and groups matches together
- Shows warnings for rows that do not fit the expected format
- Runs as a standalone Windows desktop app

## Run the app
Open the published executable in the `EQL-InventoryDuplicateFinder/bin/Release/net8.0-windows/win-x64/publish` folder, or build it with:

```powershell
dotnet publish "EQL-InventoryDuplicateFinder/InventoryDuplicateFinder.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## Notes
This repository includes the source code for the application and a compiled standalone Windows x64 release asset.
