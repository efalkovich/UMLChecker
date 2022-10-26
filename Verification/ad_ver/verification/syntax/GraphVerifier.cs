﻿using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verification.ad_ver.verification;
using Verification.rating_system;

namespace ActivityDiagramVer.verification.syntax {
    internal class GraphVerifier {
        private const char NO_TOKEN = '0';
        private const char TOKEN = '1';
        private const char NEW_TOKEN = '2';
        private const int NO_COLOR = 0;
        private int randomColor = 0;

        /**
         * Создает маску, в которой все эл-ты неактивны
         * @param length необходимая длина
         * @return строка, заполненная нулями
         */
        private List<Token> createEmptyMask(int length) {
            List<Token> mask = new List<Token>();
            for (int i = 0; i < length; i++)
                mask.Add(new Token());
            return mask;
        }

        /**
         * Генерация рандомного цвета для токена, проходящего через разветвитель
         */
        private int generateRandomColor() {
            return ++randomColor;
        }

        public void check(ADNodesList adList) {
            Queue<List<Token>> leaves = new Queue<List<Token>>();       // необработанные маски
            HashSet<string> masksInUsed = new HashSet<string>(adList.getPetriElementsCount());   // использованные маски

            // ищем начальное состояние
            ADNodesList.ADNode initNode = adList.findInitial();
            var finalNode = adList.findFinal();
            var indexesOfFinalNode = finalNode.Select(x => ((DiagramElement)x.getValue()).petriId).ToList();

            // создаем маску и добавляем ее в необработанные
            List<Token> maskTmp = createEmptyMask(adList.getPetriElementsCount());
            setNewPaleToken(maskTmp, ((DiagramElement)initNode.getValue()).petriId);

            leaves.Enqueue(maskTmp);
            masksInUsed.Add(maskToString(maskTmp));   // добавляем в множество использованных

            bool canReachFinal = false;

            List<List<Token>> stepResultMasks = new List<List<Token>>();       // содержит маски, кот могут получиться на каждом шаге
            string originMaskString = "";

            // главный цикл прохода по всем элементам, участвующих в проверке
            while (leaves.Count > 0) {
                List<Token> originMask = leaves.Dequeue();  // берем первую маску
                originMaskString = maskToString(originMask);
                stepResultMasks.Clear();
                List<Token> stepMask = copyMask(originMask); // маска, которую будем изменять по мере деактивации токенов
                stepResultMasks.Add(copyMask(stepMask));   // Если список масок не изменится, то будет тупик, тк текущая маска уже добавлена в использованные

                int i = 0;
                // Console.WriteLine("Or: " + maskToString(originMask));

                // проходим по всем элементам маски, находим активированные
                // и проверяем, можно ли активировать следующий элемент
                for (int stepProhod = 0; stepProhod < 2; stepProhod++) {     // сначала проходим по условным, затем по остальным эл-м
                    i = 0;
                    while (i < stepMask.Count) {
                        if (stepProhod == 0 && adList.getNodeByPetriIndex(i).getValue().getType() != ElementType.DECISION) {
                            i++;
                            continue;   // интересуют только эл-ты, содержащие токены на данном шаге
                        }
                        if (stepProhod == 0 && adList.getNodeByPetriIndex(i).getValue().getType() == ElementType.DECISION && stepMask[i].type != TOKEN) {
                            i++;
                            continue;   // интересуют только эл-ты, содержащие токены на данном шаге
                        }
                        if (stepProhod == 1 && stepMask[i].type != TOKEN) {
                            i++;
                            continue;   // интересуют только эл-ты, содержащие токены на данном шаге
                        }
                        // нашли активный элемент
                        ADNodesList.ADNode curNode = adList.getNodeByPetriIndex(i);     //индекс в списке совпадает с петри ид эл-та
                        int curNodeIndex = ((DiagramElement)curNode.getValue()).petriId;
                        List<int> colorsCurToken = new List<int>(stepMask[curNodeIndex].colors);

                        // особо обрабатываем ситуации, когда элемент имеет несколько выходных переходов
                        if (curNode.nextSize() > 1) {
                            // если эл-т разветвитель, то последующее состояние единственно, однако надо установить несколько токенов за раз
                            if (curNode.getValue().getType() == ElementType.FORK) {
                                colorsCurToken.Add(generateRandomColor());

                                // активируем все следующие элементы
                                for (int j = 0; j < curNode.nextSize(); j++) {
                                    // сразу после форка не может быть join'a, поэтому все эл-ты будут активированы
                                    int indexOfNewToken = ((DiagramElement)curNode.getNext(j).getValue()).petriId;
                                    // проверка, что эл-т не был раннее активирован
                                    if (wasAlreadyActive(indexOfNewToken, stepResultMasks, curNode.getNext(j))) return;
                                    // изменяем существующие результирующие маски
                                    updateStepMasks(((DiagramElement)curNode.getValue()).petriId, indexOfNewToken, stepResultMasks, colorsCurToken);
                                }
                                setNewEmptyToken(stepMask, curNodeIndex);
                            }
                            // если это условный оператор, то он порождает несколько возможных последующих состояний
                            else {
                                List<List<Token>> decisionMasks = new List<List<Token>>();   // содержит все возможные маски вариантов перемещения токена
                                bool tokenWasRemoved = false;
                                // активируем следующие элементы по одному
                                for (int j = 0; j < curNode.nextSize(); j++) {
                                    int indexOfNewToken = ((DiagramElement)curNode.getNext(j).getValue()).petriId;      // индекс активируемого эл-та
                                                                                                                        // проверка, что эл-т не был раннее активирован
                                    if (wasAlreadyActive(indexOfNewToken, stepResultMasks, curNode.getNext(j))) return;
                                    // Join обрабатывается с помощью reverse проверки
                                    if ((curNode.getNext(j).getValue()).getType() == ElementType.JOIN) {
                                        List<List<Token>> temp = copyMasks(stepResultMasks);
                                        List<Token> result = activateJoin(curNode.getNext(j), stepMask, temp, colorsCurToken);
                                        if (result != null) {
                                            stepMask = result;
                                            decisionMasks.AddRange(temp);
                                            tokenWasRemoved = true;
                                        }
                                    } else {
                                        // в каждой существующей результирующей маске меняем токен и сохраняем в промежуточный список
                                        // в итоге получится новый список, содержащий несколько вариантов результирующих масок,
                                        // в каждом из которых создан новый токен в зависимости от активированного следующего эл-та
                                        foreach (List<Token> resultMask in stepResultMasks) {
                                            List<Token> temp = copyMask(resultMask);
                                            setToken(temp, indexOfNewToken, NEW_TOKEN, colorsCurToken);     // добавляем новый токен

                                            setNewEmptyToken(temp, curNodeIndex);           // удаляем токен текущего эл-та
                                            decisionMasks.Add(temp);
                                            tokenWasRemoved = true;
                                        }
                                    }

                                }
                                // изменяем маску шага и список результирующих масок, если токен был удален
                                if (tokenWasRemoved) {
                                    setNewEmptyToken(stepMask, ((DiagramElement)curNode.getValue()).petriId);
                                    // меняем результирующую маску
                                    stepResultMasks = decisionMasks;
                                }
                            }
                        } else {       // если выходной переход один
                            int indexOfNewToken = ((DiagramElement)curNode.getNext(0).getValue()).petriId;
                            // проверка, что эл-т не был раннее активирован
                            if (wasAlreadyActive(indexOfNewToken, stepResultMasks, curNode.getNext(0))) return;
                            // если следующий Join
                            if ((curNode.getNext(0).getValue()).getType() == ElementType.JOIN) {
                                List<Token> result = activateJoin(curNode.getNext(0), stepMask, stepResultMasks, colorsCurToken);
                                if (result != null)
                                    stepMask = result;
                            } else {
                                updateStepMasks(((DiagramElement)curNode.getValue()).petriId, indexOfNewToken, stepResultMasks, colorsCurToken);
                                setNewEmptyToken(stepMask, curNodeIndex);
                            }
                        }
                        i++;
                    }
                }
                // в результирующих масках заменяем NEW_Token на TOKEN
                foreach (List<Token> stepResultMask in stepResultMasks) {
                    stepResultMask.ForEach(x => x.type = x.type == NEW_TOKEN ? TOKEN : x.type);
                    //Console.WriteLine("Re: " + maskToString(stepResultMask));
                    if (originMaskString == maskToString(stepResultMask)) {
                        ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.DEAD_ROAD], MistakeAdapter.toString(MISTAKES.DEAD_ROAD), ALL_MISTAKES.DEAD_ROAD);
                        //Console.WriteLine("Again?: "+originMaskString);
                        return;
                    }
                }
                    // проверяем, что новой маски нет во множестве обработанных и добавляем в необработанные в таком случае
                foreach (List<Token> resultMask in stepResultMasks) {
                    if (!findInMasksInUsed(masksInUsed, resultMask)) {
                        int indexOfFinalNode = -1;

                        // проверяем, не достигли ли конечной маркировки (существует путь позволяющий добраться хоть до какого-н finalNode)
                        foreach (var ind in indexesOfFinalNode) {
                            if (resultMask[ind].type == TOKEN) {
                                indexOfFinalNode = ind;
                                break;
                            }
                        }
                        if (indexOfFinalNode != -1) {
                            canReachFinal = true;
                            if (resultMask[indexOfFinalNode].peekLastColor() != NO_COLOR) {
                                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.FINAL_COLOR_TOKEN], MistakeAdapter.toString(MISTAKES.FINAL_COLOR_TOKEN), ALL_MISTAKES.FINAL_COLOR_TOKEN);
                                return;
                            }
                            //Проверка, что не осталось токенов
                            int tokenCount = 0;
                            for (int j = 0; j < resultMask.Count && tokenCount <= 1; j++) {
                                if (resultMask[j].type == TOKEN) {
                                    tokenCount++;
                                }
                            }
                            if (tokenCount > 1) {
                                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.MANY_TOKENS_IN_END], MistakeAdapter.toString(MISTAKES.MANY_TOKENS_IN_END), ALL_MISTAKES.MANY_TOKENS_IN_END);
                                return;
                            }
                        } else leaves.Enqueue(copyMask(resultMask));
                        masksInUsed.Add(maskToString(resultMask));      // добавляем полученную маску в множество обработанных
                    } 
                }
            }

            // проверяем, что конечное состояние было достигнуто
            if (!canReachFinal)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.COULD_NOT_REACH_FINAL], MistakeAdapter.toString(MISTAKES.COULD_NOT_REACH_FINAL), ALL_MISTAKES.COULD_NOT_REACH_FINAL);
        }

        private void setNewPaleToken(List<Token> mask, int index) {
            mask[index] = new Token(GraphVerifier.TOKEN, new List<int>() { NO_COLOR });
        }

        private void setNewEmptyToken(List<Token> mask, int index) {
            mask[index] = new Token();
        }


        private void setToken(List<Token> mask, int index, char token, List<int> colors) {
            mask[index] = new Token(token, colors);
        }

        private List<Token> copyMask(List<Token> old) {
            List<Token> newlst = new List<Token>();
            old.ForEach(x => newlst.Add(new Token(x)));
            return newlst;
        }
        private List<List<Token>> copyMasks(List<List<Token>> old) {
            List<List<Token>> newlst = new List<List<Token>>();
            foreach (List<Token> masks in old)
                newlst.Add(copyMask(masks));
            return newlst;
        }


        private string maskToString(List<Token> mask) {
            string maskStr = "";
            mask.ForEach(x => maskStr += x.type);
            return maskStr;
        }
        private bool findInMasksInUsed(HashSet<string> inUsed, List<Token> mask) {
            return inUsed.Contains(maskToString(mask));
        }

        /**
         * Проверка был ли данный элемент раннее активирован. Если да, то проверка завершается
         */
        private bool wasAlreadyActive(int tokenIndex, List<List<Token>> stepResultMasks, ADNodesList.ADNode curNode) {
            foreach (List<Token> stepResultMask in stepResultMasks) {
                if (stepResultMask[tokenIndex].type != NO_TOKEN) {
                    if (curNode.getValue().getType() != ElementType.FINAL_NODE)
                        ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.TWO_TOKENS], MistakeAdapter.toString(MISTAKES.TWO_TOKENS), ALL_MISTAKES.TWO_TOKENS);
                    else
                        ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.MANY_TOKENS_IN_END], MistakeAdapter.toString(MISTAKES.MANY_TOKENS_IN_END), ALL_MISTAKES.MANY_TOKENS_IN_END);
                    return true;
                }
            }
            return false;
        }

        /**
         * Обновляет маски списка, создавая новые токены по указанному индексу в списке масок, копирует список цветов,
         * удаляет токен по индексу в каждой маске из списка
         */
        private void updateStepMasks(int removedIndex, int newIndex, List<List<Token>> stepMasks, List<int> colors) {
            stepMasks.ForEach(x => {
                setToken(x, newIndex, NEW_TOKEN, colors);
                setNewEmptyToken(x, removedIndex);
            }
        );
        }

        private List<Token> activateJoin(ADNodesList.ADNode joinNode, List<Token> mask, List<List<Token>> resultMasks, List<int> curColors) {
            List<Token> newMask = copyMask(mask);
            List<Token> maskCur = copyMask(mask);
            int color = 0;
            int curIndex = ((DiagramElement)joinNode.getValue()).petriId;
            List<int> idsPrev = new List<int>();         // список ид элементов перед join
            List<int> newColors = new List<int>(curColors);
            newColors.RemoveAt(newColors.Count - 1);           // подготовили список цветов для join


            // все предшествующие элементы должны содержать токен одного цвета
            for (int i = 0; i < joinNode.prevSize(); i++) {
                int idPrev = ((DiagramElement)joinNode.getPrev(i).getValue()).petriId;
                idsPrev.Add(idPrev);
                if (newMask[idPrev].type == NO_TOKEN)
                    return null;
                if (i == 0) color = newMask[idPrev].peekLastColor();
                if (newMask[idPrev].peekLastColor() != color)
                    return null;
            }
            // если все предыдущие элементы были активны, активируем текущий
            // для каждой результирующей маски шага удаляем токены и добавляем токен в join
            foreach (int id in idsPrev) {
                updateStepMasks(id, curIndex, resultMasks, newColors);
                setNewEmptyToken(maskCur, id);      // удаляем токены из маски шага
            }

            return maskCur;
        }

        private class Token {
            public char type;
            public List<int> colors;
            public Token() {
                type = NO_TOKEN;
                colors = new List<int>();
            }
            public Token(char type, List<int> colors) {
                this.type = type;
                this.colors = new List<int>(colors);
            }
            public Token(Token old) {
                type = old.type;
                colors = new List<int>(old.colors);
            }
            public int peekLastColor() {
                return colors[colors.Count - 1];
            }
        }
    }
}
