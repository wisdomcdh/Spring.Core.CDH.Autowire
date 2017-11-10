using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spring.Core.CDH.Autowire
{
    internal class ForcedAutowirePropertyInfo : PropertyInfo
    {
        string _name;
        Type _propertyType;
        Attribute[]
        AutowireAttribute _autowireAttribute;

        public override PropertyAttributes Attributes
        {
            get
            {
                return PropertyAttributes.None;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override Type DeclaringType
        {
            get
            {
                return typeof(SpringAutowire);
            }
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return _propertyType;
            }
        }

        public override Type ReflectedType
        {
            get
            {
                return _propertyType;
            }
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new[] { _autowireAttribute };
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
