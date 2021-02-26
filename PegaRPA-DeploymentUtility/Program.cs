using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PegaRPA.DeploymentUtility
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
		{
			Assembly result = null;

			try
			{
				var fileName = $"{args.Name.Split(',')[0]}.dll";
				result = LoadEmbeddedAssembly(fileName);

				// TODO: Log information about result to the console.
			}
			catch (Exception e)
			{
				// TODO: Log information about the exception.
				result = null;
			}

			return result;
		}

		private static Assembly LoadEmbeddedAssembly(string assemblyFileName)
		{
			Assembly result = null;

			var resourcePath = typeof(Program).Assembly.GetManifestResourceNames().FirstOrDefault(n => n == $"{ResourceBasePaths.Assemblies}.{assemblyFileName}");

			if (!string.IsNullOrWhiteSpace(resourcePath))
			{
				using (var assemblyStream = typeof(Program).Assembly.GetManifestResourceStream(resourcePath))
				{
					var assemblyData = new byte[assemblyStream.Length];
					assemblyStream.Read(assemblyData, 0, assemblyData.Length);
					result = Assembly.Load(assemblyData);
				};
			}
			else
			{
				// TODO: Make a log entry stating that the assembly was not found in embedded assemblies.
			}

			return result;
		}
	}
}
