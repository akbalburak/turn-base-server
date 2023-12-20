namespace ModuleDTOLayer
{
    public class StageLevelDTO : IStageLevelDTO
    {
        public int Stage { get; set; }
        public int Level { get; set; }
        public int PlayCount { get; set; }
        public int CompletedCount { get; set; }
    }

    public interface IStageLevelDTO
    {
        int Stage { get; }
        int Level { get; }
        int PlayCount { get; }
        int CompletedCount { get; }
    }
}
