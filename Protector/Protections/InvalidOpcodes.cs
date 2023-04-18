using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
    public class InvalidOpcodes
    {
        public InvalidOpcodes(OliviaLib lib) => Main(lib);
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(6)]).ToArray());
        }
        void Main(OliviaLib lib)
        {
            foreach (TypeDef typeDef in lib.moduleDef.GetTypes())
            {
                foreach (MethodDef methodDef in typeDef.Methods)
                {
                    if (methodDef.Body == null ||  methodDef.Name == ".cctor") continue;                
                    for (int i = 0; i < 6; i++)
                    {
                        if (!methodDef.HasBody || !methodDef.Body.HasInstructions) continue;
                        Random random = new Random();
                        var local = new Local(methodDef.Module.ImportAsTypeSig(typeof(string)));
                        int randomi = random.Next(0, methodDef.Body.Instructions.Count - methodDef.Body.Instructions.Count / 2);
                        int randomi2 = random.Next(0, methodDef.Body.Instructions.Count - methodDef.Body.Instructions.Count / 2);
                        methodDef.Body.Instructions.Insert(random.Next(0, methodDef.Body.Instructions.Count - methodDef.Body.Instructions.Count / 2), Instruction.Create(OpCodes.Box, methodDef.Module.Import(typeof(Math))));
                        string texto = "";
                        switch (random.Next(0, 1))
                        {
                            case 0:
                                texto = System.IO.File.ReadAllText("mytext.txt").Replace("Olivia Crashed", RandomString(12));
                                break;  

                                case 1:
                                texto = System.IO.File.ReadAllText("mytext.txt");
                                break;
                        }                        
                        methodDef.Body.Variables.Add(local);
                            methodDef.Body.Instructions.Insert(randomi2, new Instruction(OpCodes.Br_S, methodDef.Body.Instructions[randomi2]));
                        //  methodDef.Body.Instructions.Insert(randomi , new Instruction(OpCodes.Unaligned, (byte)random.Next(0,200)));
                           methodDef.Body.Instructions.Insert(randomi + 1, OpCodes.Ldstr.ToInstruction(texto));
                         methodDef.Body.Instructions.Insert(randomi + 2, OpCodes.Pop.ToInstruction());
                    }
                    //methodDef.Body.Instructions.Insert(randomi + 1, Instruction.Create(OpCodes.Box,texto));

                }
            }
        }
    }
}