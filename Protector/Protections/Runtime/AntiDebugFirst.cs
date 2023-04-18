using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OliviaGuard.Protector.Protections.Runtime
{
    internal static class AntiDebugFirst
    {
     
        static void Initialize()
        {
            Task.Run(() =>
            {

				Run();
            });
        }
		internal static bool ValueType()
		{
			bool flag = !File.Exists(Environment.ExpandEnvironmentVariables("%appdata%") + "\\dnSpy\\dnSpy.xml");
			return !flag;
		}
		internal static bool Valoo()
		{
				bool isAttached = Debugger.IsAttached;
				try
				{
					bool flag = false;
					ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
					foreach (object obj in modules)
					{
						ProcessModule processModule = (ProcessModule)obj;
						if (processModule.ModuleName.EndsWith("Decompiler.dll", StringComparison.CurrentCulture))
						{
							flag = true;
						}
						if (processModule.ModuleName.EndsWith("ILSpy.x.dll", StringComparison.CurrentCulture))
						{
							flag = true;
						}
					}
					return isAttached && flag;
				}
				catch (Exception ex)
				{
					Environment.Exit(-1);
				}
				return isAttached;
			
		}
		private static bool ZZZZZZZZZX()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}
		[DllImport("kernel32.dll")]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x060007DB RID: 2011
		[DllImport("kernel32.dll")]
		private static extern bool IsDebuggerPresent();

		// Token: 0x060007DC RID: 2012
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int OutputDebugString(string str);

		// Token: 0x060007DD RID: 2013
		[DllImport("Kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetCurrentThread();

		// Token: 0x060007DE RID: 2014
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

		// Token: 0x060007DF RID: 2015
		[DllImport("Ntdll.dll", SetLastError = true)]
		private static extern uint NtSetInformationThread(IntPtr hThread, int ThreadInformationClass, IntPtr ThreadInformation, uint ThreadInformationLength);

		// Token: 0x060007E0 RID: 2016
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, IntPtr processInformation, uint processInformationLength, IntPtr returnLength);

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002CE20 File Offset: 0x0002B020
		private static void Worker(object thread)
		{
			Thread th = thread as Thread;
			if (th == null)
			{
				th = new Thread(new ParameterizedThreadStart(Worker));
				th.IsBackground = true;
				th.Start(Thread.CurrentThread);
				Thread.Sleep(500);
			}
			for (; ; )
			{
				if (Debugger.IsAttached || Debugger.IsLogging())
				{
					Environment.Exit(0);
				}
				bool present = false;
				CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref present);
				if (present)
				{
					Environment.Exit(0);
				}
				uint Status = NtSetInformationThread(GetCurrentThread(), 17, IntPtr.Zero, 0U);
				if (Status != 0U)
				{
					Environment.Exit(0);
				}
				IntPtr NoDebugInherit = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
				int status2 = NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 31, NoDebugInherit, 4U, IntPtr.Zero);
				if ((uint)Marshal.PtrToStructure(NoDebugInherit, typeof(uint)) == 0U)
				{
					Environment.Exit(0);
				}
				IntPtr hDebugObject = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
				if (NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 30, hDebugObject, 4U, IntPtr.Zero) == 0)
				{
					Environment.Exit(0);
				}
				if (IsDebuggerPresent())
				{
					Environment.Exit(0);
				}
				Process ps = Process.GetCurrentProcess();
				if (ps.Handle == IntPtr.Zero)
				{
					Environment.Exit(0);
				}
				ps.Close();
				if (OutputDebugString("") > IntPtr.Size)
				{
					Environment.Exit(0);
				}
				if (!th.IsAlive)
				{
					Environment.Exit(0);
				}
				Thread.Sleep(1000);
			}
		}
		internal static bool Run()
		{
			string x = "COR";
			Type env = typeof(Environment);
			MethodInfo method = env.GetMethod("GetEnvironmentVariable", new Type[] { typeof(string) });
			if (method != null && "1".Equals(method.Invoke(null, new object[] { x + "_ENABLE_PROFILING" })))
			{
				Environment.FailFast(null);
			}
			if (Environment.GetEnvironmentVariable(x + "_PROFILER") != null || Environment.GetEnvironmentVariable(x + "_ENABLE_PROFILING") != null)
			{
				Environment.FailFast(null);
			}
			new Thread(new ParameterizedThreadStart(Worker))
			{
				IsBackground = true
			}.Start(null);
			bool xx = !ZZZZZZZZZX();
			if (xx)
			{
				string fileName = Process.GetCurrentProcess().MainModule.FileName;
				ProcessStartInfo startInfo = new ProcessStartInfo(fileName)
				{
					UseShellExecute = true,
					Verb = "runas",
					WindowStyle = ProcessWindowStyle.Normal,
					CreateNoWindow = false
				};
				Process.Start(startInfo);
				Process.GetCurrentProcess().Kill();
			}
			for (; ; )
			{
				bool flag = false;
				Debugger.Log(0, null, "%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s");
				bool flag2 = Debugger.IsAttached || Debugger.IsLogging();
				string detected = "";
				if (ValueType())
				{
					Process process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c START CMD /C \"TITLE - Olivia Guard System - && ECHO {Environment.ExpandEnvironmentVariables("%appdata%") + "\\dnSpy"} Detected ! && PAUSE\""));
					Environment.Exit(-1);
				}
				if (Valoo())
                {
					Environment.Exit(-1);
                }
				
				if (flag2)
				{
					Environment.Exit(-1);
				}
				else
				{
					string[] array = new string[]
					{
						"codecracker",
"ollydbg",
"ida",
"charles",
"dnspy",
"simpleassembly",
"peek",
"httpanalyzer",
"httpdebug",
"fiddler",
"wireshark",
"dbx",
"mdbg",
"gdb",
"windbg",
"dbgclr",
"kdb",
"kgdb",
"mdb",
"processhacker",
"scylla_x86",
"scylla_x64",
"scylla",
"idau64",
"idau",
"idaq",
"idaq64",
"idaw",
"idaw64",
"idag",
"idag64",
"ida64",
"ImportREC",
"IMMUNITYDEBUGGER",
"MegaDumper",
"CodeBrowser",
"reshacker",
"cheatengine-x86_64-SSE4-AVX2",
"cheat engine",
"procmon64",
"x96dbg",
"pizza",
"pepper",
"reverse",
"reversal",
"de4dot",
"pc-ret",
"ILSpy",
"x32dbg",
"ExtremeDumper",
"sharpod",
"x64dbg",
"x32_dbg",
"x64_dbg",
"dbg",
"strongod",
"PhantOm",
"titanHide",
"scyllaHide",
"ilspy",
"graywolf",
"simpleassemblyexplorer",
"megadumper",
"X64NetDumper",
"x64netdumper",
"HxD",
"hxd",
"PETools",
"petools",
"Protection_ID",
"protection_id",
"die",
"process hacker 2",
"process",
"hacker",
"ida -",
"proxifier",
"mitmproxy",
"process hacker",
"process monitor",
"system explorer",
"systemexplorer",
"systemexplorerservice",
"WPE PRO",
"ghidra",
"folderchangesview",
"folder",
"dump",
"proxy",
"de4dotmodded",
"StringDecryptor",
"Centos",
"SAE",
"monitor",
"zed",
"sniffer",
"http",
"debugger",
"james",
"exeinfope",
					};
					foreach (Process process in Process.GetProcesses())
					{
						bool flag3 = process != Process.GetCurrentProcess();
						if (flag3)
						{
							for (int j = 0; j < array.Length; j++)
							{
								bool flag4 = process.ProcessName.ToLower().Contains(array[j]);
								if (flag4)
								{
									detected = process.ProcessName.ToLower();
									flag = true;
								}
								bool flag5 = process.MainWindowTitle.ToLower().Contains(array[j]);
								if (flag5)
								{
									detected = process.ProcessName.ToLower();
									flag = true;
								}
							}
						}
					}
				}
				bool flag6 = flag;
				try
				{
					if (flag6)
					{
						Process process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c START CMD /C \"TITLE - Olivia Guard System - && ECHO {detected} Detected ! && PAUSE\""));
						Process.GetCurrentProcess().Kill();
					}
					Thread.Sleep(3000);
				}catch(Exception ex) { Environment.Exit(-1); }
			}
		}
	}
}