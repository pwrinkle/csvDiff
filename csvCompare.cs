using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csvDifferences
{
    class csvCompare
    {
        public csv a;
        public csv b;
        public List<int> colIndexToCompare;
        public int colIndexToGroupBy;
        public Dictionary<string, string> bufferFound;
        public List<int> indexesNotMatched;
        public List<int> indexesMatched;

        public csvCompare(csv a, csv b, string[] headersToCompare, string headerToGroupBy, string pathToResult)
        {
            this.a = a;
            this.b = b;
            this.bufferFound = new Dictionary<string, string>();
            this.indexesNotMatched = new List<int>();
            this.indexesMatched = new List<int>();
            this.colIndexToCompare = new List<int>();
            string[] aHeaders = a.headers.ToArray<string>();
            string[] bHeaders = b.headers.ToArray<string>();
            if ((aHeaders.Length == bHeaders.Length) && (headersToCompare.Length < aHeaders.Length))
            {
                foreach (string header in headersToCompare)
                {
                    if (Array.IndexOf<string>(aHeaders, header) != -1)
                    {
                        this.colIndexToCompare.Add(Array.IndexOf<string>(aHeaders, header));
                    }
                }
                if (Array.IndexOf<string>(aHeaders, headerToGroupBy) != -1)
                {
                    this.colIndexToGroupBy = Array.IndexOf<string>(aHeaders, headerToGroupBy);
                }
                for (int h = 0; h < a.rows.Count; h++)
                {
                    if (h % 100 == 0)
                    {
                        Console.WriteLine("Compared " + h + " / " + a.rows.Count);
                    }
                    List<string> row = a.rows.ElementAt(h);
                    List<string> cellsStart = new List<string>();
                    string grouping = row.ElementAt<string>(this.colIndexToGroupBy);
                    this.colIndexToCompare.ForEach(delegate(int i)
                    {
                        cellsStart.Add(row.ElementAt<string>(i));
                    });
                    bool matched = false;
                    for (int i = 0; i < b.rows.Count; i++)
                    {
                        List<string> cellsEnd = new List<string>();
                        List<string> rowB = b.rows.ElementAt(i);
                        for (int j = 0; j < this.colIndexToCompare.Count; j++)
                        {
                            cellsEnd.Add(rowB.ElementAt<string>(j));
                        }
                        List<string> afterCompareA = cellsStart.Except<string>(cellsEnd).ToList<string>();
                        List<string> afterCompareB = cellsEnd.Except<string>(cellsStart).ToList<string>();
                        if (afterCompareA.Count == 0 && afterCompareB.Count == 0)
                        {
                            matched = true;
                            this.indexesMatched.Add(h);
                            break;
                        }
                    }
                    if (!matched)
                    {
                        this.indexesNotMatched.Add(h);
                    }
                }
                if (this.indexesNotMatched.Count > 0)
                {
                    List<string> rowsToWrite = new List<string>();
                    rowsToWrite.Add(a.getHeaderForWrite());
                    for (int i = 0; i < this.indexesNotMatched.Count; i++)
                    {
                        rowsToWrite.Add(a.getRowForWrite(i));
                    }
                    string[] rowsArray = rowsToWrite.ToArray<string>();
                    System.IO.File.WriteAllLines(pathToResult, rowsArray);
                }
            }
        }
    }
}
