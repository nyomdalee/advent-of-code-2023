namespace Four.Models;
internal record Card(int Id, IEnumerable<int> WinningNumbers, IEnumerable<int> HeldNumbers);
