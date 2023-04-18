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
    public class AntiDebuggerz
    {
        public void modifythemodule(OliviaLib lib, string nameof, string targetText)
        {
            var typeModule = ModuleDefMD.Load(typeof(Runtime.MethodInvoker).Module);
            var Method = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.MethodInvoker).MetadataToken));
            var members = InjectHelper.Inject(Method, lib.moduleDef.GlobalType, lib.moduleDef);
            for (int i = 0; i < lib.thefinalcctor.Body.Instructions.Count; i++)
            {
                if (lib.thefinalcctor.Body.Instructions[i].OpCode == OpCodes.Ldstr && lib.thefinalcctor.Body.Instructions[i].Operand.ToString() == targetText)
                    lib.thefinalcctor.Body.Instructions[i].Operand = nameof;
            }
        }
        public AntiDebuggerz(OliviaLib lib) => Main(lib);
        void Main(OliviaLib lib)
        {
            var module = lib.moduleDef;
            var typeModule = ModuleDefMD.Load(typeof(Runtime.AntiDebugFirst).Module);
            var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.AntiDebugFirst).MetadataToken));
            var members = InjectHelper.Inject(typeDefs, module.GlobalType, module);
            var init = (MethodDef)members.Single(method => method.Name == "Initialize");
            init.Name = Renamer.GenerateString();
            modifythemodule(lib, init.Name, "<InjectAntiDebug>");
            //lib.ctor.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(init));

        }
        
    }
}
