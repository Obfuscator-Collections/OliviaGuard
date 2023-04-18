using System.Collections.Generic;
using OliviaGuard.Protector.Class;
using OliviaGuard.Protector.Protections;
using OliviaGuard.Protector.Protections.Constants;
using OliviaGuard.Protector.Protections.ControlFlow;
using OliviaGuard.Protector.Protections.ReferenceProxy;
using OliviaGuard.Protector.Protections.Mutation;

namespace OliviaGuard.Protector
{
    public class Olivia
    {
        public List<Enums.Protections> protections = new List<Enums.Protections>();

        OliviaLib lib { get; set; }
        public string GoldName { get ; set; }
        public string GoldKey { get; set; }
        public string GoldResource { get; set; }
        public Olivia(string filePath)
        {
            lib = new OliviaLib(filePath);
        }

        public void Protect()
        {
            lib.GoldName = GoldName;
            lib.GoldKey = GoldKey;
            lib.GoldResource = GoldResource;
            foreach (Enums.Protections protection in protections)
            {
                if (protection == Enums.Protections.CallConvertion)
                    new CallConvertion(lib);

                if (protection == Enums.Protections.Constants)
                    new Constants(lib);

                //   if (protection == Enums.Protections.VM)
                // new Protections.Virtualization.Virtualzation(lib);
                if (protection == Enums.Protections.JControlFlow)
                    new JControlFLow(lib);
                   if (protection == Enums.Protections.AntiDebuggerz)
                    new AntiDebuggerz(lib); 

                if (protection == Enums.Protections.ReferenceProxy)
                    new ReferenceProxy(lib);

                if (protection == Enums.Protections.ControlFlow)
                    new ControlFlow(lib);

                if (protection == Enums.Protections.InvalidOpcodes)
                    new InvalidOpcodes(lib);

                if (protection == Enums.Protections.AntiDump)
                    new AntiDump(lib);

                if (protection == Enums.Protections.AntiDebug)
                    new AntiDebug(lib);

                if (protection == Enums.Protections.Mutation)
                    new Mutation(lib);

                if (protection == Enums.Protections.MethodHider)
                    new MethodHider(lib);

                if (protection == Enums.Protections.NewMutation)
                    new NewMutation(lib);

                if (protection == Enums.Protections.ImportantProtection)
                    new ImportProtection(lib);

                if (protection == Enums.Protections.AntiDe4dot)
                    new AntiDe4dot(lib);

                if (protection == Enums.Protections.StringToArray)
                    new StringToArray(lib);

                if (protection == Enums.Protections.StackConfusion)
                    new StackConfusion(lib);

                if (protection == Enums.Protections.HideNameSpaces)
                    new HideNameSpace(lib);

                if (protection == Enums.Protections.HideMethods)
                    new HideMethods(lib);

                if (protection == Enums.Protections.Renamer2)
                    new Renamer2(lib);

                if (protection == Enums.Protections.AntiDebugFirst)
                    new AntiDebuggerz(lib);

                if (protection == Enums.Protections.ConstantMelt)
                    new MeltConstants(lib);

                if (protection == Enums.Protections.OpCodeProtection)
                    new OpCodeProtection(lib);


            }
            
                foreach (Enums.Protections protection in protections)
            {
                if (protection == Enums.Protections.PosConstants)
                    new PosConstants(lib);

                if (protection == Enums.Protections.Renamer)
                    new Renamer(lib);

                if (protection == Enums.Protections.FakeAttributes)
                    new FakeAttributes(lib);

                if (protection == Enums.Protections.WaterMark)
                    new WaterMark(lib);

                if (protection == Enums.Protections.ProAntiTamper)
                    new ProAntiTamper(lib);

                GoldName = lib.GoldName;
                GoldKey = lib.GoldKey;  
                GoldResource = lib.GoldResource;
                // if (protection == Enums.Protections.ReferenceOverload)
                // new ReferenceOverload(lib);
            }
        }

        public void Save()
        {
            lib.buildASM(Enums.saveMode.x86);
        }
    }
}