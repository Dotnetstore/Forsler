using CommunityToolkit.Mvvm.ComponentModel;

namespace ColourMemory.Models;

public class CardViewModel(MemoryCard model) : ObservableObject
{
    public MemoryCard Model { get; } = model;
    public int PairId => Model.PairId;
    public string Color => Model.Color;

    private bool _isRevealed;
    public bool IsRevealed
    {
        get => _isRevealed;
        set => SetProperty(ref _isRevealed, value);
    }

    private bool _isRemoved;
    public bool IsRemoved
    {
        get => _isRemoved;
        set => SetProperty(ref _isRemoved, value);
    }
}