using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using OliviaGuard.Protector.Enums;
using OliviaGuard.Protector.Protections;

namespace OliviaGuard.Protector.Class
{
    public class OliviaLib
    {
        public string filePath { get; private set; }
        public string GoldName { get;  set; }
        public string GoldKey { get; set; }
        public string GoldResource { get; set; }

        public AssemblyDef assembly { get; private set; }
        public ModuleDef moduleDef { get;  set; }
        public ModuleDefMD ModuleDefMD { get; private set; }
        public TypeDef globalType { get; private set; }
        public MethodDef ctor { get;  set; }
        public MethodDef thefinalcctor { get; set; }
        public NativeModuleWriterOptions nativeModuleWriterOptions { get; private set; }
        public ModuleWriterOptions moduleWriterOptions { get; private set; }
        public bool noThrowInstance { get; set; }


        public OliviaLib(string file)
        {
            filePath = file;

            assembly = AssemblyDef.Load(file);
            moduleDef = assembly.ManifestModule;
            ModuleDefMD = moduleDef as ModuleDefMD; 
            globalType = assembly.ManifestModule.GlobalType;
            thefinalcctor = assembly.ManifestModule.GlobalType.FindOrCreateStaticConstructor();
            string nameof = Renamer.GenerateString();
            ctor = new MethodDefUser(nameof, dnlib.DotNet.MethodSig.CreateStatic(ModuleDefMD.CorLibTypes.Void),
                   MethodImplAttributes.IL | MethodImplAttributes.Managed,
                   MethodAttributes.Public | MethodAttributes.Static);
            CilBody cilBody = new CilBody();
            cilBody.Instructions.Add(OpCodes.Ret.ToInstruction());
            ctor.Body = cilBody;
            moduleDef.GlobalType.Methods.Add(ctor);
            //CurrentListener.OnWriterEvent += (sender, e) => Listener();
            noThrowInstance = false;

            nativeModuleWriterOptions = new NativeModuleWriterOptions(moduleDef as ModuleDefMD, true)
            {
                MetadataLogger = DummyLogger.NoThrowInstance
            };

            moduleWriterOptions = new ModuleWriterOptions(moduleDef as ModuleDef)
            {
                MetadataLogger = DummyLogger.NoThrowInstance
            };
        }

        void Listener() { }

        string NewName()
        {
            return Path.GetDirectoryName(filePath) + "//" + Path.GetFileNameWithoutExtension(filePath) + "_" + "Olivied" + Path.GetExtension(filePath);
        }

        public void buildASM(saveMode mode)
        {
            if (mode == saveMode.Normal)
            {
                moduleWriterOptions.MetadataOptions.Flags = MetadataFlags.AlwaysCreateStringsHeap | MetadataFlags.AlwaysCreateBlobHeap | MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.AlwaysCreateUSHeap;
                moduleDef.Write(NewName(), moduleWriterOptions);
            }
            else if (mode == saveMode.x86)
            {
                nativeModuleWriterOptions.MetadataOptions.Flags = MetadataFlags.AlwaysCreateStringsHeap | MetadataFlags.AlwaysCreateBlobHeap | MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.AlwaysCreateUSHeap;
                (moduleDef as ModuleDefMD).NativeWrite(NewName(), nativeModuleWriterOptions);
            }
        }
    }
}