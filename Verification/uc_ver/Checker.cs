using System.Collections.Generic;
using System.Linq;
using Verification.package_ver;
using Verification.rating_system;

namespace Verification.uc_ver
{
    internal class Checker
    {
        private readonly Dictionary<string, Element> elements;
        private readonly List<Mistake> mistakes;
        public Checker(Dictionary<string, Element> elements, List<Mistake> mistakes)
        {
            this.elements = elements;
            this.mistakes = mistakes;
        }
        public void Check()
        {
            CheckActors();
            CheckComments();
            СheckPackages();
            CheckPrecedents();
        }

        #region Checks
        private void CheckActors()
        {
            Dictionary<string, Element> actors = new Dictionary<string, Element>();
            foreach (var el in elements.Values)
            {
                if (el.Type == ElementTypes.Actor)
                    actors.Add(el.Id, el);
            }
            foreach (var actorName in actors.GroupBy(a => a.Value.Name))
            {
                if (actorName.Count() > 1)
                {
                    Dictionary<string, Element> errorElements = new Dictionary<string, Element>();
                    foreach (var el in elements.Values)
                    {
                        if (el.Type == ElementTypes.Actor && el.Name == actorName.Key)
                            errorElements.Add(el.Id, el);
                    }

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Имя актора не уникально: {actorName.Key}",
                            element.Value, ALL_MISTAKES.UCREPEAT));
                    }
                }
                var firstWord = actorName.Key.Split(' ')[0];
                if (string.IsNullOrEmpty(actorName.Key.Trim()) || !char.IsUpper(actorName.Key[0]) || !IsNoun(firstWord))
                {
                    Dictionary<string, Element> errorElements = new Dictionary<string, Element>();
                    foreach (var el in elements.Values)
                    {
                        if (el.Type == ElementTypes.Actor && el.Name == actorName.Key)
                            errorElements.Add(el.Id, el);
                    }

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.ERROR,
                           $"Имя актора не представлено в виде существительного с заглавной буквы: {actorName.Key}",
                           element.Value, ALL_MISTAKES.UCNOUN));
                    }
                }
            }

            foreach (var actor in actors)
            {
                if (!HaveConnection(actor.Key, ElementTypes.Association))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.ERROR,
                           $"Актор не имеет ни одной связи типа ассоцияция с прецедентами: {actor.Value.Name}",
                           actor.Value, ALL_MISTAKES.UCNOLINK));
                }
            }
        }

        private void CheckComments()
        {
            Dictionary<string, Element> comments = new Dictionary<string, Element>();
            foreach (var el in elements.Values)
            {
                if (el.Type == ElementTypes.Comment)
                    comments.Add(el.Id, el);
            }

            foreach (var comment in comments)
                if (string.IsNullOrEmpty(comment.Value.Name))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                             $"Отсутствует текст в условии расширения",
                            comment.Value, ALL_MISTAKES.UCNOTEXT));
                }
        }

        private void СheckPackages()
        {
            Dictionary<string, Element> packages = new Dictionary<string, Element>();
            foreach (var el in elements.Values)
            {
                if (el.Type == ElementTypes.Package)
                    packages.Add(el.Id, el);
            }

            if (packages.Count() == 0)
            {
                mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствует граница системы", ALL_MISTAKES.UCNOBORDER));
            }


            foreach (var package in packages)
                if (string.IsNullOrEmpty(package.Value.Name.Trim()))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствует назние системы",
                            package.Value, ALL_MISTAKES.UCNONAME));
                }
        }

        private void CheckPrecedents()
        {
            Dictionary<string, Element> extensionPoints = new Dictionary<string, Element>();
            foreach (var el in elements.Values)
            {
                if (el.Type == ElementTypes.ExtensionPoint)
                    extensionPoints.Add(el.Id, el);
            }
            foreach (var point in extensionPoints)
                if (string.IsNullOrEmpty(point.Value.Name.Trim()))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствует текст в точке расширения прецедента",
                            point.Value, ALL_MISTAKES.UCNOTEXTINPRECEDENT));
                }

            Dictionary<string, Element> precedents = new Dictionary<string, Element>();
            foreach (var el in elements.Values)
            {
                if (el.Type == ElementTypes.Precedent)
                    precedents.Add(el.Id, el);
            }
            foreach (var precedentName in precedents.GroupBy(p => p.Value.Name))
            {
                if (precedentName.Count() > 1)
                {
                    Dictionary<string, Element> errorElements = new Dictionary<string, Element>();
                    foreach (var el in elements.Values)
                    {
                        if (el.Type == ElementTypes.Precedent && el.Name == precedentName.Key)
                            errorElements.Add(el.Id, el);
                    }

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Имя прецедента не уникально: {precedentName.Key}",
                            element.Value, ALL_MISTAKES.UCREPETEDNAME));
                    }
                }
                var firstWord = precedentName.Key.Split(' ')[0];
                if (string.IsNullOrEmpty(precedentName.Key.Trim()) || !char.IsUpper(precedentName.Key[0]) || !IsVerb(firstWord))
                {
                    Dictionary<string, Element> errorElements = new Dictionary<string, Element>();
                    foreach (var el in elements.Values)
                    {
                        if (el.Type == ElementTypes.Precedent && el.Name == precedentName.Key)
                            errorElements.Add(el.Id, el);
                    }

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Имя прецедента не представлено в виде действия, начинаясь с заглавной буквы: {precedentName.Key}",
                            element.Value, ALL_MISTAKES.UCBIGLETTER));
                    }
                }
            }

            foreach (var precedent in precedents)
            {
                bool haveAssociation = HaveConnection(precedent.Value.Id, ElementTypes.Association),
                    haveGeneralization = HaveConnection(precedent.Value.Id, ElementTypes.Generalization),
                    haveExtendsion = HaveConnection(precedent.Value.Id, ElementTypes.Extend),
                    haveIncluding = HaveConnection(precedent.Value.Id, ElementTypes.Include);

                if (!haveAssociation && !haveGeneralization && !haveExtendsion && !haveIncluding)
                {
                    mistakes.Add(UCMistakeFactory.Create(
                        MistakesTypes.ERROR,
                        $"Прецедент должен иметь связь с актором в виде ассоциации," +
                        $" либо иметь отношения расширения," +
                        $" дополнения или включения с другими прецедентами: {precedent.Value.Name}",
                        precedent.Value, ALL_MISTAKES.UCASSOSIATION));
                }

                if (haveExtendsion)
                {
                    bool havePoint = elements.Where(element =>
                    {
                        if (element.Value.Type != ElementTypes.ExtensionPoint) return false;
                        if (((Arrow)element.Value).To.Equals(precedent.Key))
                            return true;
                        return false;
                    }).Count() > 0;
                    bool extended = elements.Where(element =>
                    {
                        if (element.Value.Type != ElementTypes.Extend) return false;
                        if (((Arrow)element.Value).To.Equals(precedent.Key))
                            return true;
                        return false;
                    }).Count() > 0;

                    if (extended && !havePoint)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствие точки расширения у прецедента с отношением расширения: {precedent.Value.Name}",
                            precedent.Value, ALL_MISTAKES.UCNOPRECEDENTDOT));
                    }
                }

                if (haveIncluding)
                {
                    var includesFrom = elements
                        .Where(element =>
                        {
                            if (element.Value.Type != ElementTypes.Include) return false;
                            if (((Arrow)element.Value).From.Equals(precedent.Key))
                                return true;
                            return false;
                        });

                    var includesTo = elements
                        .Where(element =>
                        {
                            if (element.Value.Type != ElementTypes.Include) return false;
                            if (((Arrow)element.Value).To.Equals(precedent.Key))
                                return true;
                            return false;
                        });

                    if (includesFrom.Count() > 0 && includesFrom.Count() < 2)
                        mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.WARNING,
                           $"Прецедент включает всего один прецедент: {precedent.Value.Name}",
                           precedent.Value, ALL_MISTAKES.UCONLYONEPRECEDENT));

                    if (includesFrom.Count() > 0 && includesTo.Count() > 0)
                        mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.WARNING,
                           $"Злоупотребление отношением включения: {precedent.Value.Name}",
                           precedent.Value, ALL_MISTAKES.UCINCLUDE));
                }
            }
        }
        #endregion

        #region Support Functions
        private bool HaveConnection(string id, string type)
        {
            Dictionary<string, Element> assoc = new Dictionary<string, Element>();
            foreach(var el in elements.Values)
            {
                if (el.Type == type)
                    assoc.Add(el.Id, el);
            }
            return assoc.Where(a =>
            {
                if (((Arrow)a.Value).To.Equals(id) ||
                ((Arrow)a.Value).From.Equals(id))
                    return true;
                return false;
            }).Count() > 0;
        }

        private bool IsNoun(string name)
        {
            return !IsVerb(name);
        }

        private bool IsVerb(string name)
        {
            return name.EndsWith("те") || name.EndsWith("чи") || name.EndsWith("ти") || name.EndsWith("ть") || name.EndsWith("чь") || name.EndsWith("т");
        }
        #endregion
    }
}
