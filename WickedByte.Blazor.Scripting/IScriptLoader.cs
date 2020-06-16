using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace WickedByte.Blazor.Scripting
{
	public interface IScriptLoader
	{
		Task Load( string path );
		Task Load( Type type );
		Task Load<T>();
		Task Load( params Type[] types );
		Task LoadAll();
	}

	class ScriptLoader: IScriptLoader
	{
		#region Private

		private readonly List<string> _paths = new List<string>();
		private readonly List<string> _loadedPaths = new List<string>();
		private readonly IJSRuntime _js;
		private const string SCRIPT_LOADER_FUNCTION = "WickedByte.ScriptLoader.loadJS";
		private const string CSS_LOADER_FUNCTION = "WickedByte.ScriptLoader.loadCss";

		private string AddPath( string path )
		{
			if( string.IsNullOrWhiteSpace( path ) ) 
				return path;
			var scriptPath = path.ToLower();
			lock( _paths )
			{
				if( !_paths.Contains( scriptPath ) )
					_paths.Add( scriptPath );
				return path;
			}
		}

		#endregion

		#region Constructors

		public ScriptLoader( IJSRuntime js ) => _js = js;

		#endregion

		public void AddScripts( Type t )
		{
			foreach( var path in ScriptAttribute.GetPaths( t ) )
				AddPath( path );
		}

		public void AddScripts( IEnumerable<Type> types )
		{
			foreach( var t in types )
				AddScripts( t );
		}

		public void AddScripts( IEnumerable<string> paths )
		{
			foreach( var scriptPath in paths )
				AddPath( scriptPath );
		}

		public void AddScripts( Assembly assembly )
		{
			foreach( var script in ScriptAttribute.GetPaths( assembly ) )
				AddPath( script );
		}

		public void AddScripts( IEnumerable<Assembly> assemblies )
		{
			foreach( var assembly in assemblies )
				AddScripts( assembly );
		}

		public async Task Load( string path )
		{
			var script = path.ToLower();
			if( _loadedPaths.Contains( script ) ) return;
			try
			{
				_ = await _js.InvokeAsync<object>(
					script.EndsWith( "js" )
						? SCRIPT_LOADER_FUNCTION
						: CSS_LOADER_FUNCTION,
					path );
			}
			catch( Exception ex )
			{
				Debug.WriteLine( ex.Message );
				Debug.WriteLine( ex.StackTrace );
			}

			_loadedPaths.Add( script );
		}

		public async Task Load( Type type )
		{
			foreach( var path in ScriptAttribute.GetPaths( type ) )
				await Load( path ); 
		}

		public async Task Load<T>()
			=> await Load( typeof( T ) );

		public async Task Load( params Type[] types )
		{
			foreach( var type in types )
				await Load( type );
		}

		public async Task LoadAll()
		{
			try
			{
				_ = await _js.InvokeAsync<object>( 
					"eval", 
					Resource.ScriptLoader_min );

				for( var i = 0; i < _paths.Count; i++ )
					await Load( _paths[ i ] );
			}
			catch( Exception ex )
			{
				Debug.WriteLine( ex.Message );
				Debug.WriteLine( ex.StackTrace );
			}
		}

	}
}
