namespace ModuleDTOLayer
{
    public class UserLevelDTO : IUserLevelDTO
    {
        public int Level { get; set; }
        public int Experience { get; set; }
    }

    public interface IUserLevelDTO
    {
        public int Level { get; }
        public int Experience { get; }
    }
}
