using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BCCL.MvvmLight;
using TEditXNA.Terraria.Objects;

namespace TEditXNA.Terraria
{
    [Serializable]
    public class Item : ObservableObject
    {
        private const int MaxStackSize = 255;

        private int _stackSize;
        private byte _prefix;
        private int _netId;

        private ushort _modId;


        public int NetId
        {
            get { return _netId; }
            set
            {
                Set("NetId", ref _netId, value);
                _currentItemProperty = World.ItemProperties.FirstOrDefault(x => x.Id == _netId);
                if (_netId == 0)
                    StackSize = 0;
                else
                {
                    if (StackSize == 0)
                        StackSize = 1;
                }
            }
        }

        public void SetFromName(string name)
        {
            var curItem = World.ItemProperties.FirstOrDefault(x => x.Name == name);
            NetId = curItem.Id;
            if (NetId != 0)
            StackSize = 1;
        }

        public string GetName()
        {
            if (_currentItemProperty != null)
                return _currentItemProperty.Name;

            return "[empty]";
        }

        public byte Prefix
        {
            get { return _prefix; }
            set { Set("Prefix", ref _prefix, value); }
        }

        public ushort ModId
        {
            get { return _modId; }
            set { Set("ModId", ref _modId, value); }
        }

        public Item()
        {
            StackSize = 0;
            NetId = 0;
            ModId = 0;
        }

        public Item(int stackSize, int netId)
        {
            StackSize = stackSize;
            NetId = stackSize > 0 ? netId : 0;
        }

        

        private ItemProperty _currentItemProperty;
        public int StackSize
        {
            get { return _stackSize; }
            set
            {
                int max = MaxStackSize;
                if (_currentItemProperty != null && _currentItemProperty.MaxStackSize > 0)
                {
                    max = _currentItemProperty.MaxStackSize;
                }

                int validValue = value;
                if (validValue > max)
                    validValue = max;
                if (validValue < 0)
                    validValue = 0;

                Set("StackSize", ref _stackSize, validValue);
            }
        }

        public Item Copy()
        {
            return new Item(_stackSize, _netId) { Prefix = _prefix };
        }

        public Visibility IsVisible
        {
            get { return _netId == 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        public override string ToString()
        {
            if (StackSize > 0)
                return string.Format("{0}: {1}", _currentItemProperty.Name, StackSize);

            return _currentItemProperty.Name;
        }
    }
}