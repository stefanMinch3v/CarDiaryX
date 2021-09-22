namespace CarDiaryX.Application.Common.Models
{
    /// <summary>
    /// Dawa address model format
    /// </summary>
    public interface IAddress
    {
        public string Name { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }
}
