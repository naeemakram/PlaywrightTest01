# Introduction to PlayWright
I'm starting to learn Playwright. It is a promising test automation tool.
The great thing is that you can use many programming languages to use PlayWright. 
The languages supported by the Playwright are given below:
1. JavaScript
2. Java
3. C#
4. Python
 
  PlayWright also provides excellent tools for debugging tests in real-time. You can also produce test run recordings and logs with detailed screenshots.
Other than UI testing, you can also test APIs with Playwright. 

 ## How to enable debugging through PowerShell:
  
 `$env:PWDEBUG=1`
 ### How to disable debugging through PowerShell:
 `$env:PWDEBUG=0`
 ### How to see trace (if enabled) through PowerShell:
`.\playwright.ps1 show-trace`
 ### How to enable headless mode through Powershell: 
`$env:HEADED="1"`

 ### How to disable headless mode through Powershell: 
`$env:HEADED="0"`

### How to run tests with specific run settings file
`dotnet test --settings:chromium.runsettings`

## Important note about running tests via GitHub Actions/Azure, etc.
  Running on Ubuntu VMs is headless mode, which is supported by default.
  To run non-headless mode, you must set up X GUI. Extra frills!

## How to launch code-generator
` .\bin\debug\net6.0\playwright.ps1 codegen google.com`
## How to launch trace viewer
`.\bin\Debug\net6.0\playwright show-trace .\bin\Debug\net6.0\playwright-traces\PlaywrightTest01.PlaywrightAutomation01.OpenGoogleAndSearchTest.zip`
