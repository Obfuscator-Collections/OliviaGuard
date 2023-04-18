using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
    public class MethodHider
    {
        public MethodHider(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
            var module = lib.moduleDef;

            var typeModule = ModuleDefMD.Load(typeof(Runtime.MethodHider).Module);
            var typeDefs = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.MethodHider).MetadataToken));

            var members = InjectHelper.Inject(typeDefs, module.GlobalType, module);
            var init = (MethodDef)members.Single(method => method.Name == "MethodHiderInj");
            init.Name = Renamer.GenerateString();
            foreach (TypeDef type in module.GetTypes())
            {
                if (type.IsGlobalModuleType) continue;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody || !method.Body.HasInstructions) continue;
                    for (int i = 0; i < method.Body.Instructions.Count; ++i)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Call)
                        {
                            if (method.Body.Instructions[i].Operand is MethodDef)
                            {
                                try
                                {
                                   // if (method.Name != "CopyAll") continue;
                                    var operand = (MethodDef)method.Body.Instructions[i].Operand;
                                    if (operand.Parameters.Count == 0) continue;
                                    if (operand.DeclaringType == module.Import(typeof(void))) continue;
                                    if (operand.IsConstructor) continue;
                                    if (!operand.IsStatic) continue;

                                    int plus = 1;

                                    Local local = new Local(module.ImportAsTypeSig(typeof(object[])));
                                    method.Body.Variables.Add(local);
                                    method.Body.Instructions.Insert(i + plus, OpCodes.Ldc_I4.ToInstruction(operand.Parameters.Count));
                                    method.Body.Instructions.Insert(i + ++plus, OpCodes.Newarr.ToInstruction(module.CorLibTypes.Object));
                                    method.Body.Instructions.Insert(i + ++plus, OpCodes.Dup.ToInstruction());

                                    for (int e = 0; e < operand.Parameters.Count; ++e)
                                    {

                                        if (method.Body.Instructions[i - e - 1].OpCode == OpCodes.Call)
                                        {
                                            method.Body.Instructions.RemoveAt(i + plus);
                                            method.Body.Instructions.RemoveAt(i + --plus);
                                            method.Body.Instructions.RemoveAt(i + --plus);
                                            break;
                                        }
                                        if (method.Body.Instructions[i - e - 1].OpCode == OpCodes.Nop)
                                        {
                                            continue;
                                        }
                                        method.Body.Instructions.Insert(i + ++plus, OpCodes.Ldc_I4.ToInstruction(e));
                                        method.Body.Instructions.Insert(i + ++plus, method.Body.Instructions[i - e - 1]);
                                        method.Body.Instructions.Insert(i + ++plus, OpCodes.Stelem_Ref.ToInstruction());
                                        if (operand.Parameters.Count - 1 != e)
                                            method.Body.Instructions.Insert(i + ++plus, OpCodes.Dup.ToInstruction());

                                    }

                                    method.Body.Instructions.Insert(i + ++plus, OpCodes.Ldc_I4.ToInstruction(operand.MDToken.ToInt32()));
                                    method.Body.Instructions.Insert(i + ++plus, OpCodes.Call.ToInstruction(init));
                                    i += plus + 1;
                                    break;
                                }
                                catch { }
                            }
                        }
                    }

                }
            }
        }
    }
}