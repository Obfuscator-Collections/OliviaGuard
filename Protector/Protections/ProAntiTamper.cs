using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Properties;
using OliviaGuard.Protector.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace OliviaGuard.Protector.Protections
{
    public class IntegrityValidator
    {
        public IntegrityValidator(Type type)// do validation checks on all methods with our attribute
        {
            int j = 0;
            foreach (var method in type.GetMethods().Where(item => item.GetCustomAttribute(typeof(ChecksumAttribute)) != null))
            {
                j++;
                if (!Validate(method))
                { //if one method is tampered
                    Environment.FailFast(null);
                }
            }
            if (j == 0)
            {
                Environment.FailFast(null);
            }
        }

        public string CalculateChecksum(MethodInfo o)
        {
            StringBuilder sb = new StringBuilder();
            var data = o.GetMethodBody().GetILAsByteArray();
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public bool Validate(MethodInfo o)
        {
            foreach (var attr in o.GetCustomAttributes(typeof(ChecksumAttribute), false))
            {
                if (attr.GetType() == typeof(ChecksumAttribute))
                {
                    var attrib = attr as ChecksumAttribute;
                    //Console.WriteLine(attrib.Hash);
                    //Console.WriteLine(CalculateChecksum(o));
                    return attrib.Hash == CalculateChecksum(o);
                }
            }
            return false; //doesn't have checksum
        }
        [AttributeUsage(AttributeTargets.Method)]
        public class ChecksumAttribute : Attribute
        {
            public string Hash { get; set; }
            public ChecksumAttribute(string hash)
            {
                this.Hash = hash;
            }
        }
        public bool ValidateIntegrity() => true;
    }
    public class ProAntiTamper
    {
        public ProAntiTamper(OliviaLib lib) => Main(lib);

        public string CalculateChecksum(MethodInfo o)
        {
            StringBuilder sb = new StringBuilder();
            var data = o.GetMethodBody().GetILAsByteArray();
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }
        Module module;
        public IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
        public Type[] GetAllTypeinAssembly(string assemblyName)
        {
            asmBase = System.IO.Path.GetDirectoryName(assemblyName);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(assemblyName);//domain.Load(bt) ;// 

            return asm.GetTypes();
        }

        private string asmBase;
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";
            objExecutingAssemblies = args.RequestingAssembly;
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.                
                    strTempAssmbPath = asmBase + "\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }

            }
            //Load the assembly from the specified path.                    
            MyAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return MyAssembly;
        }
        public List<string> tamperedmethods;
        public static string GetHashSHA1(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
            }
        }
        private static CilBody MergeMethods(MethodDef original, MethodDef target, int mergeLoc = 0)
        {

          //  original.FreeMethodBody();
            target.FreeMethodBody();

            var originalBody = original.Body;
            var targetBody = target.Body;

            var targetModule = target.Module;

            var originalInstructions = originalBody.Instructions;
            var targetInstructions = targetBody.Instructions;

            Console.WriteLine("=original method=");
            Console.WriteLine();
            foreach (var originalInstruction in originalInstructions)
            {
                Console.WriteLine(originalInstruction);
            }
            Console.WriteLine();
            Console.WriteLine("=target method=");
            Console.WriteLine();
            foreach (var targetInstruction in targetInstructions)
            {
                Console.WriteLine(targetInstruction);
            }

          //  RemoveReturn(ref originalInstructions, true);

            var localOffset = targetBody.Variables.Count;

            for (var i = 0; i < originalBody.Variables.Count; i++)
            {
                targetBody.Variables.Add(
                    new Local(originalBody.Variables[i].Type, originalBody.Variables[i].Name));
            }

            for (var i = originalInstructions.Count - 1; i >= 0; i--)
            {
                var o = originalInstructions[i];
                var c = new Instruction(o.OpCode, o.Operand);

                switch (o.Operand)
                {
                    case IType type:
                        c.Operand = targetModule.Import(type);
                        break;
                    case IMethod method:
                        c.Operand = targetModule.Import(method);
                        break;
                    case IField field:
                        c.Operand = targetModule.Import(field);
                        break;
                }

                if (o.OpCode == OpCodes.Stloc)
                {
                    c.OpCode = OpCodes.Stloc;
                    c.Operand = targetBody.Variables[o.GetParameterIndex() + localOffset];
                }
                else if (o.OpCode == OpCodes.Ldloc)
                {
                    c.OpCode = OpCodes.Ldloc;
                    c.Operand = targetBody.Variables[i + localOffset];
                }
                else if (o.OpCode == OpCodes.Ldloca)
                {
                    c.OpCode = OpCodes.Ldloca;
                    c.Operand = targetBody.Variables[i + localOffset];
                }

                targetInstructions.Insert(mergeLoc, c);
            }

            targetBody.OptimizeMacros();
            targetBody.OptimizeBranches();

            Console.WriteLine();
            Console.WriteLine("=merged method=");
            Console.WriteLine();

            foreach (var instruction in targetBody.Instructions)
            {
                Console.WriteLine(instruction);
            }

            Console.WriteLine(targetBody.Variables.Count);

            return targetBody;
        }
        private static byte[] GetKey(string password)
        {
            var keyBytes = Encoding.UTF8.GetBytes(password);
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(keyBytes);
            }
        }
        public static byte[] EncryptString(string password,string toEncrypt)
        {
            var key = GetKey(password);

            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(key, key))
            {
                var plainText = Encoding.UTF8.GetBytes(toEncrypt);
                return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
            }
        }
        public static void EditForReason(OliviaLib lib,string MethodName, string WhatToEdit,string ReplacedWith)
        {
            MethodDef init = lib.moduleDef.GlobalType.FindMethod(MethodName);
            for (int j = 0; j < init.Body.Instructions.Count; j++)
            {
                if (init.Body.Instructions[j].OpCode == OpCodes.Ldstr && init.Body.Instructions[j].Operand.ToString().Contains(WhatToEdit))
                {
                    init.Body.Instructions[j].Operand = ReplacedWith;
                }
            }
        }
        void Main(OliviaLib lib)
        {
            var module = lib.moduleDef;
      //      var typeModule = ModuleDefMD.Load(typeof(Runtime.AntiTamper).Module);
     //       var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.AntiTamper).MetadataToken));
     //       var members = InjectHelper.Inject(typeDefs, module.GlobalType, module);
     //       var init = (MethodDef)members.Single(method => method.Name == "Initialize");
        //    var init2 = (MethodDef)members.Single(method => method.Name == ".cctor");
            MethodDef cctor = lib.thefinalcctor;
            //     init.Name = "NotMePlease";
            //      cctor.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(init));

            // -------------------------------

            // -------------------------------
            List<string> lisoX = new List<string>();
            try
            {
                tamperedmethods = new List<string>();
                ModuleDef _moduleDef = lib.moduleDef;
                Assembly assemblyDef = null;
              
                     assemblyDef =  Assembly.LoadFrom(lib.filePath);
                AssemblyName[] l = assemblyDef.GetReferencedAssemblies();
                MethodInfo methodInfo = (MethodInfo)assemblyDef.EntryPoint;
               // var publicClasses = assemblyDef.GetExportedTypes().Where(p => p.IsClass);
                Dictionary<string, string> d = new Dictionary<string, string>();
                string omag = "";
                foreach (TypeDef type in _moduleDef.Types)
                {
                    if (type.IsDelegate)
                    {
                        omag += type.Name + "\r\n";
                        continue;
                    }
                        foreach (MethodDef method in type.Methods)
                    {
                        try
                        {


                            if (method.Name.Contains("OliviaGuard"))
                            {
                                MethodInfo met = null;
                                try
                                {
                                    met = (MethodInfo)assemblyDef.ManifestModule.ResolveMethod(method.MDToken.ToInt32());
                                }
                                catch (Exception ex) { continue; }
                                if (!tamperedmethods.Contains(method.Name))
                                {

                                    int maxStackSize = 0;
                                    int MethodLength = 0;
                                    try
                                    {
                                        maxStackSize = met.GetMethodBody().MaxStackSize;
                                    }
                                    catch (Exception) { }
                                    try
                                    {
                                        MethodLength = met.GetMethodBody().GetILAsByteArray().Length;
                                    }
                                    catch (Exception) { continue; }
                                    string[] liso = new string[]
                        {
                            method.Name,
                            MethodLength.ToString(),
                            maxStackSize.ToString(),
                            $"{method.ReturnType}",
                            $"{method.DeclaringType}",
                            $"{lib.GoldName}"
                        };
                                    //   liso.Add($"{method.Name + method.DeclaringType}");
                                    //   liso.Add($"{method.Name + method.ReturnType}");*/
                                    //List<string> liso = new List<string>();
                                    //     foreach (var instruction in method.Body.Instructions)
                                    //         liso.Add($"{instruction.OpCode} \"{instruction.Operand}\"");

                                    // byte[] methodo = met.GetMethodBody().GetILAsByteArray();
                                    //   if (methodo.Length != 0)
                                    //   {
                                    //  byte[] last8 = new byte[5];
                                    //       Array.Copy(methodo, methodo.Length - 5, last8, 0, 5);
                                    d.Add(met.Name, GetHashSHA1(Encoding.UTF8.GetBytes(string.Concat(liso))));
                                    //lib.moduleDef.Resources.Add(new EmbeddedResource(met.Name, Encoding.UTF8.GetBytes(GetHashSHA1(Encoding.UTF8.GetBytes(string.Concat(liso))))));
                                    tamperedmethods.Append(met.Name);
                                    // xxx.Add(met.Name + "\r\n" + "IL Bytes: " + met.GetMethodBody().GetILAsByteArray().Length + "\r\n");
                                    TypeSig stringSig = module.CorLibTypes.String;



                                    // }
                                }
                            }


                        }
                        catch (Exception) { continue; }
                    }
                }
                Type[] typesInAsm;
                try
                {
                    typesInAsm = assemblyDef.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInAsm = ex.Types;
                }
                foreach (var t in typesInAsm)
                {
                    if(t != null)
                    if (omag.Contains(t.Name))
                        continue;

                    if (t == null)
                        continue;

                    MethodInfo[] methodInfos = t.GetMethods();

                    foreach (var method in t.GetRuntimeMethods())
                    {
                        MethodInfo methodDef = method;
                        if (methodDef.Name.Contains("OliviaGuard"))
                        {

                            //   ObjectToByteArray(method.MethodBody);
                            //        met.GetMethodBody().GetILAsByteArray();
                            try
                            {
                                if (!tamperedmethods.Contains(method.Name))
                                {
                                    tamperedmethods.Append(methodDef.Name);
                                    byte[] methodo = methodDef.GetMethodBody().GetILAsByteArray();
                                    if (methodo.Length != 0)
                                    {
                                        //byte[] last8 = new byte[5];
                                        //   Array.Copy(methodo, methodo.Length - 5, last8, 0, 5);
                                        int maxStackSize = 0;
                                        try
                                        {
                                            maxStackSize = methodDef.GetMethodBody().MaxStackSize;
                                        }
                                        catch (Exception) { }
                                        string[] liso = new string[]
                        {
                            method.Name,
                            methodDef.GetMethodBody().GetILAsByteArray().Length.ToString(),
                            maxStackSize.ToString(),
                            $"{method.ReturnType}",
                            $"{method.DeclaringType}",
                            $"{lib.GoldName}"
                        };

                                        //        liso.Add($"{method.Name +method.DeclaringType}");
                                        //   liso.Add($"{method.Name + method.ReturnType}");*/

                                        d.Add(methodDef.Name, GetHashSHA1(Encoding.UTF8.GetBytes(string.Concat(liso))));
                                        //lib.moduleDef.Resources.Add(new EmbeddedResource(methodDef.Name, Encoding.UTF8.GetBytes(GetHashSHA1(Encoding.UTF8.GetBytes(string.Concat(liso))))));

                                    }
                                }
                            }
                            catch (Exception) { continue; }
                        }


                    }
                }
                foreach (var info in d)
                {
                    lib.moduleDef.Resources.Add(new EmbeddedResource(info.Key, Encoding.UTF8.GetBytes(info.Value)));
                    lisoX.Add(info.Key + info.Value);
                }
                string goldkey = lib.GoldKey;

                string key = GetHashSHA1(Encoding.UTF8.GetBytes(lib.GoldName + "̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀Olivia Crashed Yà͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̼̮̖͎̭̭͇͚̮͙͙̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀̀"));
                byte[] encryptedbytes = EncryptString(key, goldkey);
                lib.moduleDef.Resources.Add(new EmbeddedResource(lib.GoldResource, encryptedbytes));

            }
            catch (Exception ex) {
                MessageBox.Show(ex.StackTrace);
            }
        }
            // var typeModule = ModuleDefMD.Load(typeof(Runtime.StrongAntiTamper).Module);
            //  var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.StrongAntiTamper).MetadataToken));
            //  var members = InjectHelper.Inject(typeDefs, lib.moduleDef.GlobalType, lib.moduleDef);

        
            
            
        
        static byte[] Compress(byte[] array)
        {
            using (var ms = new MemoryStream())
            {
                using (var def = new DeflateStream(ms, CompressionLevel.NoCompression))
                {
                    def.Write(array, 0, array.Length);
                }

                return ms.ToArray();
            }
        }
        public byte[] ToArray( Stream s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.CanRead)
                throw new ArgumentException("Stream cannot be read");

            MemoryStream ms = s as MemoryStream;
            if (ms != null)
                return ms.ToArray();

            long pos = s.CanSeek ? s.Position : 0L;
            if (pos != 0L)
                s.Seek(0, SeekOrigin.Begin);

            byte[] result = new byte[s.Length];
            s.Read(result, 0, result.Length);
            if (s.CanSeek)
                s.Seek(pos, SeekOrigin.Begin);
            return result;
        }
    }
}