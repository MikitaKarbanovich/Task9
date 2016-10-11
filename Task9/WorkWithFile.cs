using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task9
{
    public static class WorkWithFile
    {
        public static Type[] GetTypes(string path)
        {
            List<Type> types = new List<Type>();
            Assembly assembly = Assembly.LoadFrom(path);
            Type  [] reflectedTypes = assembly.GetTypes();
            return reflectedTypes;
        }
        public static MethodInfo[] GetMethods(Type type)
        {
            var methods = type.GetMethods();
            return methods;
        }
        public static PropertyInfo[] GetProperties(Type type)
        {
            var properties = type.GetProperties();
            return properties;
        }
        public static FieldInfo[] GetFields(Type type)
        {
            var fields = type.GetFields();
            return fields;
        }
    }
}
