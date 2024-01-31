using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VectorGraphicsDrawer.Controls;
using VGE.Resources;

namespace VectorGraphicsDrawer.ViewModels;

public class MainViewModel : ViewModelBase
{
    [Reactive] public string SetName { get; set; }
    public AvaloniaList<RawSetElement> CurrentElements { get; set; } = new AvaloniaList<RawSetElement>();
    [Reactive] public bool IsTextureSetSelected { get; set; }
    [Reactive] public bool IsUpdateMode { get; set; }
    public bool HideLastLine { set => GridManager.Instance.SwitchHideLastLine(value); }

    #region Add new mode
    [Reactive] public string TextureName { get; set; }

    public void AddNew()
    {
        if (string.IsNullOrEmpty(TextureName))
            return;

        if (CurrentElements.Any(x => x.Name == TextureName))
            return;

        CurrentElements.Add(new RawSetElement() 
        { 
            Name = TextureName, 
            Shapes = GridManager.Instance.Build()
        });
        TextureName = string.Empty;

        GridManager.Instance.Clear();
    }

    public void AddNewShape() =>GridManager.Instance.AddShape();
    #endregion

    #region Update mode
    int _currentElementIndex;
    public int CurrentElementIndex 
    {
        get => _currentElementIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentElementIndex, value);
            ChangedLetter();
        }
    }

    [Reactive] public RawSetElement CurrentElement { get; set; } 

    public void Delete()
    {
        CurrentElements.RemoveAt(CurrentElementIndex);
        CurrentElementIndex = 0;
    }

    void ChangedLetter()
    {
        if (CurrentElements.Count == 0)
            return;

        //CurrentElement aktulizuje się później od CurrentElementIndex
        //Wykorzystuje to w zapisie
        SaveCurrent();
        LoadNew();
    }

    void SaveCurrent()
    {
        var oldIndex = CurrentElements.IndexOf(CurrentElement);

        if (oldIndex == -1)
            return;

        var temp = CurrentElements[oldIndex];
        temp.Shapes = GridManager.Instance.Build();
        CurrentElements[oldIndex] = temp;
    }

    void LoadNew() 
    {
        if (CurrentElementIndex == -1)
            return;

        GridManager.Instance.SetShape(CurrentElements[CurrentElementIndex].Shapes); 
    }
    #endregion

    public void Undo() => GridManager.Instance.Undo();
    public void Clear() => GridManager.Instance.Clear();

    #region Managing Sets
    public async void LoadSet()
    {
        string result = string.Empty;

        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var files = await desktop.MainWindow!.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions());

            if (files.Count == 0)
                return;

            string? tempResult = files[0].Path.LocalPath;

            if (result is null)
                return;

            result = tempResult;
        }


        string text = File.ReadAllText(result);

        var set = JsonConvert.DeserializeObject<ShapeSet>(text);

        SetName = set.Name;

        foreach (var element in set.Set)
            CurrentElements.Add(new RawSetElement()
            {
                Name = element.Key,
                Shapes = element.Value,
            });

        IsTextureSetSelected = true;
        GridManager.Instance.SwitchDrawing(false);
    }

    public void SaveSet()
    {
        Dictionary<string, RawShape[]> set = new Dictionary<string, RawShape[]>();

        foreach (var element in CurrentElements)
            set[element.Name] = element.Shapes;

        string json = JsonConvert.SerializeObject(new ShapeSet()
        {
            Name = SetName,
            Set = set,
        });
        Directory.CreateDirectory("./Builds/");
        File.WriteAllText($"./Builds/{SetName}.json", json);
    }

    public void NewSet()
    {
        if (string.IsNullOrEmpty(SetName))
            return;

        IsTextureSetSelected = true;
        GridManager.Instance.SwitchDrawing(false);
    }

    public void SwitchMode(object mode) => SwitchMode(Convert.ToBoolean(Convert.ToInt32(mode)));
    void SwitchMode(bool mode)
    {
        GridManager.Instance.Clear();

        if (mode)
            LoadNew();

        TextureName = string.Empty;
        IsUpdateMode = mode;
    }
    #endregion
}

public struct RawSetElement
{
    public string Name { get; set; }
    public RawShape[] Shapes { get; set; }
}