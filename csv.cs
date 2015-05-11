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
        public Dictionary<string, int> countOfEachCompareFeild;
        public List<string> groupColumn;

        public csv(string path)
        {
            this.headers = new List<string>();
            this.rows = new List<List<string>>();
            int lineNum = 0;
            this.countOfEachCompareFeild = new Dictionary<string, int>();
            this.groupColumn = new List<string>();
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

        public void populateCounts(string headerToCompare)
        {
            int index = this.headers.IndexOf(headerToCompare);
            if (index != -1)
            {
                for (int i = 0; i < this.rows.Count; i++) 
                {
                    List<string> row = this.rows.ElementAt<List<string>>(i);
                    string col = row.ElementAt<string>(index);
                    if (this.countOfEachCompareFeild.ContainsKey(col))
                    {
                        this.countOfEachCompareFeild[col]++;
                    }
                    else
                    {
                        this.countOfEachCompareFeild.Add(col, 1);
                    }
                }
            }
        }

        public string[] getSortedListOfGroupings(string groupBy)
        {
            int index = this.headers.IndexOf(groupBy);
            if (index != -1)
            {
                foreach (List<string> row in this.rows)
                {
                    string col = row.ElementAt<string>(index);
                    this.groupColumn.Add(col);
                }
            }
            this.groupColumn.Sort();
            return this.groupColumn.ToArray<string>();
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

        public string getRowForWrite(int index, string[] headers)
        {
            List<string> row = this.rows.ElementAt<List<string>>(index);
            string result = "";
            string delimiter = "";
            foreach (string column in headers)
            {
                result += delimiter + row.ElementAt<string>(this.headers.IndexOf(column));
                if (delimiter == "")
                {
                    delimiter = ",";
                }
            }
            return result;
        }

        public string getHeaderForWrite(string[] head)
        {
            string result = "";
            string delimiter = "";
            foreach (string column in head)
            {
                int index = this.headers.IndexOf(column);
                if (index != -1)
                {
                    result += delimiter + this.headers.ElementAt<string>(index);
                    if (delimiter == "")
                    {
                        delimiter = ",";
                    }
                }
            }
            return result;
        }

        public int[] getRowIndexForValue(string header, string value)
        {
            List<int> results = new List<int>();
            int index = this.headers.IndexOf(header);
            if (index != -1)
            {
                for(int i=0;i<this.rows.Count;i++)
                {
                    List<string> row = this.rows.ElementAt<List<string>>(i);
                    string col = row.ElementAt<string>(index);
                    if (col == value)
                    {
                        results.Add(i);
                    }
                }
                return results.ToArray<int>();
            }
            else
            {
                return new int[0];
            }
        }
    }
}
