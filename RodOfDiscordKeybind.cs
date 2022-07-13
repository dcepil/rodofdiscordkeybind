using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace RodOfDiscordKeybind
{
    public class RodOfDiscordKeybind : Mod
    {
        public static ModKeybind Hotkey { get; private set; }

        public override void Load()
        {
            Hotkey = KeybindLoader.RegisterKeybind(this, "Rod of Discord", Keys.Q);
        }

        public override void Unload()
        {
            Hotkey = null;
        }
    }

    public class RodOfDiscordKeybindPlayer : ModPlayer
    {
        private int _previousItem;
        private bool _revertItem;
        private const int RodOfDiscordId = ItemID.RodofDiscord;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (RodOfDiscordKeybind.Hotkey.JustPressed && Player.HasItem(RodOfDiscordId) &&
                Player.ItemTimeIsZero && Player.ItemAnimationEndingOrEnded)
            {
                _previousItem = Player.selectedItem;
                _revertItem = true;
                Player.selectedItem = Player.FindItem(RodOfDiscordId);
                Player.controlUseItem = true;
                Player.ItemCheck(Main.myPlayer);
            }
        }

        public override void PostUpdate()
        {
            if (_revertItem && Player.ItemTimeIsZero && Player.ItemAnimationEndingOrEnded)
            {
                Player.selectedItem = _previousItem;
                _revertItem = false;
            }
        }
    }
}