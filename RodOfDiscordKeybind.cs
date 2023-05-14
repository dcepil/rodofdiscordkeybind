using Microsoft.Xna.Framework.Input;
using System;
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
            Hotkey = KeybindLoader.RegisterKeybind(this, "Rod of Discord/Harmony", Keys.Q);
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
        private const int RodOfHarmonyId = ItemID.RodOfHarmony;

        private void HandleRodUsage(int rodId)
        {
            _previousItem = Player.selectedItem;
            _revertItem = true;
            Player.selectedItem = Player.FindItem(rodId);
            Player.controlUseItem = true;
            Player.ItemCheck();
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (RodOfDiscordKeybind.Hotkey.JustPressed &&
                Player.ItemTimeIsZero && Player.ItemAnimationEndingOrEnded)
            {
                if (Player.HasItem(RodOfHarmonyId))
                {
                    HandleRodUsage(RodOfHarmonyId);
                }
                else if (Player.HasItem(RodOfDiscordId))
                {
                    HandleRodUsage(RodOfDiscordId);
                }
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