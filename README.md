# Blazor Scripting
Loads scripts and css dynamically in Blazor webassembly apps.

## Description
In the dotnet Blazor webassembly templates, you typically have to add '`link`' and '`script`' tags statically by editing `Index.html` every time you use a 3rd party component library. This library enables you to load Javascript and css at runtime, or configure what you want to load at startup via dependency injection by calling `builder.Services.AddScripts()` in `Program.Main()`, without having to edit `Index.html`.

You can create component libraries that automatically insert related scripts and css at startup via `Script` attributes.

Scripts and css can also be loaded at runtime on a per page basis via the `IScriptLoader` service.

## Installation
Use the Nuget package manager to install `WickedByte.Blazor.Scripting`.

## Usage

- [ ] At the top of `Program.cs`, add `using WickedByte.Blazor.Scripting;`
- [ ] In the `Main()` function, call `builder.Services.AddScripts()` or one of its overloads to inject the `IScriptLoader` service and configure which scripts / css files get loaded at startup.
- [ ] In your `MyComponent.razor` class, add `@using WickedByte.Blazor.Scripting` and `@inject IScriptLoader Scripts` to the top of the file.
- [ ] Call`Scripts.Load("[path to .js or .css file]")` (or one of its overloads) when you want to load the script or css, preferably within the  `OnAfterRenderAsync()` component lifecycle method.
- [ ] If you used `builder.Services.AddScripts()` to configure an initial set of scripts to be loaded at startup, you must edit `App.razor` and overload its `OnAfterRenderAsync()` method. Within the method, call `IScriptLoader.LoadAll()`.  When the app first loads, this will cause the browser to load all the scripts and css that you configured in `Program.Main()` via `builder.Services.AddScripts()`. All those scripts will subsequently become available to the child pages / components of your main `App` component.

## Script Loading via ScriptAttribute

You can decorate any of your classes or interfaces with the `[Script(Path='path to my script')]` attribute. When you call `IServiceCollection.AddScripts()` without any parameters, your assembly will be reflected for classes and interfaces that are decorated with the `[Script]` attribute, and the associated paths will be loaded when you first call `IScriptLoader.LoadAll()`.

Similarly, you can also call `IServiceCollection.AddScripts( config => config.Add( IEnumerable<Type | Assembly> items )  )` to cause other types or assemblies to be reflected for `[Script]` attributes. The associated paths will be loaded when you first call `IScriptLoader.LoadAll()`. 

If the `[Script]` attribute is applied to a class or interface without specifying a value for `Path`, then the associated script will be found by convention using the path `"_content/[class namespace]/[class name].js"`. For example,

```
`namespace WickedByte.Blazor{
	[Script] class MyExample{}
}`
```

would automatically be associated with the script path `"_content/WickedByte.Blazor/MyExample.js"`.

## License
[MIT](https://choosealicense.com/licenses/mit/)
