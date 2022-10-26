﻿using System;
using System.Collections.Generic;
using Verification.rating_system;

namespace Verification.settings {
    [Serializable]
    public class MistakeModel {
        public static IDictionary<ALL_MISTAKES, Tuple<double, String>> mistakes = new Dictionary<ALL_MISTAKES, Tuple<double, String>>() {
                { ALL_MISTAKES.MORE_THAN_ONE_INIT, new Tuple<double, string>(1.0,"AD. Более одного начального состояния") },
                { ALL_MISTAKES.NO_FINAL, new Tuple<double, string>(1.0,"AD. Более одного конечного состояния") },
                { ALL_MISTAKES.NO_INITIAL, new Tuple<double, string>(1.0,"AD. Отсутствует начальное состояние") },
                { ALL_MISTAKES.NO_ACTIVITIES, new Tuple<double, string>(1.0,"AD. Отсутствуют активности") },
                { ALL_MISTAKES.MORE_THAN_ONE_OUT, new Tuple<double, string>(1.0,"AD. Элемент имеет более одного ") },
                { ALL_MISTAKES.DO_NOT_HAVE_ALT, new Tuple<double, string>(1.0,"AD. Условный переход не имеет альтернатив") },
                { ALL_MISTAKES.ONLY_ONE_ALT, new Tuple<double, string>(1.0,"AD. Условный переход имеет всего одну альтернативу") },
                { ALL_MISTAKES.NO_OUT, new Tuple<double, string>(1.0,"AD. У элемента (кроме конечного состояния) отсутствует выходящий переход") },
                { ALL_MISTAKES.NO_IN, new Tuple<double, string>(1.0,"AD. У элемента (кроме начального состояния) отсутствует входящий переход	") },
                { ALL_MISTAKES.NO_PARTION, new Tuple<double, string>(1.0,"AD. Элемент не принадлежит никакому участнику") },
                { ALL_MISTAKES.REPEATED_NAME, new Tuple<double, string>(1.0,"AD. Имя участника или альтернатива не уникальны") },
                { ALL_MISTAKES.SAME_TARGET, new Tuple<double, string>(1.0,"AD. Альтернативы ведут в один и тот же элемент") },
                { ALL_MISTAKES.OUT_NOT_IN_ACT, new Tuple<double, string>(1.0,"AD. Переход ведет не в активность, разветвитель или в условный переход ") },
                { ALL_MISTAKES.NEXT_DECISION, new Tuple<double, string>(1.0,"AD. Альтернатива ведет в условный переход") },
                { ALL_MISTAKES.HAS_1_IN, new Tuple<double, string>(1.0,"AD. Разветвитель или слияние имеют всего один входной переход") },

                { ALL_MISTAKES.FORBIDDEN_ELEMENT, new Tuple<double, string>(1.0,"AD. В диаграмме используется недопустимый элемент") },
                { ALL_MISTAKES.NO_SWIMLANE, new Tuple<double, string>(0.0,"AD. Отсутствуют акторы или используется не верный элемент") },
                { ALL_MISTAKES.SMALL_LETTER, new Tuple<double, string>(1.0,"AD. Имя активности или участника начинается с маленькой буквы") },
                { ALL_MISTAKES.NO_NAME, new Tuple<double, string>(1.0,"AD. Актор или активность не имееют подписи") },
                { ALL_MISTAKES.NOT_NOUN, new Tuple<double, string>(1.0,"AD. Первое слово в имени активности, возможно, не является именем существительным") },
                { ALL_MISTAKES.END_WITH_QUEST, new Tuple<double, string>(1.0,"AD. В условии отсутствует знак вопроса") },
                { ALL_MISTAKES.HAVE_NOT_QUEST, new Tuple<double, string>(1.0,"AD. Отсутствует подпись в условии") },
                { ALL_MISTAKES.REPEATED_ALT, new Tuple<double, string>(1.0,"AD. Повторяется альтернатива у одного условного перехода") },
                { ALL_MISTAKES.HAVE_EMPTY_ALT, new Tuple<double, string>(1.0,"AD. Условный переход имеет не подписанную альтернативу") },
                { ALL_MISTAKES.HAVE_MARK, new Tuple<double, string>(1.0,"AD. Переход имеет подпись, не являясь условием или альтернативой") },
                { ALL_MISTAKES.STRANGE_SYMBOL, new Tuple<double, string>(1.0,"AD. В имени участника используется специальный символ") },
                { ALL_MISTAKES.EMPTY_SWIMLANE, new Tuple<double, string>(0.0,"AD. Дорожка участника не содержит элементов") },

                { ALL_MISTAKES.TWO_TOKENS, new Tuple<double, string>(1.0,"AD. Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем") },
                { ALL_MISTAKES.DEAD_ROAD, new Tuple<double, string>(1.0,"AD. Тупик. Невозможно активировать синхронизатор") },
                { ALL_MISTAKES.MANY_TOKENS_IN_END, new Tuple<double, string>(1.0,"AD. Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем") },
                { ALL_MISTAKES.COULD_NOT_REACH_FINAL, new Tuple<double, string>(1.0,"AD. Недостижимое конечное состояние. Возможно имеется синхронизатор, который невозможно активировать") },
                { ALL_MISTAKES.FINAL_COLOR_TOKEN, new Tuple<double, string>(1.0,"AD. Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем") },
       
                {ALL_MISTAKES.CD_NO_LINK, new Tuple<double, string>(1.0, "CD. Класс/интерфейс/перечисление не имеет связей")},
                {ALL_MISTAKES.CD_NO_CHILDREN, new Tuple<double, string>(1.0, "CD. Класс/интерфейс с элементами \"protected\" не имеет потомков")},
                {ALL_MISTAKES.CD_INCORRECT_NAME, new Tuple<double, string>(1.0, "CD. Неверное имя элемента")},
                {ALL_MISTAKES.CD_ATTRIB_WITH_ACTION, new Tuple<double, string>(1.0, "CD. Имя атрибута обозначает действие")},
                {ALL_MISTAKES.CD_EMPTY_CLASS, new Tuple<double, string>(1.0, "CD. Класс/интерфейс не имеет ни атрибутов, ни операций")},
                {ALL_MISTAKES.CD_MUST_BE_ATTRIB, new Tuple<double, string>(1.0, "CD. Класс/интерфейс не имеет раздела с атрибутами")},
                {ALL_MISTAKES.CD_HAS_OUTPUT_TYPE, new Tuple<double, string>(1.0, "CD. Конструктор/деструктор имеет возвращаемый тип")},
                {ALL_MISTAKES.CD_HAS_NOT_OUTPUT_TYPE, new Tuple<double, string>(1.0, "CD. Не указан возвращаемый тип операции")},
                {ALL_MISTAKES.CD_NO_AVAILABLE_LINKS, new Tuple<double, string>(1.0, "CD. Перечисление не имеет допустимых связей")},
                {ALL_MISTAKES.CD_RESTRICTION_HAS_NO_BRACKETS, new Tuple<double, string>(0.0, "CD. Ограничение не записано в скобках {}")},
                {ALL_MISTAKES.CD_NO_PACKAGE, new Tuple<double, string>(1.0, "CD. Отсутствует элемент \"package\"")},
                {ALL_MISTAKES.CD_NO_CONTAINER, new Tuple<double, string>(1.0, "CD. Класс/интерфейс не имеет контейнера для объектов другого класса/интерфейса")},
                {ALL_MISTAKES.CD_LESS_ZERO, new Tuple<double, string>(1.0, "CD. Значение кратности меньше нуля")},
                {ALL_MISTAKES.CD_WRONG_RANGE, new Tuple<double, string>(1.0, "CD. Диапазон кратности записан неверно")},
                {ALL_MISTAKES.CD_DUPLICATE_NAME, new Tuple<double, string>(1.0, "CD. Элемент с таким именем уже существует")},
                {ALL_MISTAKES.CD_IMPOSSIBLE_ELEMENT, new Tuple<double, string>(1.0, "CD. Недопустимый элемент")},
                {ALL_MISTAKES.CD_IMPOSSIBLE_TYPE, new Tuple<double, string>(1.0, "CD. Недопустимый тип")},
                {ALL_MISTAKES.CD_AGGREG_COMPOS_CYCLE, new Tuple<double, string>(1.0, "CD. Цикл в агрегациях/композициях")},
                {ALL_MISTAKES.CD_SETTER_WITHOUT_PARAMS, new Tuple<double, string>(1.0, "CD. Операция set не имеет ни одного параметра")},
                {ALL_MISTAKES.CD_GETTER_WITH_PARAMS, new Tuple<double, string>(1.0, "CD. Операция get имеет параметры")},
                {ALL_MISTAKES.CD_SUPERFLUOUS_CONNECTION, new Tuple<double, string>(1.0, "CD. Лишняя ассоциация между элементами")},

                {ALL_MISTAKES.UCREPEAT, new Tuple<double, string>(1.0,"UCD. Имя актора не уникально")},
                {ALL_MISTAKES.UCNOUN, new Tuple<double, string>(1.0,"UCD. Имя актора представлено не в виде существительного с заглавной буквы")},
                {ALL_MISTAKES.UCNOLINK, new Tuple<double, string>(1.0,"UCD. Актор не имеет ни одной связи типа ассоцияция с прецедентами")},
                {ALL_MISTAKES.UCNOTEXT, new Tuple<double, string>(1.0,"UCD. Отсутствует текст в условии расширения")},
                {ALL_MISTAKES.UCNOBORDER, new Tuple<double, string>(1.0,"UCD. Отсутствует граница системы")},
                {ALL_MISTAKES.UCNONAME, new Tuple<double, string>(1.0,"UCD. Отсутствует назние системы")},
                {ALL_MISTAKES.UCNOTEXTINPRECEDENT, new Tuple<double, string>(1.0,"UCD. Отсутствует текст в точке расширения прецедента")},
                {ALL_MISTAKES.UCREPETEDNAME, new Tuple<double, string>(1.0,"UCD. Имя прецедента не уникально")},
                {ALL_MISTAKES.UCBIGLETTER, new Tuple<double, string>(1.0,"UCD. Имя прецедента не представлено в виде действия, начинаясь с заглавной буквы")},
                {ALL_MISTAKES.UCASSOSIATION, new Tuple<double, string>(1.0,"UCD. Прецедент должен иметь связь с актором в виде ассоциации, либо иметь отношения расширения, дополнения или включения с другими прецедентами")},
                {ALL_MISTAKES.UCNOPRECEDENTDOT, new Tuple<double, string>(1.0,"UCD. Отсутствие точки расширения у прецедента с отношением расширения")},
                {ALL_MISTAKES.UCONLYONEPRECEDENT, new Tuple<double, string>(1.0,"UCD. Прецедент включает всего один прецедент")},
                {ALL_MISTAKES.UCINCLUDE, new Tuple<double, string>(1.0,"UCD. Злоупотребление отношением включения")},
                {ALL_MISTAKES.UCBEHINDBORDER, new Tuple<double, string>(1.0,"UCD. Элемент находится за пределами системы")},
                {ALL_MISTAKES.UCNOAVALABELELEMENT, new Tuple<double, string>(1.0,"UCD. Недопустимый элемент")},
                {ALL_MISTAKES.UCNOCOORDINATE, new Tuple<double, string>(0,"UCD. Координаты отсутствуют")},
                {ALL_MISTAKES.UCNOAVALABELELEMENTINSYSTEM, new Tuple<double, string>(1.0,"UCD. Недопустимый элемент внутри системы")}
            };

        public IDictionary<ALL_MISTAKES, Tuple<double, string>> Mistakes { get => mistakes; set => mistakes = value; }

        public void setValue(string key, string value) {
            var text = mistakes[(ALL_MISTAKES)Enum.Parse(typeof(ALL_MISTAKES), key)].Item2;
            mistakes[(ALL_MISTAKES)Enum.Parse(typeof(ALL_MISTAKES), key)] = new Tuple<double, string>(Double.Parse(value), text);
        }
    }
}
