using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sqo;

namespace SiaqodbExamples
{
    public class SiaqodbFactoryExample
    {
        internal static string siaoqodbPath;
        private static Siaqodb instance;

        public static void SetDBFolder(string folder)
        {
            siaoqodbPath = folder;
        }
        public static Siaqodb GetInstance()
        {
            if (instance == null)
            {
                instance = new Siaqodb(siaoqodbPath);
            }
            return instance;
        }
        public static void CloseDatabase()
        {
            if (instance != null)
            {
                instance.Close();
                instance = null;
            }
        }
    }
}
