# README

## Requirements

1. .NET 4.5
2. PowerShell
3. Network capability to pull latest nuget packages

## Config

- At run time `PowerReport.Runner.exe.config`. The release folder of `PowerReport.Runner`. This will usually be at `PowerReport\PowerReport.Runner\bin\{Debug|Release}`
- At design time `PowerReport.Runner\App.config`. This is in the `PowerReport.Runner` project

```xml
<appSettings>
    <add key="MaxTimeout" value="00:00:45" />
    <add key="LogLocation" value="C:\PowerPosition" />
    <add key="ReportLocation" value="C:\PowerPosition" />
    <add key="Cron" value="* * * * *" />
</appSettings>
```

- `MaxTimeout` is a maximum period allowed for the attempt to retrieve trades from the lib by coding task provider.
  **NOTE:** this should be in relation to the specified `Cron` value
- `LogLocation` is a destination folder for logs
- `ReportLocation` is a destination folder for a csv report generated during an extract
- `Cron` is a scheduling configuration for repeated runs of an extract. See more details [here](https://en.wikipedia.org/wiki/Cron). Current value of `* * * *` represents a schedule of every minute

## Deployment steps

You can select either of 2 options:

### Script

1. Review configuration
2. Start a `PowerShell` terminal as admin in the folder `.deploy` at the root level within `PowerReport` repo
3. Run `deploy.ps1` script

   ```PowerShell
   .\deploy1.ps
   ```

4. Review configured folders in `LogLocation` and `ReportLocation` for outcomes
5. Stop and cleanup installed service

   ```PowerShell
   .\cleanup.ps1
   ```

### Manual

1. Review configuration
2. Restore nuget packages and build the solution with desired release configuration `Debug` or `Release`
3. Navigate to the release folder of `PowerReport.Runner`. This will usually be at `PowerReport\PowerReport.Runner\bin\{Debug|Release}`
4. Start a `PowerShell` terminal as admin in the folder
5. Run exe program to install the service

   ```PowerShell
   .\PowerReport.Runner.exe install
   ```

6. Start service either in the `Services` app and service named `Power Position Extractor`, or by running `PowerShell` command

   ```PowerShell
   Start-Service -Name "PowerPositionExtractor"
   ```

7. Review configured folders in `LogLocation` and `ReportLocation` for outcomes
8. Stop and cleanup installed service

   ```PowerShell
   .\PowerReport.Runner.exe uninstall
   ```
