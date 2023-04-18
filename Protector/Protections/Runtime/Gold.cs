using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OliviaGuard.Protector.Protections.Runtime
{
    public static class Gold
    {
        private static void Goldx()
        {
            /*   StackTrace stackTrace = new StackTrace();
               MethodBase method = ((stackTrace != null) ? stackTrace.GetFrame(1) : null).GetMethod();
               if (method != null)
               {
                   Debugger.Log(0, null, "%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s");
                   if (Assembly.GetExecutingAssembly().Equals(Assembly.GetCallingAssembly()))
               */
            //   if (method.Name == "InvokeMethod")
            //       goto end;
            Assembly assemblyDef = Assembly.GetExecutingAssembly();
            List<string> lisoX = new List<string>();
            foreach (var t in assemblyDef.GetTypes())
            {
                MethodInfo[] methodInfos = t.GetMethods();

                foreach (var xxxxxxxxxxxx in t.GetRuntimeMethods().Where(t => t.Name.StartsWith(Encoding.UTF8.GetString(Convert.FromBase64String("PE9saXZpYUd1YXJkPg==")))))
                {
                    MethodInfo methodDef = xxxxxxxxxxxx;
                    var stream = assemblyDef.GetManifestResourceStream(methodDef.Name);
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    //    string[] liso = null;
                    int maxStackSize = 0;
                    try
                    {
                        maxStackSize = methodDef.GetMethodBody().MaxStackSize;
                    }
                    catch (Exception) { }
                    string[] liso = new string[]
    {
                            methodDef.Name,
                            methodDef.GetMethodBody().GetILAsByteArray().Length.ToString(),
                            maxStackSize.ToString(),
                            $"{methodDef.ReturnType}",
                            $"{methodDef.DeclaringType}",
                            $"{"InjecToMePleases"}"
    };
                    //   liso.Add($"{method.Name + method.DeclaringType}");
                    //   liso.Add($"{method.Name + method.ReturnType}");
                    if (System.Text.Encoding.UTF8.GetString(ms.ToArray()) != null)
                    {
                        string o = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        string lol = string.Concat(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(liso))).Select(x => x.ToString("X2")));
                        if (o != lol)
                        {
                            MessageBox.Show("ERRRRRRRR");
                        }
                        else
                        {
                            MessageBox.Show(string.Concat(liso));
                        }
                    }
                    lisoX.Add(string.Concat(liso));


                }




                // MessageBox.Show(oma);
            }
            MessageBox.Show(string.Concat(lisoX));
            /*   string oma = string.Concat(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(lisoX))).Select(x => x.ToString("X2")));
               var keyBytes = Encoding.UTF8.GetBytes(oma);
               byte[] ekoo;
               using (var md5 = MD5.Create())
               {
                   ekoo = md5.ComputeHash(keyBytes);
               }
               var streamX = assemblyDef.GetManifestResourceStream("<OliviaGuard></OliviaGuard>");
               var msX = new MemoryStream();
               streamX.CopyTo(msX);
               byte[] bytes = msX.ToArray(); try {
                   using (var aes = Aes.Create())
                   using (var encryptor = aes.CreateDecryptor(ekoo, ekoo))
                   {
                       var decryptedBytes = encryptor
                           .TransformFinalBlock(bytes, 0, bytes.Length);
                   }
               }*/
        
    
            
            /*  }
              else
              {
                  MessageBox.Show("$$$$$$$$$$$$");
              }


          end:
              MessageBox.Show("rrrrrrrrrrrrrr");*/
        }

        //public static string Decrypt(string s, int key)
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    if (assembly == Assembly.GetCallingAssembly())
        //    {
        //        var module = assembly.ManifestModule;
        //        var field = module.ResolveField(777);
        //        var array = (int[])field.GetValue(null);
        //        for (int i = 0; i < array.Length; i++)
        //            key += array[i];
        //        var charray = new char[s.Length];
        //        for (int j = 0; j < s.Length; j++)
        //        {
        //            charray[j] = (char)(s[j] ^ key);
        //        }
        //        return new string(charray);
        //    }
        //    return assembly.FullName;
        //}
    }
}
