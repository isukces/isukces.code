﻿using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces.Ammy;

namespace AmmySamples
{
    internal class Program
    {
        [AmmyBuilder]
        public static void AmmyInit(AmmyBuilderContext x)
        {
            x.EmbedInRelativeFile();
            x.AddImportNamespace("AmmySamples");

            x.RegisterMixin<Sample>("SampleMixing")
                .WithProperty(a => a.Name, "Piotr");
        }

        private static void Main(string[] args)
        {
            IAssemblyFilenameProvider pro =
                new SimpleAssemblyFilenameProvider(new AssemblyBaseDirectoryProvider(), "++AutoCode.cs");

            var acg = new AutoCodeGenerator(pro);
            acg.CodeGenerators.Clear();
            acg.WithGenerator(new AmmyAutocodeGenerator(pro));
            acg.Make<Program>();
        }
    }

    public sealed class Sample
    {
        public string Name { get; set; }
    }
}