using dnlib.DotNet;
using OliviaGuard.Protector.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet.Emit;
using Microsoft.VisualBasic;
using OpCodes = dnlib.DotNet.Emit.OpCodes;
using System.Reflection;
using System.IO;
using OliviaGuard.Protector.Class.Constants;
using dnlib.IO;
using dnlib.PE;
using MethodImplAttributes = dnlib.DotNet.MethodImplAttributes;
using MethodAttributes = dnlib.DotNet.MethodAttributes;

namespace OliviaGuard.Protector.Protections.Constants
{
    public class Constants
    {
        public Constants(OliviaLib lib)
        {
            encodedMethods = new List<EncodedMethod>();
            Main(lib);
        }
        public static List<EncodedMethod> encodedMethods;
        public static Random rnd = new Random();
        public void modifythemodule(OliviaLib lib,string nameof,string targetText)
        {
            var typeModule = ModuleDefMD.Load(typeof(Runtime.MethodInvoker).Module);
            var Method = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.MethodInvoker).MetadataToken));
            var members = InjectHelper.Inject(Method, lib.moduleDef.GlobalType, lib.moduleDef);
            MethodDef init = lib.moduleDef.GlobalType.FindMethod("Invoker");
            lib.thefinalcctor.Body = init.Body;
            for(int i=0;i<lib.thefinalcctor.Body.Instructions.Count;i++)
            {
                if (lib.thefinalcctor.Body.Instructions[i].OpCode == OpCodes.Ldstr && lib.thefinalcctor.Body.Instructions[i].Operand.ToString() == targetText)
                    lib.thefinalcctor.Body.Instructions[i].Operand = nameof;
            }
            lib.globalType.Methods.Remove(init);

        }
        void Main(OliviaLib lib)
        {
            HolaAmigus();
            var goldstring = new FieldDefUser(Renamer.GenerateAnother(), new FieldSig(lib.moduleDef.ImportAsTypeSig(typeof(string))), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
            lib.moduleDef.GlobalType.Fields.Add(goldstring);
            GetTheField(lib,goldstring);

           

            MethodDef cctor = lib.ctor;
            // init.Name = Renamer.GenerateString();
            lib.globalType.Name = GenerateString();
           /* string nameof = GenerateString();
            MethodDef newMethod = new MethodDefUser(nameof, dnlib.DotNet.MethodSig.CreateStatic(lib.ModuleDefMD.CorLibTypes.Void),
                   MethodImplAttributes.IL | MethodImplAttributes.Managed,
                   MethodAttributes.Public | MethodAttributes.Static );
            CilBody cilBody = new CilBody();
            cilBody.Instructions.Add(OpCodes.Ret.ToInstruction());
            newMethod.Body = cilBody; 
            lib.moduleDef.GlobalType.Methods.Add(newMethod);*/
            string nameof = lib.ctor.Name;
            modifythemodule(lib,nameof, "<Injectmehere>");
            //**************************************************************************************************************
            //var typeModule = ModuleDefMD.Load(typeof(Runtime.Gold).Module);
           // var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.Gold).MetadataToken));
           // var members = InjectHelper.Inject(typeDefs, lib.moduleDef.GlobalType, lib.moduleDef);

       //     var init = (MethodDef)members.Single(method => method.Name == "Goldx");
            //init.Name = Renamer.InvisibleName;
          //  modifyAgain(lib, init.Name, "<InjectHereMotherFucker>");
          //  EditTheGold(lib, "InjecToMePleases", init.Name);
            //**************************************************************************************************************
            var module = lib.moduleDef;
            MethodDef newMethod = lib.ctor;
            var array = GenerateArray();
            string name = Renamer.InvisibleName;
            var field = new FieldDefUser(name, new FieldSig(module.ImportAsTypeSig(typeof(int[]))), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
            module.GlobalType.Fields.Add(field);
            InjectArray(array, newMethod, field);
            var decryptor = InjectDecryptor(lib);
            decryptor = ModifyDecryptor(decryptor, field,nameof);

            var Intdecryptor = InjectIntDecryptor(lib);
            Intdecryptor = ModifyDecryptor(decryptor, field,nameof);
            SpecialModify(lib, decryptor.Name);
            lib.GoldKey = GoldKey;
            foreach (TypeDef type in module.Types)
            {            
                if (type.IsGlobalModuleType) continue;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody || !method.Body.HasInstructions) continue;
                    if (!hasStrings(method)) continue;
                  
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            var local = new Local(method.Module.ImportAsTypeSig(typeof(string)));
                            method.Body.Variables.Add(local);
                            string operand = method.Body.Instructions[i].Operand.ToString();
                         
                            method.Body.Instructions[i].OpCode = OpCodes.Ldloc;
                            method.Body.Instructions[i].Operand = local;
                            method.Body.Instructions.Insert(0, OpCodes.Ldstr.ToInstruction(operand));
                            method.Body.Instructions.Insert(1, OpCodes.Stloc_S.ToInstruction(local));
                            i += 2;
                        }
                    }
                    int num = rnd.Next(100, 500);
                    encodedMethods.Add(new EncodedMethod(method, num));

                    var ctor = newMethod;

                    Local hlocal = new Local(module.ImportAsTypeSig(typeof(RuntimeMethodHandle)));
                    ctor.Body.Variables.Add(hlocal);
                    ctor.Body.Instructions.Insert(0, OpCodes.Ldtoken.ToInstruction(method));
                    ctor.Body.Instructions.Insert(1, OpCodes.Stloc.ToInstruction(hlocal));


                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        MethodDef currentlydecryptor = null;
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            var s = method.Body.Instructions[i].Operand.ToString();
                            var realkey = rnd.Next(10, 50);
                            string encoded = EncodeString(s, realkey + num, array);
                            bool fooo = false;
                            var newfieldo = new FieldDefUser(Renamer.GenerateName(), new FieldSig(module.ImportAsTypeSig(typeof(int[]))), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
                            if (encoded.Length != 0)
                            {
                                fooo = true;
                                module.GlobalType.Fields.Add(newfieldo);
                                var newarray = GenerateIntArrayFromString(encoded);
                                InjectArray(newarray, newMethod, newfieldo);
                                currentlydecryptor = Intdecryptor;
                            }
                            var fieldstatic = new FieldDefUser(Renamer.GenerateAnother(), new FieldSig(module.ImportAsTypeSig(typeof(string))), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
                            module.GlobalType.Fields.Add(fieldstatic);

                            var e = ctor.Body.Instructions.Count - 1;
                            if (fooo == false)
                            {
                            ctor.Body.Instructions.Insert(e, OpCodes.Ldstr.ToInstruction(""));
                                currentlydecryptor = decryptor;
                            }else
                            {
                                ctor.Body.Instructions.Insert(e, OpCodes.Ldsfld.ToInstruction(newfieldo));
                              //  ctor.Body.Instructions.Insert(++e,new Instruction(OpCodes.Call, module.Import(typeof(System.String).GetMethod("Concat", new Type[] { typeof(string[]) }))));
                            }
                            // ctor.Body.Instructions.Insert(e, new Instruction(OpCodes.Call, module.Import(typeof(System.String).GetMethod("Concat", new Type[] { typeof(string[]) }))));
                            ctor.Body.Instructions.Insert(++e, OpCodes.Ldc_I4.ToInstruction(realkey));
                            ctor.Body.Instructions.Insert(++e, OpCodes.Ldloc.ToInstruction(hlocal));
                            ctor.Body.Instructions.Insert(++e, OpCodes.Call.ToInstruction(currentlydecryptor));
                            ctor.Body.Instructions.Insert(++e, OpCodes.Stsfld.ToInstruction(fieldstatic));
                        //    ctor.Body.Instructions.Insert(++e, OpCodes.Stsfld.ToInstruction(newfieldo));
                            method.Body.Instructions[i] = OpCodes.Ldsfld.ToInstruction(fieldstatic);

                            method.Body.SimplifyBranches();
                            method.Body.OptimizeBranches();
                        }
                    }
                }
            }

        }

        string EncodeString(string s, int realkey, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
                realkey += array[i];
            var charray = new char[s.Length];
            for (int j = 0; j < s.Length; j++)
            {
                charray[j] = (char)(s[j] ^ realkey);
            }
            return new string(charray);
        }


        int[] GenerateArray()
        {
            var array = new int[rnd.Next(10, 50)];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(100, 500);
            }
            return array;
        }
        string[] GenerateStringArray(string iok)
        {
            var array = new string[iok.Length];
            int o = 0;
            foreach (var i in iok)
            {
                array[o] = Convert.ToString(i);
                o++;
            }
            return array;
        }
        public  string WhatWillbe;
        public  string HolaAmigus()
        {
            WhatWillbe = new Random().Next(1995, 2022) + "|" + new Random().Next(1,12) + "|" + new Random().Next(1, 30);
            return WhatWillbe;
        }
        public int oooo()
        {
            string[] oka = WhatWillbe.Split('|');
            int year = Convert.ToInt32(oka[0]);
            int month = Convert.ToInt32(oka[1]);
            int day = Convert.ToInt32(oka[2]);
            GoldKey = $"{year - month + day}";
            return year - month + day;
        }
        int[] GenerateIntArrayFromString(string iok)
        {
          
            var array = new int[iok.Length];
            int o = 0;
            foreach (var i in iok)
            {
                array[o] = (int)i * oooo();
                o++;
            }
            return array;
        }
        void InjectArray(int[] array, MethodDef cctor, FieldDef arrayField)
        {
            if (rnd.Next(0, 3) == 1)
            {
                List<Instruction> toInject = new List<Instruction>
                {
                    OpCodes.Ldc_I4.ToInstruction(array.Length),
                    OpCodes.Newarr.ToInstruction(cctor.Module.CorLibTypes.Int32),
                    OpCodes.Stsfld.ToInstruction(arrayField)
                };
                for (int i = 0; i < array.Length; i++)
                {
                    toInject.Add(OpCodes.Ldsfld.ToInstruction(arrayField));
                    toInject.Add(OpCodes.Ldc_I4.ToInstruction(i));
                    toInject.Add(OpCodes.Ldc_I4.ToInstruction(array[i]));
                    toInject.Add(OpCodes.Stelem_I4.ToInstruction());
                    toInject.Add(OpCodes.Nop.ToInstruction());
                }
                for (int j = 0; j < toInject.Count; j++)
                    cctor.Body.Instructions.Insert(j, toInject[j]);
            }
            else
            {
                List<Instruction> toInject = new List<Instruction>
                {
                    OpCodes.Ldc_I4.ToInstruction(array.Length),
                    OpCodes.Newarr.ToInstruction(cctor.Module.CorLibTypes.Int32),
                    OpCodes.Dup.ToInstruction()
                };
                for (int i = 0; i < array.Length; i++)
                {
                    toInject.Add(OpCodes.Ldc_I4.ToInstruction(i));
                    toInject.Add(OpCodes.Ldc_I4.ToInstruction(array[i]));
                    toInject.Add(OpCodes.Stelem_I4.ToInstruction());
                    if (i != array.Length - 1)
                    {
                        toInject.Add(OpCodes.Dup.ToInstruction());
                    }
                }
                toInject.Add(OpCodes.Stsfld.ToInstruction(arrayField));
                for (int j = 0; j < toInject.Count; j++)
                    cctor.Body.Instructions.Insert(j, toInject[j]);
            }

        }
        string GoldKey;
        void InjectStringArray(string[] array, MethodDef cctor, FieldDef arrayField)
        {
            if (rnd.Next(0, 3) == 1)
            {
                List<Instruction> toInject = new List<Instruction>
                {
                    OpCodes.Ldc_I4.ToInstruction(array.Length),
                    OpCodes.Newarr.ToInstruction(cctor.Module.CorLibTypes.String),
                    OpCodes.Stsfld.ToInstruction(arrayField)
                };
                for (int i = 0; i < array.Length; i++)
                {
                    toInject.Add(OpCodes.Ldsfld.ToInstruction(arrayField));
                    toInject.Add(OpCodes.Ldc_I4.ToInstruction(i));
                    toInject.Add(OpCodes.Ldstr.ToInstruction(array[i]));
                    toInject.Add(OpCodes.Stelem_I4.ToInstruction());
                    toInject.Add(OpCodes.Nop.ToInstruction());
                }
                for (int j = 0; j < toInject.Count; j++)
                    cctor.Body.Instructions.Insert(j, toInject[j]);
            }
            else
            {
                List<Instruction> toInject = new List<Instruction>
                {
                    OpCodes.Ldc_I4.ToInstruction(array.Length),
                    OpCodes.Newarr.ToInstruction(cctor.Module.CorLibTypes.String),
                    OpCodes.Dup.ToInstruction()
                };
                for (int i = 0; i < array.Length; i++)
                {
                    toInject.Add(OpCodes.Ldc_I4.ToInstruction(i));
                    toInject.Add(OpCodes.Ldstr.ToInstruction(array[i]));
                    toInject.Add(OpCodes.Stelem_I4.ToInstruction());
                    if (i != array.Length - 1)
                    {
                        toInject.Add(OpCodes.Dup.ToInstruction());
                    }
                }
                toInject.Add(OpCodes.Stsfld.ToInstruction(arrayField));
                for (int j = 0; j < toInject.Count; j++)
                    cctor.Body.Instructions.Insert(j, toInject[j]);
            }

        }
        MethodDef InjectDecryptor(OliviaLib lib)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(Runtime.ConstantsRuntime).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.ConstantsRuntime).MetadataToken));
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, lib.globalType, lib.moduleDef);
            var dec = (MethodDef)members.Single(method => method.Name == "Decrypt");

            foreach (var mem in members)
                mem.Name = GenerateString();

            return dec;
        }
        MethodDef InjectIntDecryptor(OliviaLib lib)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(Runtime.IntConstantsRuntime).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.IntConstantsRuntime).MetadataToken));
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, lib.globalType, lib.moduleDef);
            var dec = (MethodDef)members.Single(method => method.Name == "Decrypt");

            foreach (var mem in members)
                mem.Name = GenerateString();

            return dec;
        }
        public static string GenerateName()
        {
            const string chars = "あいうえおかきくけこがぎぐげごさしすせそざじずぜアイウエオクザタツワルムパリピンプペヲポ";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public static string GenerateString()
        {
            int seed = rnd.Next();
            return "<OliviaGuard>" + (seed * 0x19660D + 0x3C6EF35).ToString("X") + "</OliviaGuard>";
        }
        MethodDef ModifyDecryptor(MethodDef dec, FieldDef field,string nameof)
        {
            for (int j = 0; j < dec.Body.Instructions.Count; j++)
            {
                if (dec.Body.Instructions[j].OpCode == OpCodes.Ldsfld )
                {
                    dec.Body.Instructions[j].OpCode = OpCodes.Ldsfld;
                    dec.Body.Instructions[j].Operand = field;
                }
                if (dec.Body.Instructions[j].OpCode == OpCodes.Ldstr && dec.Body.Instructions[j].Operand.ToString() == ".cctor")
                {
                    dec.Body.Instructions[j].Operand = nameof;
                }
               
                if (dec.Body.Instructions[j].OpCode == OpCodes.Ldc_I4 && dec.Body.Instructions[j].Operand.ToString().Contains("77777"))
                {
                    string numberedHex = $"0x{tokeno:X6}";
                    int value = (int)new System.ComponentModel.Int32Converter().ConvertFromString(numberedHex) + 1;
                    dec.Body.Instructions[j].Operand = value;
                }
                if (dec.Body.Instructions[j].OpCode == OpCodes.Ldc_I4 && dec.Body.Instructions[j].Operand.ToString().Contains("777"))
                {
                    dec.Body.Instructions[j].Operand = oooo() - oooo() ;
                }
            }
            return dec;
        }
        int tokeno;
        public void modifyAgain(OliviaLib lib, string nameof, string targetText)
        {
            for (int i = 0; i < lib.thefinalcctor.Body.Instructions.Count; i++)
            {
                if (lib.thefinalcctor.Body.Instructions[i].OpCode == OpCodes.Ldstr && lib.thefinalcctor.Body.Instructions[i].Operand.ToString() == targetText)
                    lib.thefinalcctor.Body.Instructions[i].Operand = nameof;
            }

        }
        public void SpecialModify(OliviaLib lib, string nameof)
        {
           /* MethodDef init = lib.moduleDef.GlobalType.FindMethod(nameof);
            for (int i = 0; i < init.Body.Instructions.Count; i++)
            {
                if (init.Body.Instructions[i].OpCode == OpCodes.Ldsfld && init.Body.Instructions[i +1].Operand.ToString().Contains("ToInt32"))
                    init.Body.Instructions[i].Operand = Omba;
            }*/

        }
        FieldDef Omba;
        public void GetTheField(OliviaLib lib,FieldDef fld)
        {
            MethodDef init = lib.moduleDef.GlobalType.FindMethod("AntiDumpInj");
            lib.GoldResource = Renamer.InvisibleName;
            for (int j = 0; j < init.Body.Instructions.Count; j++)
            {
                 if (init.Body.Instructions[j].OpCode == OpCodes.Ldsfld && init.Body.Instructions[j].Operand.ToString().Contains("lokalo"))
                 {
                    FieldDef fieldDef = (FieldDef)init.Body.Instructions[j].Operand;
                    tokeno =  fieldDef.MDToken.ToInt32();
                  //  MessageBox.Show(field.MDToken.ToString());
                    //init.Body.Instructions[j].Operand = fld;
                 }
                if (init.Body.Instructions[j].OpCode == OpCodes.Ldstr && init.Body.Instructions[j].Operand.ToString().Contains("<OliviaGuard></OliviaGuard>"))
                {

                    init.Body.Instructions[j].Operand = lib.GoldResource ;
                  
                }
            }
            init.Name = Renamer.InvisibleName;
            lib.GoldName = init.Name;
        }
        public void EditTheGold(OliviaLib lib,string nameof,string MethodName)
        {
            MethodDef init = lib.moduleDef.GlobalType.FindMethod(MethodName);
            for (int j = 0; j < init.Body.Instructions.Count; j++)
            {
                if (init.Body.Instructions[j].OpCode == OpCodes.Ldstr && init.Body.Instructions[j].Operand.ToString().Contains(nameof))
                {
                    init.Body.Instructions[j].Operand = lib.GoldName;
                }
            }
        }
        bool hasStrings(MethodDef method)
        {
            foreach (var instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldstr)
                    return true;
            }
            return false;
        }
    }
}