using System;
using System.ComponentModel;

namespace WMIViewer
{
    public sealed class GenericPropertyDescriptor<T> : PropertyDescriptor
    {
        private T m_Value;

        public GenericPropertyDescriptor(string name, Attribute[] attributes)
            : base(name, attributes)
        {
        }

        public GenericPropertyDescriptor(string name, T value, Attribute[] attributes)
            : base(name, attributes)
        {
            m_Value = value;
        }

        public GenericPropertyDescriptor(MemberDescriptor description)
            : base(description)
        {
            
        }

        public GenericPropertyDescriptor(MemberDescriptor description, Attribute[] attributes)
            : base(description, attributes)
        {
            
        }
         

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(GenericPropertyDescriptor<T>);
            }
        }

        public override object GetValue(object component)
        {
            return m_Value;
        }

        public override bool IsReadOnly
        {
            get
            {
                return Array.Exists(AttributeArray, attr => attr is ReadOnlyAttribute);
            }
        }

        public override Type PropertyType
        {
            get
            {
                return typeof(T);
            }
        }

        public override void ResetValue(object component)
        {
            SetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            m_Value = (T)value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}