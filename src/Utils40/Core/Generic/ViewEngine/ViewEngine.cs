using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using int32.Utils.Core.Generic.Singleton;

namespace int32.Utils.Core.Generic.ViewEngine
{
    public class  ViewEngine : Singleton<ViewEngine>
    {
        /// <summary>
        /// Compiled Regex for single substitutions
        /// </summary>
        private static readonly Regex SingleSubstitutionsRegEx = new Regex(@"@(?<Encode>!)?Model(?:\.(?<ParameterName>[a-zA-Z0-9-_]+))*;?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled Regex for context subsituations
        /// </summary>
        private static readonly Regex ContextSubstitutionsRegEx = new Regex(@"@(?<Encode>!)?Context(?:\.(?<ParameterName>[a-zA-Z0-9-_]+))*;?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled Regex for each blocks
        /// </summary>
        private static readonly Regex EachSubstitutionRegEx = new Regex(@"@Each(?:\.(?<ModelSource>(Model|Context)+))?(?:\.(?<ParameterName>[a-zA-Z0-9-_]+))*;?(?<Contents>.*?)@EndEach;?", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Compiled Regex for each block current substitutions
        /// </summary>
        private static readonly Regex EachItemSubstitutionRegEx = new Regex(@"@(?<Encode>!)?Current(?:\.(?<ParameterName>[a-zA-Z0-9-_]+))*;?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled Regex for if blocks
        /// </summary>
        private static readonly Regex ConditionalSubstitutionRegEx = new Regex(@"@If(?<Not>Not)?(?<Null>Null)?(?:\.(?<ModelSource>(Model|Context)+))?(?:\.(?<ParameterName>[a-zA-Z0-9-_]+))+;?(?<Contents>.*?)@EndIf;?", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Compiled regex for partial blocks
        /// </summary>
        private static readonly Regex PartialSubstitutionRegEx = new Regex(@"@Partial\['(?<ViewName>.+)'(?<Model>.[ ]?Model(?:\.(?<ParameterName>[a-zA-Z0-9-_]+))*)?\];?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled RegEx for section block declarations
        /// </summary>
        private static readonly Regex SectionDeclarationRegEx = new Regex(@"@Section\[\'(?<SectionName>.+?)\'\];?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled RegEx for section block contents
        /// </summary>
        private static readonly Regex SectionContentsRegEx = new Regex(@"(?:@Section\[\'(?<SectionName>.+?)\'\];?(?<SectionContents>.*?)@EndSection;?)", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Compiled RegEx for master page declaration
        /// </summary>
        private static readonly Regex MasterPageHeaderRegEx = new Regex(@"^(?:@Master\[\'(?<MasterPage>.+?)\'\]);?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled RegEx for path expansion
        /// </summary>
        private static readonly Regex PathExpansionRegEx = new Regex(@"(?:@Path\[\'(?<Path>.+?)\'\]);?", RegexOptions.Compiled);

        /// <summary>
        /// Compiled RegEx for the CSRF anti forgery token
        /// </summary>
        private static readonly Regex AntiForgeryTokenRegEx = new Regex(@"@AntiForgeryToken;?", RegexOptions.Compiled);

        /// <summary>
        /// View engine transform processors
        /// </summary>
        private readonly List<Func<string, object, string>> processors;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperSimpleViewEngine"/> class.
        /// </summary>
        public ViewEngine()
        {
            this.processors = new List<Func<string, object, string>>
                {
                    this.PerformSingleSubstitutions,
                    this.PerformEachSubstitutions,
                    this.PerformConditionalSubstitutions   
                };
        }

        /// <summary>
        /// Renders a template
        /// </summary>
        /// <param name="template">The template to render.</param>
        /// <param name="model">The model to user for rendering.</param>
        /// <param name="host">The view engine host</param>
        /// <returns>A string containing the expanded template.</returns>
        public string Render(string template, dynamic model)
        {
            return processors.Aggregate(template, (current, processor) => processor(current, model ?? new object()));
        }

        /// <summary>
        /// Performs single @Model.PropertyName substitutions.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="model">The model.</param>
        /// <param name="host">View engine host</param>
        /// <returns>Template with @Model.PropertyName blocks expanded.</returns>
        private string PerformSingleSubstitutions(string template, object model)
        {
            return SingleSubstitutionsRegEx.Replace(
                template,
                m =>
                {
                    var properties = GetCaptureGroupValues(m, "ParameterName");

                    var substitution = GetPropertyValueFromParameterCollection(model, properties);

                    if (!substitution.Item1)
                    {
                        return "[ERR!]";
                    }

                    if (substitution.Item2 == null)
                    {
                        return string.Empty;
                    }

                    return substitution.Item2.ToString();
                });
        }

        /// <summary>
        /// Performs @Each.PropertyName substitutions
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="model">The model.</param>
        /// <param name="host">View engine host</param>
        /// <returns>Template with @Each.PropertyName blocks expanded.</returns>
        private string PerformEachSubstitutions(string template, object model)
        {
            return EachSubstitutionRegEx.Replace(
                template,
                m =>
                {
                    var properties = GetCaptureGroupValues(m, "ParameterName");

                    var substitutionObject = GetPropertyValueFromParameterCollection(model, properties);

                    if (substitutionObject.Item1 == false)
                    {
                        return "[ERR!]";
                    }

                    if (substitutionObject.Item2 == null)
                    {
                        return string.Empty;
                    }

                    var substitutionEnumerable = substitutionObject.Item2 as IEnumerable;
                    if (substitutionEnumerable == null)
                    {
                        return "[ERR!]";
                    }

                    var contents = m.Groups["Contents"].Value.Trim();

                    return substitutionEnumerable.Cast<object>().Aggregate(string.Empty, (current, item) => current + ReplaceCurrentMatch(contents, item));
                });
        }

        /// <summary>
        /// Expand a @Current match inside an @Each iterator
        /// </summary>
        /// <param name="contents">Contents of the @Each block</param>
        /// <param name="item">Current item from the @Each enumerable</param>
        /// <param name="host">View engine host</param>
        /// <returns>String result of the expansion of the @Each.</returns>
        private string ReplaceCurrentMatch(string contents, object item)
        {
            return EachItemSubstitutionRegEx.Replace(
                contents,
                eachMatch =>
                {
                    if (string.IsNullOrEmpty(eachMatch.Groups["ParameterName"].Value))
                    {
                        return item.ToString();
                    }

                    var properties = GetCaptureGroupValues(eachMatch, "ParameterName");

                    var substitution = GetPropertyValueFromParameterCollection(item, properties);

                    if (!substitution.Item1)
                    {
                        return "[ERR!]";
                    }

                    if (substitution.Item2 == null)
                    {
                        return string.Empty;
                    }

                    return substitution.Item2.ToString();
                });
        }

        /// <summary>
        /// Performs @If.PropertyName and @IfNot.PropertyName substitutions
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="model">The model.</param>
        /// <param name="host">View engine host</param>
        /// <returns>Template with @If.PropertyName @IfNot.PropertyName blocks removed/expanded.</returns>
        private string PerformConditionalSubstitutions(string template, object model)
        {
            var result = template;

            result = ConditionalSubstitutionRegEx.Replace(
                result,
                m =>
                {
                    var properties = GetCaptureGroupValues(m, "ParameterName");

                    var predicateResult = GetPredicateResult(model, properties, m.Groups["Null"].Value == "Null");

                    if (m.Groups["Not"].Value == "Not")
                    {
                        predicateResult = !predicateResult;
                    }

                    return predicateResult ? m.Groups["Contents"].Value.Trim() : string.Empty;
                });

            return result;
        }

        #region helpers 

        /// <summary>
        /// <para>
        /// Gets a property value from the given model.
        /// </para>
        /// <para>
        /// Anonymous types, standard types and ExpandoObject are supported.
        /// Arbitrary dynamics (implementing IDynamicMetaObjectProvicer) are not, unless
        /// they also implement IDictionary string, object for accessing properties.
        /// </para>
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">The property name to evaluate.</param>
        /// <returns>Tuple - Item1 being a bool for whether the evaluation was sucessful, Item2 being the value.</returns>
        /// <exception cref="ArgumentException">Model type is not supported.</exception>
        private static Tuple<bool, object> GetPropertyValue(object model, string propertyName)
        {
            if (model == null || string.IsNullOrEmpty(propertyName))
            {
                return new Tuple<bool, object>(false, null);
            }

            if (!typeof(IDynamicMetaObjectProvider).IsAssignableFrom(model.GetType()))
            {
                return StandardTypePropertyEvaluator(model, propertyName);
            }

            if (typeof(IDictionary<string, object>).IsAssignableFrom(model.GetType()))
            {
                return DynamicDictionaryPropertyEvaluator(model, propertyName);
            }

            throw new ArgumentException("model must be a standard type or implement IDictionary<string, object>", "model");
        }

        /// <summary>
        /// A property extractor for standard types.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Tuple - Item1 being a bool for whether the evaluation was sucessful, Item2 being the value.</returns>
        private static Tuple<bool, object> StandardTypePropertyEvaluator(object model, string propertyName)
        {
            var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            var property =
                properties.Where(p => string.Equals(p.Name, propertyName, StringComparison.InvariantCulture)).
                FirstOrDefault();

            return property == null ? new Tuple<bool, object>(false, null) : new Tuple<bool, object>(true, property.GetValue(model, null));
        }

        /// <summary>
        /// A property extractor designed for ExpandoObject, but also for any
        /// type that implements IDictionary string object for accessing its
        /// properties.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Tuple - Item1 being a bool for whether the evaluation was sucessful, Item2 being the value.</returns>
        private static Tuple<bool, object> DynamicDictionaryPropertyEvaluator(object model, string propertyName)
        {
            var dictionaryModel = (IDictionary<string, object>)model;

            object output;
            return !dictionaryModel.TryGetValue(propertyName, out output) ? new Tuple<bool, object>(false, null) : new Tuple<bool, object>(true, output);
        }

        /// <summary>
        /// Gets an IEnumerable of capture group values
        /// </summary>
        /// <param name="m">The match to use.</param>
        /// <param name="groupName">Group name containing the capture group.</param>
        /// <returns>IEnumerable of capture group values as strings.</returns>
        private static IEnumerable<string> GetCaptureGroupValues(Match m, string groupName)
        {
            return m.Groups[groupName].Captures.Cast<Capture>().Select(c => c.Value);
        }

        /// <summary>
        /// Gets a property value from a collection of nested parameter names
        /// </summary>
        /// <param name="model">The model containing properties.</param>
        /// <param name="parameters">A collection of nested parameters (e.g. User, Name</param>
        /// <returns>Tuple - Item1 being a bool for whether the evaluation was sucessful, Item2 being the value.</returns>
        private static Tuple<bool, object> GetPropertyValueFromParameterCollection(object model, IEnumerable<string> parameters)
        {
            if (parameters == null)
            {
                return new Tuple<bool, object>(true, model);
            }

            var currentObject = model;

            foreach (var parameter in parameters)
            {
                var currentResult = GetPropertyValue(currentObject, parameter);

                if (currentResult.Item1 == false)
                {
                    return new Tuple<bool, object>(false, null);
                }

                currentObject = currentResult.Item2;
            }

            return new Tuple<bool, object>(true, currentObject);
        }

        /// <summary>
        /// Gets the predicate result for an If or IfNot block
        /// </summary>
        /// <param name="item">The item to evaluate</param>
        /// <param name="properties">Property list to evaluate</param>
        /// <param name="nullCheck">Whether to check for null, rather than straight boolean</param>
        /// <returns>Bool representing the predicate result</returns>
        private static bool GetPredicateResult(object item, IEnumerable<string> properties, bool nullCheck)
        {
            var substitutionObject = GetPropertyValueFromParameterCollection(item, properties);

            if (substitutionObject.Item1 == false && properties.Last().StartsWith("Has"))
            {
                var newProperties =
                    properties.Take(properties.Count() - 1).Concat(new[] { properties.Last().Substring(3) });

                substitutionObject = GetPropertyValueFromParameterCollection(item, newProperties);

                return GetHasPredicateResultFromSubstitutionObject(substitutionObject.Item2);
            }

            return GetPredicateResultFromSubstitutionObject(substitutionObject.Item2, nullCheck);
        }

        /// <summary>
        /// Returns the predicate result if the substitionObject is a valid bool
        /// </summary>
        /// <param name="substitutionObject">The substitution object.</param>
        /// <param name="nullCheck"></param>
        /// <returns>Bool value of the substitutionObject, or false if unable to cast.</returns>
        private static bool GetPredicateResultFromSubstitutionObject(object substitutionObject, bool nullCheck)
        {
            if (nullCheck)
            {
                return substitutionObject == null;
            }

            var predicateResult = false;
            var substitutionBool = substitutionObject as bool?;
            if (substitutionBool != null)
            {
                predicateResult = substitutionBool.Value;
            }

            return predicateResult;
        }

        /// <summary>
        /// Returns the predicate result if the substitionObject is a valid ICollection
        /// </summary>
        /// <param name="substitutionObject">The substitution object.</param>
        /// <returns>Bool value of the whether the ICollection has items, or false if unable to cast.</returns>
        private static bool GetHasPredicateResultFromSubstitutionObject(object substitutionObject)
        {
            var predicateResult = false;

            var substitutionCollection = substitutionObject as ICollection;
            if (substitutionCollection != null)
            {
                predicateResult = substitutionCollection.Count != 0;
            }

            return predicateResult;
        }

        #endregion
    }
}
