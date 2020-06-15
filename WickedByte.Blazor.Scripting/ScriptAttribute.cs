using System;
using System.Collections.Generic;
using System.Reflection;

namespace WickedByte.Blazor.Scripting
{
	/// <summary>
	/// Can be applied to classes to associate a script to be loaded
	/// into the page at runtime.
	/// </summary>
	[AttributeUsage( 
		AttributeTargets.Class | AttributeTargets.Interface, 
		Inherited = false, 
		AllowMultiple = true )]
	public class ScriptAttribute : Attribute
	{
		#region Private

		private static ScriptAttribute[] GetScriptAttributes( Type type )
			=> (ScriptAttribute[])GetCustomAttributes( type, typeof( ScriptAttribute ) );

		private static string FormatPath( Type t )
		{
			var ns = t.FullName.Substring( 0, t.FullName.LastIndexOf( '.' ) );
			return $"_content/{ns}/{t.Name}.js";
		}

		private static string FormatPath( Type t, ScriptAttribute script )
		{
			if( script != null && !string.IsNullOrWhiteSpace( script.Path ) &&
				script.Path.IndexOf('/') > 0 ) 
				return script.Path;

			var ns = t.FullName.Substring( 0, t.FullName.LastIndexOf( '.' ) );
			return script.Path[0] != '/'
				? $"_content/{ns}/{script.Path}"
				: $"_content/{ns}{script.Path}";
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the path of the script to load.
		/// </summary>
		public string? Path{ get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of a <see cref="ScriptAttribute"/>/
		/// </summary>
		/// <param name="path">Path to the script file.</param>
		public ScriptAttribute( string path )
			=> Path = path;

		/// <summary>
		/// Creates a new instance of a <see cref="ScriptAttribute"/>.
		/// </summary>
		public ScriptAttribute() { }

		#endregion

		/// <summary>
		/// Gets script paths associated with a type.
		/// </summary>
		/// <param name="t">The type for which script paths will
		/// be retrieved.</param>
		/// <param name="scriptAttributesOnly">Only retrieve paths
		/// for types decorated with a <see cref="ScriptAttribute"/>.</param>
		/// <returns></returns>
		internal static IEnumerable<string> GetPaths( 
			Type t, 
			bool scriptAttributesOnly = false )
		{
			var attributes = GetScriptAttributes( t );
			if( attributes != null && attributes.Length > 0 )
				for( var i = 0; i < attributes.Length; i++ )
				{
					var path = attributes[ i ];
					yield return !string.IsNullOrWhiteSpace( path.Path )
						? FormatPath( t, path )
						: FormatPath( t );
				}
			else if( !scriptAttributesOnly ) 
				yield return FormatPath( t );

			yield break;	
		}

		/// <summary>
		/// Gets all the script paths found in an <see cref="Assembly"/>
		/// based on classes decorated with a <see cref="ScriptAttribute"/>/
		/// </summary>
		internal static IEnumerable<string> GetPaths( Assembly assembly )
		{
			foreach( Type t in assembly.GetTypes() )
			foreach( var path in GetPaths( t, true ) )
				yield return path;			
			yield break;
		}
	}
}
