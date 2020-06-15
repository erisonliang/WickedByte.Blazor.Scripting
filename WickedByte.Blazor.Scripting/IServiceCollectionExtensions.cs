using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Reflection;

namespace WickedByte.Blazor.Scripting
{
	public static class IServiceCollectionExtensions
	{
		#region Private

		private static readonly ScriptLoaderConfiguration _config = new ScriptLoaderConfiguration();

		private static IServiceCollection AddScriptLoader( 
			IServiceCollection services,
			ScriptLoaderConfiguration config )
			=> services.AddSingleton<IScriptLoader, ScriptLoader>( context =>
			{
				var loader = new ScriptLoader( context.GetRequiredService<IJSRuntime>() );
				if( config != null ) config.AddScripts( loader );
				return loader;
			} );

		#endregion

		/// <summary>
		/// Adds a dynamic Javascript loader.
		/// </summary>
		public static IServiceCollection AddScripts(
			this IServiceCollection services,
			Action<IScriptLoaderConfiguration>? func = null ) 
		{

			if( func == null )
				_config.Assemblies.Add( Assembly.GetCallingAssembly() );		
			else func.Invoke( _config );
			AddScriptLoader( services, _config );
			return services;
		}
	}
}
