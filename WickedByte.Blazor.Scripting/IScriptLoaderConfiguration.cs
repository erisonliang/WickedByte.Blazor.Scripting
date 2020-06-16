using System;
using System.Collections.Generic;
using System.Reflection;

namespace WickedByte.Blazor.Scripting
{
	/// <summary>
	/// Configuration for loading scripts.
	/// </summary>
	public interface IScriptLoaderConfiguration
	{
		/// <summary>
		/// Adds types whose scripts / css will be loaded into the web page.
		/// </summary>
		void Add( IEnumerable<Type> types );

		/// <summary>
		/// Adds types whose scripts / css will be loaded into the web page.
		/// </summary>
		void Add( params Type[] types );

		/// <summary>
		/// Adds paths whose scripts / css will be loaded into the web page.
		/// </summary>
		void Add( IEnumerable<string> paths );
		/// <summary>
		/// Adds paths whose scripts / css will be loaded into the web page.
		/// </summary>
		void Add( params string[] paths );
		/// <summary>
		/// Adds assemblies that will be interrogated for the <see cref="ScriptAttribute"/>.
		/// </summary>
		void Add( IEnumerable<Assembly> assemblies );
		/// <summary>
		/// Adds assemblies that will be interrogated for the <see cref="ScriptAttribute"/>.
		/// </summary>
		void Add( params Assembly[] assemblies );
	}

	/// <summary>
	/// Configuration for loading scripts.
	/// </summary>
	class ScriptLoaderConfiguration : IScriptLoaderConfiguration
	{
		public List<Type> Types { get; } = new List<Type>();
		public List<string> Paths { get; } = new List<string>();
		public List<Assembly> Assemblies { get; } = new List<Assembly>();

		public void Add( IEnumerable<Type> types )
			=> Types.AddRange( types );

		public void Add( params Type[] types )
			=> Types.AddRange( types );

		public void Add( IEnumerable<string> paths )
			=> Paths.AddRange( paths );

		public void Add( params string[] paths )
			=> Paths.AddRange( paths );

		public void Add( IEnumerable<Assembly> assemblies )
			=> Assemblies.AddRange( assemblies );

		public void Add( params Assembly[] assemblies )
			=> Assemblies.AddRange( assemblies );

		public void AddScripts( ScriptLoader loader )
		{
			loader.AddScripts( Types );
			loader.AddScripts( Assemblies );
			loader.AddScripts( Paths );
		}
	}
}
