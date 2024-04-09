param (
	[ValidateSet("none", "server", "webassembly", "auto", "all")]
    [Parameter(Mandatory=$true,
			   HelpMessage="Type of interactivity: none, server, webassembly or all")][string]$interactivity, 
	[ValidateSet("none", "individual", "all")]
    [Parameter(Mandatory=$true,
			   HelpMessage="Type of authentication: none, individual or all")][string]$auth
 )

#dotnet new sln -n FluentUI.TemplateValidation --force

if ($auth -eq "none" -or $auth -eq "all")
{
	if ($interactivity -eq "none" -or $interactivity -eq "all")
	{
		# Interactivity: None (SSR) / Auth: None / All Interactive: not available  when Interactivity = None
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\A-SSR --interactivity None --auth None --force
	}

	if ($interactivity -eq "server" -or $interactivity -eq "all")
	{
		# Interactivity: Server / Auth: None / All Interactive: True
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\B-Server\Global --interactivity server --auth None --all-interactive true --force

		# Interactivity: Server / Auth: None / All Interactive: False
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\B-Server\PerPage --interactivity server --auth None --all-interactive false --force
	}

	if ($interactivity -eq "webassembly" -or $interactivity -eq "all")
	{
		# Interactivity: WebAssembly / Auth: None / All Interactive: True
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\C-Wasm\Global --interactivity webassembly --auth None --all-interactive true --force

		# Interactivity: WebAssembly / Auth: None / All Interactive: False
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\C-Wasm\PerPage --interactivity webassembly --auth None --all-interactive false --force
	}

	if ($interactivity -eq "auto" -or $interactivity -eq "all")
	{
		# Interactivity: Auto / Auth: None / All Interactive: True
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\D-Auto\Global --interactivity auto --auth None --all-interactive true --force

		# Interactivity: Auto / Auth: None / All Interactive: False
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o NoAuthentication\D-Auto\PerPage --interactivity auto --auth None --all-interactive false --force
	
		# Interactivity: Auto / Auth: None / All Interactive: True / Empty: true
		dotnet new fluentblazor -n FluentUI.TemplateValidation.Auto -o NoAuthentication\D-Auto\NoSamplePages --interactivity auto --auth None --all-interactive true --empty true --force
	}
}

if ($auth -eq 'individual' -or $auth -eq 'all' ) 
{
	if ($interactivity -eq "none" -or $interactivity -eq "all")
	{
		# Interactivity: None (SSR) / Auth: Individual / All Interactive: not available  when Interactivity = None
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\A-SSR --interactivity None --auth Individual --force
	}

	if ($interactivity -eq "server" -or $interactivity -eq "all")
	{
		# Interactivity: Server / Auth: Individual / All Interactive: False
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\B-Server\Global --interactivity server --auth Individual --all-interactive true --force

		# Interactivity: Server / Auth: Individual / All Interactive: True
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\B-Server\PerPage --interactivity server --auth Individual --all-interactive false --force
	}

	if ($interactivity -eq "webassembly" -or $interactivity -eq "all")
	{
		# Interactivity: WebAssembly / Auth: Individual / All Interactive: False
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\C-Wasm\Global --interactivity webassembly --auth Individual --all-interactive true --force

		# Interactivity: WebAssembly / Auth: Individual / All Interactive: True
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\C-Wasm\PerPage --interactivity webassembly --auth Individual --all-interactive false --force
	}

	if ($interactivity -eq "auto" -or $interactivity -eq "all")
	{
		# Interactivity: Auto / Auth: Individual / All Interactive: True
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\D-Auto\Global --interactivity auto --auth Individual --all-interactive true --force

		# Interactivity: Auto / Auth: Individual / All Interactive: False
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\D-Auto\PerPage --interactivity auto --auth Individual --all-interactive false --force

		# Interactivity: Auto / Auth: Individual / All Interactive: True / Empty: true
		dotnet new fluentblazor -n FluentUI.TemplateValidation -o IndividualAccounts\D-Auto\NoSamplePages --interactivity auto --auth Individual --all-interactive true --empty true --force
	}
}

# Wasm Standalone
dotnet new fluentblazorwasm -n FluentUI.TemplateValidation -o WasmStandalone\NoSamplePages --empty true --force
dotnet new fluentblazorwasm -n FluentUI.TemplateValidation -o WasmStandalone\IndividualAccounts --auth individual --force
dotnet new fluentblazorwasm -n FluentUI.TemplateValidation -o WasmStandalone\NoAuthentication --auth none --force