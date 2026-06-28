using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class EternalBlade
    {
        public const string Guid = "9DC45444-3F03-4BF3-BB73-64AC6E14DEC2";

        public static void Configure()
        {
            ArchetypeConfigurator.New("EternalBlade", Guid, SwordsageC.Guid)
                .SetLocalizedName("EternalBlade.Name")
                .SetLocalizedDescription("EternalBlade.Desc")
                .Configure();
        }
    }
}