using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    class file
    {
        private static file instance = null;
        public static file Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new file();
                }
                return instance;
            }
        }

        public string[] fileLDLine(string filename)
        {
            try
            {

                int row_num = GetRows(filename);
                if (row_num == 0) throw new IOException();

                string[] LDLine = new string[row_num];
                int counter = 0;
                string line;

                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    LDLine[counter] = line;
                    counter++;
                }
                return LDLine;
            }
            catch (IOException)
            {
                MessageBox.Show("UNABLE TO REACH THE FILE :" +filename, "IOException ERROR");
            }
            return null;
        }


        public int GetRows(string FilePath)
        {
            try
            {
                using (StreamReader read = new StreamReader(FilePath, Encoding.Default))
                {
                    return read.ReadToEnd().Split('\n').Length;
                }
            }

            catch
            {
                return 0;
            }
           
        }
    }
}
