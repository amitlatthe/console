using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OssCash
{
    public static class FileEx
    {
        private const int DefaultBufferSize = 4096;

        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.

        private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

        public static Task<IEnumerable<string>> ReadAllLinesAsync(string inputPath, string outputPath, string errorPath)
        {
            return ReadAllLinesAsync(inputPath, outputPath, errorPath, Encoding.UTF8);
        }

        public static async Task<IEnumerable<string>> ReadAllLinesAsync(string inputPath, string outputPath, string errorPath, Encoding encoding)
        {
            var lines = new List<string>();
            string line;

            try
            {
                // Open the FileStream with the same FileMode, FileAccess
                // and FileShare as a call to File.OpenText would've done.
                using (var stream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
                {
                    using (var reader = new StreamReader(stream, encoding))
                    {
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            if (line.Contains("oss", System.StringComparison.CurrentCultureIgnoreCase))
                            {

                                string[] text = line.Split(new[] { ",", ": " }, StringSplitOptions.RemoveEmptyEntries);

                                string csv = string.Format("{0}, {1}, {2}{3}", text[0], text[2], DateTime.Now, Environment.NewLine);

                                File.AppendAllText(outputPath, csv);

                                lines.Add(line);

                            }
                        }

                    }
                }

            }
            catch (System.Exception e)
            {
                ErrorLogging(errorPath, e);
            }

            return lines;
        }

        public static void ErrorLogging(string errorPath, Exception ex)
        {
            if (!File.Exists(errorPath))
            {
                File.Create(errorPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(errorPath))
            {
                sw.WriteLine("-----Error Logging-----");
                sw.WriteLine("-----Start----- " + DateTime.Now);
                sw.WriteLine("Error Message: " + ex.Message);
                sw.WriteLine("Stack Trace: " + ex.StackTrace);
                sw.WriteLine("-----End----- " + DateTime.Now);

            }
        }

    }
}
