//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Verification.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Verification.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 0 | СЕРЬЕЗНОСТЬ | ОШИБКА | ПОЯСНЕНИЯ | ЭТАП ПРОВЕРКИ |
        ///1 | Warning | Имя начинается с маленькой буквы | | Лексический |
        ///2 | Warning | Первое слово возможно не является именем существительным | | Лексический |
        ///3 | Warning | Нет знака вопроса | | Лексический |
        ///4 | Serious | Отсутствует подпись условия | | Лексический |
        ///5 | Serious | Неподписанная альтернатива | | Лексический |
        ///6 | Serious | Переход имеет подпись, не являясь условием или альтернативой | | Лексический |
        ///7 | Serious | Диаграмма не содержит элеме [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string ADMistakes {
            get {
                return ResourceManager.GetString("ADMistakes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 0 | СЕРЬЕЗНОСТЬ | ОШИБКА | ПОЯСНЕНИЯ | ЭТАП ПРОВЕРКИ |
        ///1 | 2 | использование недопустимых для диаграмм классов лексем | | Лексический | 
        ///2 | 1 | имя интерфейса или перечисления не начинается с большой буквы или содержит пробелы | | Лексический | 
        ///3 | 0 | текст ограничения не заключен в фигурные скобки {}  | | Лексический | 
        ///4 | 2 | у конструктора или деструктора указан возвращаемый тип  | | Синтаксический | 
        ///5 | 1 | у операции (кроме конструктора и деструктора) не указан возвращаемый тип | | Синтаксический  [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string CDMistakes {
            get {
                return ResourceManager.GetString("CDMistakes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {\pard\sa180\fs24 О программе\par}
        ///{\pard Данная программа предназначена для верификации UML диаграммы активностей (ДА), классов (ДК) и прецедентов (ДП). \par}
        ///{\pard Загружаемыми данными для программы являются XMI-файл, экспортированный из программы GenMyModel (https://app.genmymodel.com/api/login), а также возможна дополнительная загрузка PNG изображения, экспортированного также из GenMyModel. \par}
        ///{\pard \par}
        ///.
        /// </summary>
        internal static string HelpGeneral {
            get {
                return ResourceManager.GetString("HelpGeneral", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {\pard\sa180\fs24 Работа с программой\par}
        ///{\pard\b Добавление диаграммы \b0\par}
        ///{\pard Входными данными системы являются XMI-файл (обязательно) и PNG-файл (по желанию), экспортированные из GenMyModel (https://app.genmymodel.com/api/login). \par}
        ///{\pard Для добавления новой модели нажмите на кнопку &quot;Добавить&quot; и выберите необходимые файлы. Одновременно возможно добавлять любое количество моделей. \par}
        ///{\pard Для установления связи между XMI файлом и PNG изображением необходимо, чтобы имена файлов были один [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string HelpProgram {
            get {
                return ResourceManager.GetString("HelpProgram", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 0 | НАИМЕНОВАНИЕ | РАСШИФРОВКА АББРЕВИАТУРЫ | ОБОЗНАЧЕНИЕ | ЕДИНИЦА ИЗМЕРЕНИЯ |
        ///1 | NOUC | Number of use cases | Количество прецедентов | Штук |
        ///2 | NOA | Number of actors | Количество акторов | Штук |
        ///3 | NOUCA | | Отношение количества акторов к количеству прецедентов | Безразмерная |
        ///4 | UC2 | | Количество связей типа ассоциации | Штук |
        ///5 | UC3 | | Величина пропорциональная количеству значимых связей типа ассоциации | Безразмерная |
        ///6 | UC4 | | Величина пропорциональная сложности диаграммы учитывающая пр [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string UCDMetrics {
            get {
                return ResourceManager.GetString("UCDMetrics", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 0 | Серьезность| Ошибка | Пояснения | Этап проверки |
        ///1 | 2 | Отсутствует граница системы  | | Лексическая |
        ///2 | 1 | Некорректное имя актора: должно быть представлено именем существительным, начинаться с большой буквы  | | Лексическая |
        ///3 | 1 | Некорректное название прецедента: должно быть представлено в виде действия (глаголом или именем существительным отглагольной формы), начинаться с большой буквы  | | Лексическая |
        ///4 | 1 | Отсутствует название системы (имя субъекта)  | | Лексическая |
        ///5 | 1 | Элемент н [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string UCDMistakes {
            get {
                return ResourceManager.GetString("UCDMistakes", resourceCulture);
            }
        }
    }
}
