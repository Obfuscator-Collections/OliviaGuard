using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace OliviaGuard.Protector.Protections.Runtime
{
    internal static class AntiDump
    {
        [DllImport("kernel32.dll")]
        static extern unsafe bool VirtualProtect(byte* lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);
        static unsafe void HelloMotherFucker()
        {
            try
            {
                List<Type> list2 = new List<Type>();
                DynamicMethod dynamicMethod2 = new DynamicMethod("<OliviaGuard></OliviaGuard>", typeof(int), list2.ToArray(), typeof(System.Environment).Module, true);
                ILGenerator ilgenerator2 = dynamicMethod2.GetILGenerator();
                ilgenerator2.Emit(OpCodes.Ldc_I4_M1);
                ilgenerator2.Emit(OpCodes.Call, typeof(System.Environment).GetMethod("Exit", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType | BindingFlags.OptionalParamBinding | BindingFlags.IgnoreReturn, null, new Type[0], null));
                ilgenerator2.Emit(OpCodes.Ret);

                dynamicMethod2.Invoke(null, new object[0]);
            }
            catch (Exception ex) { Environment.Exit(-1 + 0); }
        }
        public static string lokalo;
        private static unsafe void AntiDumpInj()
        {
            
            
        
            #region Stuffs
            uint AnY;

            var module = typeof(AntiDump).Module;
            var bas = (byte*)Marshal.GetHINSTANCE(module);

            var ptr = bas + 0x3c;
            byte* ptr2;
            ptr = ptr2 = bas + *(uint*)ptr;
            ptr += 0x6;

            var sectNum = *(ushort*)ptr;
            ptr += 14;

            var optSize = *(ushort*)ptr;
            ptr = ptr2 = ptr + 0x4 + optSize;

            byte* @new = stackalloc byte[11];
            #endregion  

            // Prevents dumping performed by famous tools as MegaDumper
            VirtualProtect(ptr - 16, 8, 0x40, out AnY);
            *(uint*)(ptr - 12) = 0;
            var mdDir = bas + *(uint*)(ptr - 16);
            *(uint*)(ptr - 16) = 0;

            // Erase MetaData (DataDir) - This is the most important part of the code!
            VirtualProtect(mdDir, 0x48, 0x40, out AnY);
            var mdHdr = bas + *(uint*)(mdDir + 8);
            *(uint*)mdDir = 0;
            *((uint*)mdDir + 1) = 0;
            *((uint*)mdDir + 2) = 0;
            *((uint*)mdDir + 3) = 0;

            // Erase value for MetaData.RVA (BSJB)
            VirtualProtect(mdHdr, 4, 0x40, out AnY);
            *(uint*)mdHdr = 0;

            // Erase sections name
            for (int i = 0; i < sectNum; i++)
            {
                VirtualProtect(ptr, 8, 0x40, out AnY);
                Marshal.Copy(new byte[8], 0, (IntPtr)ptr, 8);
                ptr += 0x28;
            }
            try
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase methodDEf = ((stackTrace != null) ? stackTrace.GetFrame(1) : null).GetMethod();
                if (methodDEf != null)
                {
                    Assembly assemblyDef = Assembly.GetExecutingAssembly();
                    List<string> lisoX = new List<string>();
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    MethodBase m = MethodBase.GetCurrentMethod();
                    foreach (var t in assemblyDef.GetTypes())
                    {
                        MethodInfo[] methodInfos = t.GetMethods();

                        foreach (var xxxxxxxxxxxx in t.GetRuntimeMethods().Where(t => t.Name.StartsWith(Encoding.UTF8.GetString(Convert.FromBase64String("PE9saXZpYUd1YXJkPg==")))))
                        {
                            MethodInfo methodDef = xxxxxxxxxxxx;
                            var stream = assemblyDef.GetManifestResourceStream(methodDef.Name);
                            var ms = new MemoryStream();
                            stream.CopyTo(ms);
                            int maxStackSize = methodDef.GetMethodBody().MaxStackSize;
                            string[] liso = null;
                            bool x = !methodDEf.Name.StartsWith(Encoding.UTF8.GetString(Convert.FromBase64String("PE9saXZpYUd1YXJkPg==")));
                            if (!x)
                            {
                                liso = new string[]
                            {
                            xxxxxxxxxxxx.Name,
                            methodDef.GetMethodBody().GetILAsByteArray().Length.ToString(),
                            maxStackSize.ToString(),
                            $"{methodDef.ReturnType}",
                            $"{methodDef.DeclaringType}",
                            $"{m.Name}"
                            };
                            }


                            //   liso.Add($"{method.Name + method.DeclaringType}");
                            //   liso.Add($"{method.Name + method.ReturnType}");
                            if (System.Text.Encoding.UTF8.GetString(ms.ToArray()) != null)
                            {
                                string ooo;
                                string o = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                                string lol = string.Concat(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(liso))).Select(x => x.ToString("X2")));
                                if (!string.Equals(o, lol))
                                {
                                    Environment.Exit(-1);

                                }           
                            }
                            else
                            {
                                Environment.Exit(-1);
                            }

                        }

                    }
                   
                    string okama =  string.Concat(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(m.Name + "̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀Olivia Crashed Yà͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀")).Select(x => x.ToString("X2")));
                    var streamX = assemblyDef.GetManifestResourceStream("<OliviaGuard></OliviaGuard>");
                    var msX = new MemoryStream();
                    streamX.CopyTo(msX);
                    byte[] bytes = msX.ToArray();
                    try
                    {
                        byte[] dodaa = null;
                        using (var md5 = MD5.Create())
                        {
                            dodaa = md5.ComputeHash(Encoding.UTF8.GetBytes(okama));
                        }
                        byte[] dooooo = null;
                        using (var aes = Aes.Create())
                        using (var encryptor = aes.CreateDecryptor(dodaa, dodaa))
                        {
                            dooooo = encryptor
                                .TransformFinalBlock(bytes, 0, bytes.Length);
                        }
                        lokalo = Encoding.UTF8.GetString(dooooo);
                        Convert.ToInt32(lokalo);
                    }
                    catch (Exception ex) { Environment.Exit(-1); }


                }
            }
            catch (Exception ex) { Environment.Exit(-1 + 0); }
        }
    }
}
