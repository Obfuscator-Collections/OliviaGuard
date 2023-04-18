using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OliviaGuard.Protector.Protections
{
    public class JControlFLow
    {
        public JControlFLow(OliviaLib lib) => Main(lib);
        void Main(OliviaLib lib)
        {
            foreach (var type in lib.moduleDef.Types)
            {
                foreach (var method in type.Methods.ToArray())
                {
                    if (!method.HasBody || !method.Body.HasInstructions || method.Body.HasExceptionHandlers) continue;
                    for (var i = 0; i < method.Body.Instructions.Count - 2; i++)
                    {
                        var inst = method.Body.Instructions[i + 1];
                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldc_I4, Convert.ToInt32(Renamer.GenerateNumbersOnly())));
                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Br_S, inst));
                        i += 2;
                    }
                }
            }
        }
        
    }
}
