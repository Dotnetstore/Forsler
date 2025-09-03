using ColourMemory.ViewModels;

namespace ColourMemory.Tests;

public class MainViewModelTests
{
    [Fact]
    public void StartGame_ShouldInitializeCards()
    {
        // Arrange
        var vm = new MainViewModel();
        
        // Act
        vm.StartGameCommand.Execute(null);

        // Assert
        Assert.Equal(16, vm.Cards.Count);
        Assert.Equal(0, vm.Score);
    }

    [Fact]
    public async Task MatchingPair_ShouldIncreaseScoreAndRemoveCards()
    {
        // Arrange
        var vm = new MainViewModel();
        
        // Act
        vm.StartGameCommand.Execute(null);

        var first = vm.Cards[0];
        var second = vm.Cards.First(c => c.PairId == first.PairId && c != first);

        vm.CardClickCommand.Execute(first);
        vm.CardClickCommand.Execute(second);

        await Task.Delay(1600);

        // Assert
        Assert.Equal(1, vm.Score);
        Assert.DoesNotContain(first, vm.Cards);
        Assert.DoesNotContain(second, vm.Cards);
    }
}