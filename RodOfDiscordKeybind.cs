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
        private int _rodItemIndex;
        private int _previousItemIndex;
        private bool _revertItemIndex;
        private Item _swapItem;
        private Item _rodItem;
        private bool _isInVoidBag;
        private readonly bool _voidBagEnabled = ModContent.GetInstance<Config>().VoidBagEnabled;
        private const int RodOfDiscordId = ItemID.RodofDiscord;
        private const int RodOfHarmonyId = ItemID.RodOfHarmony;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (RodOfDiscordKeybind.Hotkey.JustPressed &&
                Player.ItemTimeIsZero && Player.ItemAnimationEndingOrEnded)
            {
                if (_voidBagEnabled ?
                     Player.HasItemInInventoryOrOpenVoidBag(RodOfHarmonyId) : Player.HasItem(RodOfHarmonyId))
                {
                    HandleRodUsage(RodOfHarmonyId);
                }
                else if (_voidBagEnabled ?
                    Player.HasItemInInventoryOrOpenVoidBag(RodOfDiscordId) : Player.HasItem(RodOfDiscordId))
                {
                    HandleRodUsage(RodOfDiscordId);
                }
            }
        }

        public override void PostUpdate()
        {
            if (_revertItemIndex && Player.ItemTimeIsZero && Player.ItemAnimationEndingOrEnded)
            {
                // Clean up after using the Rod Item - swap the items back around and cleanup vars
                if (_isInVoidBag)
                {
                    Player.bank4.item[_rodItemIndex] = _rodItem;
                    Player.inventory[_rodItemIndex] = _swapItem;
                    _swapItem = null;
                    _rodItem = null;
                    _isInVoidBag = false;
                }

                Player.selectedItem = _previousItemIndex;
                _revertItemIndex = false;
            }
        }

        private void HandleRodUsage(int rodId)
        {
            _previousItemIndex = Player.selectedItem;
            _revertItemIndex = true;
            _rodItemIndex = _voidBagEnabled ?
                Player.FindItemInInventoryOrOpenVoidBag(rodId, out _isInVoidBag) : Player.FindItem(rodId);
            /*
                FindItemInInventoryOrOpenVoidBag() will return -1 if it can't find the item.
                For some reason it will falsely return -1 even when the item is there
                when the player doesn't have an item at that index in their inventory,
                so we have to check the Void Bag manually also,
                but only if the above fails AND the Void Bag is open and enabled.
            */
            if (_voidBagEnabled && _rodItemIndex == -1 && Player.IsVoidVaultEnabled)
            {
                var manuallyCheckedIndex = Array.FindIndex(Player.bank4.item, i => i.netID == rodId);
                if (manuallyCheckedIndex != -1)
                {
                    // Since we only manually check the Void Bag, the Rod Item is guaranteed to be in the Void Bag
                    _isInVoidBag = true;
                    _rodItemIndex = manuallyCheckedIndex;
                }
            }

            if (_rodItemIndex != -1)
            {
                // Swap the items at the given index between the Player's main inventory and their Void Bag
                if (_isInVoidBag)
                {
                    _swapItem = Player.inventory[_rodItemIndex];
                    _rodItem = Player.bank4.item[_rodItemIndex];
                    Player.inventory[_rodItemIndex] = Player.bank4.item[_rodItemIndex];
                }

                Player.selectedItem = _rodItemIndex;
                Player.controlUseItem = true;
            }
        }
    }
}