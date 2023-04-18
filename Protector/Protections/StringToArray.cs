using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OliviaGuard.Protector.Protections
{
    public class Randomizer
    {
        private static readonly RandomNumberGenerator csp = RandomNumberGenerator.Create();
        private static readonly char[] chars = "duckduckduckduckduckduckduckduckduckduckduck".ToCharArray();

        public static int Next(int maxValue, int minValue = 0)
        {
            if (minValue >= maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
            long diff = (long)maxValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;
            uint ui;
            do { ui = RandomUInt(); } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        public static string GenerateRandomString(int size)
        {
            StringBuilder stringy = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var renamer = "_" + Next(100000, 10000);
                stringy.Append(renamer);
            }
            return stringy.ToString();
        }

        private static uint RandomUInt()
        {
            return BitConverter.ToUInt32(RandomBytes(sizeof(uint)), 0);
        }

        private static byte[] RandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            csp.GetNonZeroBytes(buffer);
            return buffer;
        }
    }
    public class StringToArray
    {
        private static ModuleDefMD _moduleDefMd;
        public static int StringLength()
        {
            return Randomizer.Next(15, 10);
        }
        public StringToArray(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            Random rng = new Random();
            foreach (TypeDef type in lib.moduleDef.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody || method.Body == null || method.Name == ".cctor" || type.Name != type.Module.GlobalType.Name || method.Body.Instructions[0].OpCode == OpCodes.Ldc_I4) continue;

                    IList<Instruction> instr = method.Body.Instructions;

                    for (int i = 0; i < instr.Count; i++)
                    {
                        try
                        {
                            if (!(method.Body.Instructions[i].OpCode == OpCodes.Ldstr)) continue;

                            //locals
                            List<Instruction> instrreal = new List<Instruction>();
                            int count = 0;

                            //add variable
                            Random dg = new Random();
                            var local = new Local(method.Module.Import(typeof(string[])).ToTypeSig(), Renamer.GenerateString());
                            method.Body.Variables.Add(local);
                            string encodedbase64 = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(instr[i].Operand.ToString())); //Encrypted String by Base64
                            int numbers = encodedbase64.Length;
                            //add array constructor
                            instrreal.Add(new Instruction(OpCodes.Ldc_I4, numbers + 1));
                            instrreal.Add(new Instruction(OpCodes.Newarr, method.Module.Import(typeof(string))));
                            instrreal.Add(new Instruction(OpCodes.Stloc_S, local));

                            //add array items
                            foreach (char boi in encodedbase64)
                            {
                                instrreal.Add(new Instruction(OpCodes.Ldloc, local));
                                instrreal.Add(new Instruction(OpCodes.Ldc_I4, count));
                                switch (rng.Next(0, 2))
                                {
                                    case 0:
                                        {
                                            // instrreal.Add(new Instruction(OpCodes.Ldc_I8, (long)boi));
                                            //   instrreal.Add(Instruction.Create(OpCodes.Call, lib.moduleDef.Import(typeof(System.Convert).GetMethod("ToChar", new Type[] { typeof(long) }))));
                                            //  break;

                                            instrreal.Add(new Instruction(OpCodes.Ldc_I4, (int)boi));
                                            instrreal.Add(Instruction.Create(OpCodes.Call, lib.moduleDef.Import(typeof(System.Convert).GetMethod("ToChar", new Type[] { typeof(int) }))));
                                            break;
                                        }
                                    case 1:
                                        {
                                            instrreal.Add(new Instruction(OpCodes.Ldc_I4, (int)boi));
                                            instrreal.Add(Instruction.Create(OpCodes.Call, lib.moduleDef.Import(typeof(System.Convert).GetMethod("ToChar", new Type[] { typeof(int) }))));
                                            break;
                                        }


                                }

                                instrreal.Add(Instruction.Create(OpCodes.Call, lib.moduleDef.Import(typeof(System.Convert).GetMethod("ToString", new Type[] { typeof(char) }))));
                                instrreal.Add(new Instruction(OpCodes.Stelem_Ref));
                                count++;
                            }

                            //actually add stuff
                            int num4 = 0;
                            foreach (Instruction item in instrreal)
                            {
                                method.Body.Instructions.Insert(num4, item);
                                num4++;
                            }

                            //replace original reference
                            instr.Insert(i + num4, new Instruction(OpCodes.Call, lib.moduleDef.Import(typeof(System.Text.Encoding).GetMethod("get_UTF8", new Type[] { }))));
                            instr.Insert(i + num4 + 1, new Instruction(OpCodes.Ldloc, local));
                            instr[i + num4 + 2].OpCode = OpCodes.Call;
                            instr[i + num4 + 2].Operand = lib.moduleDef.Import(typeof(System.String).GetMethod("Concat", new Type[] { typeof(string[]) }));
                            instr.Insert(i + num4 + 3, new Instruction(OpCodes.Call, lib.moduleDef.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) }))));
                            instr.Insert(i + num4 + 4, new Instruction(OpCodes.Callvirt, lib.moduleDef.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) }))));

                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
        }
    }
}