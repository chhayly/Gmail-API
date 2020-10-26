using System.IO;
using System.Text;

namespace Gmail_Api.Helper
{
    public static class FileIOHelper
    {
        public static void WriteStringToJson(string content, string path)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + path))
            {
                var files = File.Create(Directory.GetCurrentDirectory() + "\\" + path);
                files.Close();
            }

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                stream.SetLength(0);

                byte[] bytes = Encoding.UTF8.GetBytes(content);

                stream.Write(bytes, 0, bytes.Length);

                stream.Close();
            }
        }
    }
}
