﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace LanExchange
{
    /// <summary>
    /// Summary description for InputBox.
    /// </summary>
    [ToolboxItem(true)]
    public class CInputBox : System.ComponentModel.Component
    {
        protected string _prompt = string.Empty;
        protected string _caption = string.Empty;
        protected string _errorMsgOnEmpty;
        private InputBoxForm inputBox;

        public CInputBox(System.ComponentModel.IContainer container)
        {
            container.Add(this);
        }

        public CInputBox() { }

        /// <summary>
        /// Property Prompt (string)
        /// </summary>
        [DefaultValue("")]
        public string Prompt
        {
            get { return _prompt; }
            set { _prompt = value; }
        }

        /// <summary>
        /// Property Caption (string)
        /// </summary>
        [DefaultValue("")]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        /// <summary>
        /// Property ErrorMsgOnEmpty (string)
        /// </summary>
        [DefaultValue("")]
        public string ErrorMsgOnEmpty
        {
            get { return _errorMsgOnEmpty; }
            set { _errorMsgOnEmpty = value; }
        }

        /// <summary>
        /// Shows input-box
        /// </summary>
        /// <param name="defText">Default text for InputBox</param>
        /// <returns>null if Cancel has been pressed or string</returns>
        public virtual string Ask(string defText, bool allow_empty)
        {
            if (inputBox == null)
                inputBox = new InputBoxForm();

            if (_caption != "")
                inputBox.Text = _caption;
            else
                inputBox.Text = Application.ProductName;

            inputBox.Prepare(_prompt, _errorMsgOnEmpty, defText, allow_empty);

            DialogResult res = inputBox.ShowDialog();
            if (res != DialogResult.OK)
                return null;
            else
                return inputBox.txtInputText.Text.Trim();
        }

        internal string Ask(string caption, string prompt, string defText, bool allow_empty)
        {
            this._caption = caption;
            this._prompt = prompt;
            return Ask(defText, allow_empty);
        }
    }
}
