using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
    public class OpCodeProtection
    {
        public OpCodeProtection(OliviaLib lib) => Main(lib);
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(6)]).ToArray());
        }
		private static void CtorCallProtection(MethodDef method)
		{
			IList<Instruction> instr = method.Body.Instructions;
			for (int i = 0; i < instr.Count; i++)
			{
				if (instr[i].OpCode == OpCodes.Call && instr[i].Operand.ToString().ToLower().Contains("void") && i - 1 > 0 && instr[i - 1].IsLdarg())
				{
					Local new_local = new Local(method.Module.CorLibTypes.Int32);
					method.Body.Variables.Add(new_local);
					instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
					instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
					instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
					instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 6, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 3, new Instruction(OpCodes.Bne_Un_S, instr[i + 4]));
					instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
					instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
				}
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00031FA4 File Offset: 0x000301A4
		private static void LdfldProtection(MethodDef method)
		{
			IList<Instruction> instr = method.Body.Instructions;
			for (int i = 0; i < instr.Count; i++)
			{
				if (instr[i].OpCode == OpCodes.Ldfld && i - 1 > 0 && instr[i - 1].IsLdarg())
				{
					Local new_local = new Local(method.Module.CorLibTypes.Int32);
					method.Body.Variables.Add(new_local);
					instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
					instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
					instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
					instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 6, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
					instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
					instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
				}
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0003211C File Offset: 0x0003031C
		private static void CallvirtProtection(MethodDef method)
		{
			IList<Instruction> instr = method.Body.Instructions;
			for (int i = 0; i < instr.Count; i++)
			{
				if (instr[i].OpCode == OpCodes.Callvirt && instr[i].Operand.ToString().ToLower().Contains("int32") && i - 1 > 0 && instr[i - 1].IsLdloc())
				{
					Local new_local = new Local(method.Module.CorLibTypes.Int32);
					method.Body.Variables.Add(new_local);
					instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
					instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
					instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(OpCodeProtection.random.Next()));
					instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
					instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 6, OpCodes.Nop.ToInstruction());
					instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
					instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
					instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
				}
			}
		}

		// Token: 0x04000D68 RID: 3432
		private static Random random = new Random();
		void Main(OliviaLib lib)
        {
			foreach (TypeDef type in lib.moduleDef.GetTypes().ToArray())
			{
				if (type.IsDelegate)
					continue;
				if (type.IsGlobalModuleType)
					continue;
				if (type.Namespace == "Costura")
					continue;
				foreach (MethodDef method in type.Methods.ToArray())
				{
					if (!method.HasBody)
						continue;
					if (!method.Body.HasInstructions)
						continue;
					if (method.IsConstructor)
						continue;

					OpCodeProtection.LdfldProtection(method);
                    OpCodeProtection.CallvirtProtection(method);
                    OpCodeProtection.CtorCallProtection(method);
                }
            }
        }
    }
}