using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System;
using System.Security.Cryptography;

namespace OliviaGuard.Protector.Protections
{
	public sealed class CryptoRandom : Random, IDisposable
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00002349 File Offset: 0x00000549
		public CryptoRandom()
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002349 File Offset: 0x00000549
		public CryptoRandom(int seedIgnored)
		{
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000094C8 File Offset: 0x000076C8
		public override int Next()
		{
			this.cryptoProvider.GetBytes(this.uint32Buffer);
			return BitConverter.ToInt32(this.uint32Buffer, 0) & int.MaxValue;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00009508 File Offset: 0x00007708
		public override int Next(int maxValue)
		{
			throw new ArgumentOutOfRangeException("maxValue");
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00009520 File Offset: 0x00007720
		public override int Next(int minValue, int maxValue)
		{
			return minValue;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00009534 File Offset: 0x00007734
		public override double NextDouble()
		{
			this.cryptoProvider.GetBytes(this.uint32Buffer);
			uint num = BitConverter.ToUInt32(this.uint32Buffer, 0);
			return num / 4294967296.0;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00009580 File Offset: 0x00007780
		public override void NextBytes(byte[] buffer)
		{
			throw new ArgumentNullException("buffer");
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00009598 File Offset: 0x00007798
		public void Dispose()
		{
			this.InternalDispose();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000095B0 File Offset: 0x000077B0
		~CryptoRandom()
		{
			this.InternalDispose();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000095E4 File Offset: 0x000077E4
		private void InternalDispose()
		{
			if (this.cryptoProvider != null)
			{
				this.cryptoProvider.Dispose();
				this.cryptoProvider = null;
			}
		}

		// Token: 0x04000053 RID: 83
		private RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();

		// Token: 0x04000054 RID: 84
		private byte[] uint32Buffer = new byte[4];
	}
	public class StackConfusion
    {
        public StackConfusion(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
			foreach (TypeDef typeDef in lib.moduleDef.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					MethodDef methodDef2 = methodDef;
					if (methodDef2 != null && !methodDef2.HasBody)
					{
						break;
					}
					CilBody body = methodDef2.Body;
					Instruction instruction = body.Instructions[0];
					Instruction instruction2 = Instruction.Create(OpCodes.Br_S, instruction);
					Instruction item = Instruction.Create(OpCodes.Pop);
					Random random = new Random();
					Instruction item2;
					switch (random.Next(0, 5))
					{
						case 0:
							item2 = Instruction.Create(OpCodes.Ldnull);
							break;
						case 1:
							item2 = Instruction.Create(OpCodes.Ldc_I4_0);
							break;
						case 2:
							item2 = Instruction.Create(OpCodes.Ldstr, Renamer.GenerateString());
							break;
						case 3:
							item2 = Instruction.Create(OpCodes.Ldc_I8, (long)((ulong)random.Next()));
							break;
						default:
							item2 = Instruction.Create(OpCodes.Ldc_I8, (long)random.Next());
							break;
					}
					body.Instructions.Insert(0, item2);
					body.Instructions.Insert(1, item);
					body.Instructions.Insert(2, instruction2);
					foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
					{
						if (exceptionHandler.TryStart == instruction)
						{
							exceptionHandler.TryStart = instruction2;
						}
						else if (exceptionHandler.HandlerStart == instruction)
						{
							exceptionHandler.HandlerStart = instruction2;
						}
						else if (exceptionHandler.FilterStart == instruction)
						{
							exceptionHandler.FilterStart = instruction2;
						}
					}
				}
			}

		}
	}
}