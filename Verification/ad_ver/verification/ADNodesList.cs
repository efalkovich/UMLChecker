﻿using ActivityDiagramVer.entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActivityDiagramVer
{
    public class ADNodesList
    {
        private readonly List<ADNode> nodes;
        private int diagramElementId = 0;       // Петри ид, присваиваемый элементу

        public ADNodesList()
        {
            nodes = new List<ADNode>();
        }
        /**
         * Возвращает колво элементов, используемых для проверки сетью Петри
         * @return
         */
        public int getPetriElementsCount()
        {
            return diagramElementId;
        }

        /**
         * получить все активности из массива
         * @return
         */
        public List<ActivityNode> getAllActivities()
        {
            List<int> temp = new List<int>();
            return nodes.Where(x => x.getValue().getType() == ElementType.ACTIVITY).ToList().Select(x => (ActivityNode)x.getValue()).ToList();
        }

        /**
         * Найти начальное состояние
         * @return ссылка на узел начального состояние
         */
        public ADNode findInitial()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].getValue().getType() == ElementType.INITIAL_NODE)
                {
                    return nodes[i];
                }
            }
            return null;
        }
        /**
         * Найти конеченое состояние
         * @return ссылка на узел конеченого состояния
         */
        public List<ADNode> findFinal()
        {
            var finalNodes = new List<ADNode>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].getValue().getType() == ElementType.FINAL_NODE)
                {
                    finalNodes.Add(nodes[i]);
                }
            }
            return finalNodes;
        }

        /**
         * Установить связи между элементами ДА
         */
        public void connect()
        {
            foreach (var node in nodes)
            {
                // связываем все элементы, кроме переходов
                if (node.getValue() is DiagramElement)
                {
                    findNext((DiagramElement)node.getValue(), node);
                }
            }
        }


        /**
         * Найти элементы для связи
         * @param cur текущий элемент, кот надо связать
         * @param curNode
         */
        private void findNext(DiagramElement cur, ADNode curNode)
        {
            // для всех выходный переходов находим таргеты и добавляем ссылки в текущий элемент на таргеты
            for (int i = 0; i < cur.outSize(); i++)
            {
                ControlFlow flow = (ControlFlow)get(cur.getOutId(i));
                ADNode target = getNode(flow.getTarget());
                curNode.next.Add(target);       // прямая связь
                target.prev.Add(curNode);        // обратная связь
            }
        }

        /**
         * Печать связей между элементами
         */
        public void print()
        {
            foreach (ADNode node in nodes)
            {
                if (node.getValue() is DiagramElement)
                {
                    Console.WriteLine("Cur: [" + ((DiagramElement)node.getValue()).petriId + "] " + ((DiagramElement)node.getValue()).getDescription() + " " + node.getValue().getType() + " | ");
                    for (int i = 0; i < node.next.Count; i++)
                    {
                        Console.WriteLine(node.getNext(i).getValue().getType() + " ");
                    }
                    Console.WriteLine(" || ");
                    for (int i = 0; i < node.prev.Count; i++)
                    {
                        Console.WriteLine(node.prev[i].getValue().getType() + " ");
                    }
                    Console.WriteLine("");
                }
            }
        }

        public int size()
        {
            return nodes.Count;
        }
        public void addLast(BaseNode node)
        {
            if (node is DiagramElement)
            {
                ((DiagramElement)node).petriId = diagramElementId;
                diagramElementId++;
            }
            nodes.Add(new ADNode(node));
        }
        public BaseNode get(int index)
        {
            return nodes[index].getValue();
        }

        /**
         * @return значение узла или null если такой не найден
         */
        public BaseNode get(string id)
        {

            ADNode node = nodes.Where(x => x.getValue().getId().Equals(id)).FirstOrDefault();
            if (node == default) return null;
            return node.getValue() != null ? node.getValue() : null;
        }

        public ADNode getNode(string id)
        {
            var node = nodes.Where(x => x.getValue().getId().Equals(id)).FirstOrDefault();
            return node == default ? null : node;

        }
        public ADNode getNode(int index)
        {
            return nodes[index];
        }

        public ADNode getNodeByPetriIndex(int id)
        {
            ADNode node = nodes.Where(x =>
            {
                if (x.getValue() is DiagramElement)
                    return ((DiagramElement)x.getValue()).petriId == id;
                return false;
            }).FirstOrDefault();
            return node == default ? null : node;
        }
        //endregion


        public class ADNode
        {
            private BaseNode value;
            public List<ADNode> next = new List<ADNode>();
            public List<ADNode> prev = new List<ADNode>();


            public ADNode(BaseNode value)
            {
                this.value = value;
            }

            public ADNode(ADNode old)
            {
                value = old.value;
                next = old.next;
                prev = old.prev;
            }

            //region Getter-Setter
            public BaseNode getValue()
            {
                return value;
            }

            public void setValue(BaseNode value)
            {
                this.value = value;
            }

            public int prevSize() { return prev.Count; }
            public int nextSize() { return next.Count; }

            public ADNode getNext(int index)
            {
                return next[index];
            }
            public ADNode getPrev(int index) { return prev[index]; }

            public List<int> getNextPetriIds()
            {
                return next.Select(x => ((DiagramElement)x.getValue()).petriId).ToList();
            }
            public List<int> getPrevPetriIds()
            {
                return prev.Select(x => ((DiagramElement)x.getValue()).petriId).ToList();
            }
            //endregion
        }
    }
}
