using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace csvDifferences
{
    class csv
    {
        public List<List<string>> rows;
        public List<string> headers;
        public int totalLines;

        public csv(string path)
        {
            this.headers = new List<string>();
            this.rows = new List<List<string>>();
            int lineNum = 0;
            using (StreamReader read = new StreamReader(path))
            {
                while (!read.EndOfStream)
                {
                    string[] row = read.ReadLine().Split(',');
                    if (lineNum++ == 0)
                    {
                        foreach (string cell in row)
                        {
                            this.headers.Add(cell);
                        }
                    }
                    else
                    {
                        List<string> tosave = new List<string>();
                        foreach (string cell in row)
                        {
                            tosave.Add(cell);
                        }
                        this.rows.Add(tosave);
                    }
                }
            }
            this.totalLines = lineNum;
        }

        public string getRowForWrite(int index)
        {
            List<string> row = this.rows.ElementAt<List<string>>(index);
            string result = "";
            string delimiter = "";
            row.ForEach(delegate(string cell)
            {
                result += delimiter + cell;
                if (delimiter == "")
                {
                    delimiter = ",";
                }
            });
            return result;
        }

        public string getHeaderForWrite()
        {
            string result = "";
            string delimiter = "";
            headers.ForEach(delegate(string cell)
            {
                result += delimiter + cell;
                if (delimiter == "")
                {
                    delimiter = ",";
                }
            });
            return result;
        }
    }
}
