﻿using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsMethod : ClassMemberBase, ICsCodeMaker, ICsClassMember
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        public CsMethod()
        {
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa metody</param>
        /// </summary>
        public CsMethod(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa metody</param>
        ///     <param name="resultType"></param>
        /// </summary>
        public CsMethod(string name, string resultType)
        {
            Name = name;
            ResultType = resultType;
        }

        public CsMethodParameter AddParam(string name, string type, string description = null)
        {
            var parameter = new CsMethodParameter(name, type, description);
            _parameters.Add(parameter);
            return parameter;
        }


        /// <summary>
        ///     Tworzy kod
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void MakeCode(ICodeWriter writer)
        {
            WriteMethodDescription(writer);

            foreach (var i in Attributes)
                writer.WriteLine("[ {0} ]", i);

            var query = from i in _parameters
                        select string.Format("{2}{0} {1}", i.Type, i.Name, i.UseThis ? "this " : "");
            var mDefinition = string.Format("{0}({1})",
                string.Join(" ", GetMethodAttributes()),
                string.Join(", ", query));
            if (IsAbstract && !IsConstructor)
            {
                writer.WriteLine(mDefinition + ";");
                return;
            }
            if (IsConstructor)
                writer.OpenConstructor(mDefinition, _baseConstructorCall);
            else
                writer.Open(mDefinition);

            query = from i in _body.Split('\r', '\n')
                    where !string.IsNullOrEmpty(i)
                    select i.TrimEnd();
            foreach (var i in query)
                writer.WriteLine(i);

            writer.Close();
        }

        private void WriteMethodDescription(ICodeWriter writer)
        {
            var anyParameterHasDescription = _parameters.Any(a => !string.IsNullOrEmpty(a.Description));
            var hasMethodDescription = !string.IsNullOrEmpty(Description);
            if (!hasMethodDescription && !anyParameterHasDescription) return;
            if (hasMethodDescription)
            {
                writer.WriteLine("/// <summary>");
                writer.WriteLine("/// " + Description.XmlEncode());
                writer.WriteLine("/// </summary>");
            }
            foreach (var i in _parameters)
                writer.WriteLine("/// <param name=\"{0}\">{1}</param>", i.Name.XmlEncode(),
                    i.Description.XmlEncode());
        }

        private string[] GetMethodAttributes()
        {
            var a = new List<string>();
            if (!(IsConstructor && IsStatic))
                if (Visibility != Visibilities.InterfaceDefault)
                    a.Add(Visibility.ToString().ToLower());
            if (IsStatic)
                a.Add("static");
            if (!IsConstructor)
            {
                if (IsAbstract)
                    a.Add("abstract");
                if (IsOverride)
                    a.Add("override");
                a.Add(_resultType);
            }
            a.Add(_name);
            return a.ToArray();
        }

        /// <summary>
        ///     Nazwa metody
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _name = value;
            }
        }


        /// <summary>
        /// </summary>
        public string ResultType
        {
            get { return _resultType; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _resultType = value;
            }
        }

        /// <summary>
        /// </summary>
        public List<CsMethodParameter> Parameters
        {
            get { return _parameters; }
            set
            {
                if (value == null) value = new List<CsMethodParameter>();
                _parameters = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// </summary>
        public bool IsOverride { get; set; }

        /// <summary>
        ///     Czy konstruktor
        /// </summary>
        public bool IsConstructor { get; set; }

        /// <summary>
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// </summary>
        public string Body
        {
            get { return _body; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _body = value;
            }
        }

        /// <summary>
        ///     wywołanie kontruktora bazowego
        /// </summary>
        public string BaseConstructorCall
        {
            get { return _baseConstructorCall; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _baseConstructorCall = value;
            }
        }

        private string _name = string.Empty;
        private string _resultType = "void";
        private List<CsMethodParameter> _parameters = new List<CsMethodParameter>();
        private string _body = string.Empty;
        private string _baseConstructorCall = string.Empty;
    }
}