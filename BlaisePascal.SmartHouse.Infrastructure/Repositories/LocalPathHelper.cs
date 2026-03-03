namespace BlaisePascal.SmartHouse.Infrastructure.Repositories
{
    public class LocalPathHelper
    {
        public static string GetSolutionRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);

            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "src")))
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception("Solution root not found");

            return dir.FullName;
        }
    }
}
