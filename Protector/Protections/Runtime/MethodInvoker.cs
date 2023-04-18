using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OliviaGuard.Protector.Protections.Runtime
{
    internal static class MethodInvoker
    {
        private static unsafe void Invoker()
        {
            int num = 0x75113256;
            var p = Assembly.GetCallingAssembly();
            var k = Assembly.GetExecutingAssembly();
            if (p.Equals(k))
                num = 0;

            string st1 = "<OliviaGuard></OliviaGuard>";
            string st2 = "<InjectAntiDebug>";
            List<Type> list2 = new List<Type>();
            DynamicMethod dynamicMethod2 = new DynamicMethod(st1, typeof(void), list2.ToArray(), typeof(MethodInvoker).Module, true);
            ILGenerator ilgenerator2 = dynamicMethod2.GetILGenerator();
            ilgenerator2.Emit(OpCodes.Jmp, typeof(MethodInvoker).GetMethod(st2, BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType | BindingFlags.OptionalParamBinding | BindingFlags.IgnoreReturn, null, new Type[0], null));
            ilgenerator2.Emit(OpCodes.Ret);
            dynamicMethod2.Invoke(null, new object[num]);


            string st3 = "<<OliviaGuard></OliviaGuard>";
            string st4 = "<Injectmehere>";
            List<Type> list = new List<Type>();
            DynamicMethod dynamicMethod = new DynamicMethod(st3, typeof(void), list.ToArray(), typeof(MethodInvoker).Module, true);
            ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
            ilgenerator.Emit(OpCodes.Jmp, typeof(MethodInvoker).GetMethod(st4, BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType | BindingFlags.OptionalParamBinding | BindingFlags.IgnoreReturn, null, new Type[0], null));
            ilgenerator.Emit(OpCodes.Ret);
            dynamicMethod.Invoke(null, new object[num]);

        }
    }
}
