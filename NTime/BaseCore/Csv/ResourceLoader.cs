using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv
{
    public class ResourceLoader
    {
        /// <summary>
        /// Loads provided resources to mainPath and returns an IEnumerable with full paths to the files
        /// </summary>
        /// <param name="filesAndPathDictionary"></param>
        /// <param name="mainPath"></param>
        /// <returns></returns>
        public IEnumerable<string> LoadFilesToTemp(Dictionary<string, string> filesAndPathDictionary, string mainPath)
        {
            foreach (KeyValuePair<string, string> dpv in filesAndPathDictionary)
            {
                if (File.Exists(mainPath + dpv.Key))
                    File.Delete(mainPath + dpv.Key);

                using (FileStream fs = File.Create(mainPath + dpv.Key))
                {
                    byte[] tb = new UTF8Encoding(true).GetBytes(dpv.Value);
                    fs.Write(tb, 0, tb.Length);
                }
            }

            return filesAndPathDictionary.Select(item => mainPath + item.Key);
        }
    }
}
