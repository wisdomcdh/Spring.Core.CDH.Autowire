using System;
using System.Globalization;
using System.Reflection;

namespace Test.TestHelp
{
    public class CustomPropertyInfo : PropertyInfo
    {
        #region Custom

        public Type C_PropertyType { get; set; }
        public PropertyAttributes C_Attributes { get; set; }
        public bool C_CanRead { get; set; }
        public bool C_CanWrite { get; set; }
        public string C_Name { get; set; }
        public Type C_DeclaringType { get; set; }
        public Type C_ReflectedType { get; set; }
        public override Type PropertyType { get { return C_PropertyType; } }
        public Func<bool, MethodInfo[]> CM_GetAccessors { get; set; }
        public Func<bool, object[]> CM_GetCustomAttributes { get; set; }
        public Func<Type, bool, object[]> CM_GetCustomAttributes2 { get; set; }

        #endregion Custom

        public override PropertyAttributes Attributes { get { return C_Attributes; } }

        public override bool CanRead { get { return C_CanRead; } }

        public override bool CanWrite { get { return C_CanWrite; } }

        public override string Name { get { return C_Name; } }

        public override Type DeclaringType { get { return C_DeclaringType; } }

        public override Type ReflectedType { get { return C_ReflectedType; } }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return CM_GetAccessors(nonPublic);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return CM_GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return CM_GetCustomAttributes2(attributeType, inherit);
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