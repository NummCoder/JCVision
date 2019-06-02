using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VisionUtil
{
   public class CreatingHelper<T>
    {
        private static T conten;

        private static object lockObj = new object();
        /// <summary>
        /// 使用无参的构造函数创建单例
        /// </summary>
        /// <returns></returns>
        public static T GetSingleObject()
        {
            if (conten==null)
            {
                lock (lockObj)
                {
                    if (conten==null)
                    {
                        conten = Activator.CreateInstance<T>();
                    }
                }
            }
            return conten;
        }

        /// <summary>
        /// 使用反射调用无参构造创建单例
        /// </summary>
        /// <param name="assemblyPath">程序集文件绝对路径</param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static T CreateInstance(string assemblyPath, string nameSpace, string className)
        {
            try
            {
                string fullName = nameSpace + "." + className;//命名空间.类型名
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                object ect = assembly.CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
                return (T)ect;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }
        /// <summary>
        /// 使用反射调用带参数的构造函数创建单例
        /// </summary>
        /// <param name="nameSpace">  类似"WorldLib.Communication" </param>
        /// <param name="className">  类的名称</param>
        /// <param name="parameters"> 参数数组</param>
        /// <returns></returns>
        public static T CreateInstance(string nameSpace, string className, object[] parameters)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string fullName = nameSpace + "." + className;//命名空间.类型名
                Type type = assembly.GetType(fullName);
                object ect = Activator.CreateInstance(type, parameters);//使用系统激活器创建对象
                return (T)ect;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }

        public static T CreateInstance(string dllFilePath,string nameSpace, string className, object[] parameters)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllFilePath);
                string fullName = nameSpace + "." + className;//命名空间.类型名
                Type type = assembly.GetType(fullName);
                object ect = Activator.CreateInstance(type, parameters);//使用系统激活器创建对象
                return (T)ect;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }
    }
}
