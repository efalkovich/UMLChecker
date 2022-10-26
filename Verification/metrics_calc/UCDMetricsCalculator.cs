using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verification.cd_ver.Entities;
using Verification.package_ver;
using Verification.uc_ver;

namespace Verification.metrics_calc
{
    internal class UCDMetricsCalculator : MetricsCalculator
    {
        private int nouc;
        private int noa;
        private double nouca;
        private int ucSecond;
        private double ucThird;
        private double ucFourth;
        private int cUcd;
        private int CTE;
        private int CIE;
        public UCDMetricsCalculator(Diagram diag) : base(diag)
        {
            calcMetrics();
        }
        private void calcMetrics()
        {
            Dictionary<string, Element>  elements = new Dictionary<string, Element>();
            Reader reader = new Reader(elements, model);
            reader.ReadData(model.XmlInfo);

            List<Element> actors = new List<Element>();
            List<Element> usecases = new List<Element>();
            List<Arrow> conns = new List<Arrow>();
            foreach(var el in elements.Values)
            {
                if(el.Type == ElementTypes.Actor)
                    actors.Add(el);
                else if (el.Type == ElementTypes.Precedent)
                    usecases.Add(el);
                else if (el.Type == ElementTypes.Association || el.Type == ElementTypes.Extend || el.Type == ElementTypes.Include)
                    conns.Add((Arrow)el);
            }


            nouc = CalcNouc(usecases);
            metrics.Add("nouc", nouc);

            noa = CalcNoa(actors);
            metrics.Add("noa", noa);

            nouca = CalcNouca();
            metrics.Add("nouca", Math.Round(nouca, 2));

            CalcUcNth(usecases, actors, conns);
            metrics.Add("UC2", ucSecond);
            metrics.Add("UC3", Math.Round(ucThird, 2));
            metrics.Add("UC4", Math.Round(ucFourth, 2));

            CalcCUcd(usecases, actors, conns);
            metrics.Add("CTE", CTE);
            metrics.Add("CIE", CIE);
            metrics.Add("Cucd", cUcd);

        }
        private int CalcNouc(List<Element> uc)
        {
            return uc.Count;
        }
        private int CalcNoa(List<Element> act)
        {
            return act.Count;
        }
        private double CalcNouca()
        {
            return (double)noa / nouc;
        }
        private void CalcUcNth(List<Element> ucs, List<Element> acts, List<Arrow> conns)
        {
            int n = nouc;
            int m = noa;
            int[,] c = new int[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; i < m; i++)
                    c[i, j] = 0;

            foreach (var conn in conns.Where(ar => ar.Type == ElementTypes.Association))
            {
                int ind1 = ucs.FindIndex(uc => uc.Id == conn.To || uc.Id == conn.From);
                int ind2 = acts.FindIndex(a => a.Id == conn.From || a.Id == conn.To);
                c[ind1, ind2] = 1;
            }

            ucSecond = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    ucSecond += c[i, j];

            int[,] d = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (c[i, j] == 0)
                    {
                        d[i, j] = 0;
                        continue;
                    }

                    int possIndex = getExtededUcIndex(i, ucs, conns);
                    if (possIndex == -1)
                        d[i, j] = c[i, j];
                    else
                        d[i, j] = c[i, j] - c[possIndex, j];
                }

            int[,] e = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (d[i, j] == 0)
                    {
                        e[i, j] = 0;
                        continue;
                    }

                    List<int> tmp = getListOfIncluded(i, ucs, conns);
                    bool redundancy = false;
                    foreach (var index in tmp)
                    {
                        if (d[index, j] == 1)
                        {
                            redundancy = true;
                            break;
                        }
                    }

                    if (redundancy)
                        e[i, j] = 0;
                    else
                        e[i, j] = 1;

                }

            ucThird = 0;
            for (int i = 0; i < n; i++)
            {
                int tempSumm = 0;
                for (int j = 0; j < m; j++)
                    tempSumm += e[i, j];
                ucThird += Math.Pow(tempSumm, 1.4);
            }

            int summE = 0;
            int summC = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    summE += e[i, j];
                    summC += c[i, j];
                }

            ucFourth = 0.1 * nouc * nouc + ucThird + 0.1 * (summC - summE);
        }
        private int getExtededUcIndex(int i, List<Element> ucs, List<Arrow> conns)
        {
            string idFrom = ucs[i].Id;
            Arrow tmp = conns.FirstOrDefault(c => c.Type == ElementTypes.Extend && c.From == idFrom);
            if (tmp == null)
                return -1;
            else
                return ucs.FindIndex(uc => uc.Id == tmp.To);
        }
        private List<int> getListOfIncluded(int i, List<Element> ucs, List<Arrow> conns)
        {
            List<int> result = new List<int>();
            string idFrom = ucs[i].Id;

            foreach (var conn in conns.Where(c => c.Type == ElementTypes.Include))
            {
                if (conn.From == idFrom)
                    result.Add(ucs.FindIndex(a => a.Id == conn.To));
            }

            return result;
        }
        private void CalcCUcd(List<Element> ucs, List<Element> acts, List<Arrow> conns)
        {
            List<string> main = new List<string>();
            foreach (var conn in conns.Where(c => c.Type == ElementTypes.Association))
            {
                if (!main.Contains(conn.To))
                {
                    main.Add(conn.To);
                }
            }

            int n = ucs.Count;
            int m = acts.Count + main.Count;
            int[,] mtrig = new int[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mtrig[i, j] = 0;

            foreach (var conn in conns)
            {
                if (conn.Type == ElementTypes.Association)
                {
                    int actorInd = acts.FindIndex(a => a.Id == conn.From || a.Id == conn.To);
                    int ucInd = ucs.FindIndex(uc => uc.Id == conn.To || uc.Id == conn.From);
                    mtrig[ucInd, actorInd] = 3;
                }
                else if (conn.Type == ElementTypes.Include)
                {
                    int includingInd = main.FindIndex(id => id == conn.From) + acts.Count;
                    int includedInd = ucs.FindIndex(uc => uc.Id == conn.To);
                    mtrig[includedInd, includingInd] = 2;
                }
                else if (conn.Type == ElementTypes.Extend)
                {
                    int extendedInd = main.FindIndex(id => id == conn.To) + acts.Count;
                    int extendingInd = ucs.FindIndex(uc => uc.Id == conn.From);
                    mtrig[extendingInd, extendedInd] = 1;
                }
            }

            int[,] minit = GetTransparentMatrix(mtrig, n, m);

            CTE = 0;
            for (int i = 0; i < n; i++)
            {
                int tmp = 0;
                bool one = false;
                for (int j = 0; j < m; j++)
                {
                    if (mtrig[i, j] != 1)
                        tmp += mtrig[i, j];
                    else
                        one = true;
                }
                CTE += tmp + (one ? 1 : 0);
            }

            CIE = 0;
            for (int i = 0; i < m; i++)
            {
                int tmp = 0;
                bool one = false;
                for (int j = 0; j < n; j++)
                {
                    if (minit[i, j] != 1)
                        tmp += minit[i, j];
                    else
                        one = true;
                }
                CIE += tmp + (one ? 1 : 0);
            }

            cUcd = CTE + CIE;
        }
        private int[,] GetTransparentMatrix(int[,] mat, int n, int m)
        {
            int[,] t = new int[m, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    t[j, i] = mat[i, j];
            return t;
        }
    }
}
