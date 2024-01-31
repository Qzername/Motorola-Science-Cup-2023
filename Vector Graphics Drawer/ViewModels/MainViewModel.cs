using ReactiveUI.Fody.Helpers;
using System;

namespace VectorGraphicsDrawer.ViewModels;

public class MainViewModel : ViewModelBase
{
    [Reactive] public bool IsUpdateMode { get; set; }

    public void SwitchMode(object mode) => IsUpdateMode = Convert.ToBoolean(Convert.ToInt32(mode));
}
