using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RodOfDiscordKeybind
{
    internal class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.RodOfDiscordKeybind.Config.Header")]

        [DefaultValue(false)]
        [ReloadRequired]
        public bool VoidBagEnabled { get; set; }
    }
}
