using System.Collections.ObjectModel;
using System.Windows;
using ColourMemory.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColourMemory.ViewModels;

public sealed class MainViewModel : ObservableObject, IMainViewModel
{
    private const int DelayTimeBetweenMoves = 1500;
    private readonly Random _rng = new();
    private CardViewModel? _firstCard;
    private CardViewModel? _secondCard;
    
    public ObservableCollection<CardViewModel> Cards { get; } = new();
    
     private int _score;
     public int Score
     {
         get => _score;
         set => SetProperty(ref _score, value);
     }
     
     private bool _isBusy;
     public bool IsBusy
     {
         get => _isBusy;
         set
         {
             if (SetProperty(ref _isBusy, value))
                 (CardClickCommand as RelayCommand<CardViewModel>)!.NotifyCanExecuteChanged();
         }
     }
    
    public IRelayCommand StartGameCommand { get; set; }
    public IRelayCommand<CardViewModel> CardClickCommand { get; set; }
    
    public MainViewModel()
    {
        StartGameCommand = new RelayCommand(StartGame);
        CardClickCommand = new RelayCommand<CardViewModel>(OnCardClicked, CanClickCard);
    }

    private void StartGame()
    {
         Score = 0;
         IsBusy = false;
         _firstCard = _secondCard = null;
         Cards.Clear();

         string[] palette =
         {
             "#FFEF4444", // red
             "#FF3B82F6", // blue
             "#FF10B981", // emerald
             "#FFF59E0B", // amber
             "#FFA855F7", // purple
             "#FFEC4899", // pink
             "#FF22C55E", // green
             "#FF06B6D4"  // cyan
         };

         var deck = palette
             .SelectMany((c, idx) => new[]
             {
                 new MemoryCard { Color = c, PairId = idx },
                 new MemoryCard { Color = c, PairId = idx }
             })
             .OrderBy(_ => _rng.Next())
             .ToList();

         foreach (var card in deck)
             Cards.Add(new CardViewModel(card));
    }

     private bool CanClickCard(object? param)
     {
         if (IsBusy || param is not CardViewModel card) return false;
         if (card.IsRemoved || card.IsRevealed) return false;
         return true;
     }

     private async void OnCardClicked(CardViewModel? card)
     {
         if(card is null) return;
         if (!CanClickCard(card)) return;

         card.IsRevealed = true;

         if (_firstCard is null)
         {
             _firstCard = card;
             return;
         }

         if (_secondCard is not null) return;
         _secondCard = card;
         IsBusy = true;

         await Task.Delay(DelayTimeBetweenMoves);

         if (_firstCard.PairId == _secondCard.PairId)
         {
             ItsAMatch();
         }
         else
         {
             ItsNotAMatch();
         }

         FinishUpRound();
     }
     
     private void ItsAMatch()
     {
         Score += 1;

         Cards.Remove(_firstCard!);
         Cards.Remove(_secondCard!);
     }
     
     private void ItsNotAMatch()
     {
         Score -= 1;
         _firstCard!.IsRevealed = false;
         _secondCard!.IsRevealed = false;
     }
     
     private void FinishUpRound()
     {
         _firstCard = _secondCard = null;
         IsBusy = false;

         if (Cards.Count == 0)
         {
             MessageBox.Show("Congratulations! You've matched all the cards!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
         }
     }
}